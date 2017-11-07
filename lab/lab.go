package lab

import (
	"fmt"
	"log"
	"os"
	"sort"
	"strings"
	"time"

	"github.com/dc0d/asterisk/astrobserve"
	"github.com/dc0d/asterisk/lab/celestial"
	pers "github.com/dc0d/persical"
	"gitlab.com/dc0d/gist/2017/05/gistsweph/def"
	"gitlab.com/dc0d/gist/2017/05/gistsweph/def/planet"
	"gitlab.com/dc0d/gist/2017/05/gistsweph/def/position"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

type PlanetPair struct {
	Fst astrobserve.Aster
	Snd astrobserve.Aster
}

func Moments(pos position.Position, startDate, endDate time.Time, step float64, stop chan struct{}) (moments, updates chan *celestial.Moment) {
	charts := Charts(pos, startDate, endDate, step, stop)
	moments = make(chan *celestial.Moment)
	updates = make(chan *celestial.Moment)
	go func() {
		var previous *celestial.Moment
	MOMENTS_LOOP:
		for {
			select {
			case c, ok := <-charts:
				if !ok {
					break MOMENTS_LOOP
				} else {
					m := &celestial.Moment{Chart: *c}
					m.Id = bson.NewObjectId()
					pyear, pmonth, pday := pers.GregorianToPersian(c.Time.Year(), int(c.Time.Month()), c.Time.Day())
					pm := pers.PersianMonth(pmonth)
					m.PersianMonth = pm

					if previous != nil {
						// calculate/build moment m

						// 1. fill aspects
						planetList := m.Chart.Planets()
						var cross []PlanetPair
						for i := 0; i <= len(planetList)-2; i++ {
							i := i
							for j := i + 1; j <= len(planetList)-1; j++ {
								cross = append(cross, PlanetPair{planetList[i], planetList[j]})
							}
						}

						for _, pair := range cross {
							pair := pair
							fst := astrobserve.CeLon{pair.Fst.Celestial, pair.Fst.AsterBook.Longitude()}
							snd := astrobserve.CeLon{pair.Snd.Celestial, pair.Snd.AsterBook.Longitude()}
							aspect, peak := astrobserve.AspectOf(fst, snd)

							if aspect == astrobserve.Unaspected {
								continue
							}

							nasp := celestial.Aspected{}
							nasp.At = m.Chart.Time
							nasp.Flag = celestial.Start
							nasp.Aspect = aspect
							nasp.AtPeak = peak
							nasp.Participants = []planet.Planet{
								*pair.Fst.AsterBook.Celestial.Planet,
								*pair.Snd.AsterBook.Celestial.Planet,
							}

							m.Aspected = append(m.Aspected, nasp)
						}

						// 2. handle previous
						previousChanged := false
						for pi, pv := range previous.Aspected {
							pv := pv
							pi := pi

							repeated := false

							for ni, nv := range m.Aspected {
								nv := nv
								ni := ni
								if nv.CompareAspect(&pv) {
									if m.Aspected[ni].AtPeak != previous.Aspected[pi].AtPeak {
										if m.Aspected[ni].AtPeak == astrobserve.AtPeak {
											m.Aspected[ni].Flag = celestial.Start
										} else {
											m.Aspected[ni].Flag = celestial.End
										}
									} else {
										m.Aspected[ni].Flag = celestial.Ongoing
									}
									repeated = true
								}
							}

							if !repeated {
								previous.Aspected[pi].Flag = celestial.End
								previousChanged = true
							}
						}

						if previousChanged {
							// previous must get updated
							updates <- previous
						}
					}

					// feed moment
					moments <- m

					previous = m
				}
			case <-stop:
				break MOMENTS_LOOP
			}
		}

		close(moments)
		close(updates)
	}()
	return
}

func Charts(pos position.Position, startDate, endDate time.Time, step float64, stop chan struct{}) chan *astrobserve.ChartSet {
	charts := make(chan *astrobserve.ChartSet)
	go func() {
		current := startDate
	CHARTS_LOOP:
		for endDate.After(current) {
			cset := &astrobserve.ChartSet{}

			select {
			case charts <- cset.For(current, pos):
			case <-stop:
				break CHARTS_LOOP
			}

			current = current.Add(def.DurationFromSecond(step))
		}

		close(charts)
	}()
	return charts
}

func TestDb1() {
	start := time.Date(2015, time.March, 19, 0, 0, 0, 0, time.UTC)
	end := start.Add(def.DurationFromHour(24 * 35))
	p := position.Position{Lon: (51. + 17./60.), Lat: (35. + 44./60.)}
	stop := make(chan struct{})

	moments, updates := Moments(p, start, end, 10., stop)

	session, err := mgo.Dial("127.0.0.1:27072")
	if err != nil {
		panic(err)
	}
	defer session.Close()

	session.SetMode(mgo.Strong, true)
	db := session.DB("aster7")
	cl := db.C("moments")

	go func() {
		for {
			select {
			case um, ok := <-updates:
				if !ok {
					return
				} else {
					cl.UpdateId(um.Id, um)
				}
			case <-stop:
				return
			}
		}
	}()

TEST_LOOP:
	for {
		select {
		case m, ok := <-moments:
			if !ok {
				close(stop)
				break TEST_LOOP
			} else {
				cl.Insert(m)
			}
		}
	}

	time.Sleep(time.Second * 5)

	index := mgo.Index{Key: []string{"aspected.multi.change.flag"}}
	err = cl.EnsureIndex(index)
	if err != nil {
		log.Panicln(err)
	}

	index = mgo.Index{Key: []string{"aspected.atpeak"}}
	err = cl.EnsureIndex(index)
	if err != nil {
		log.Panicln(err)
	}

	time.Sleep(time.Second * 5)
}

func TestReport1() {
	session, err := mgo.Dial("127.0.0.1:27072")
	if err != nil {
		log.Panicln(err)
	}
	defer session.Close()

	session.SetMode(mgo.Strong, true)
	db := session.DB("aster7")
	cl := db.C("moments")

	f, err := os.Create("test.html")
	if err != nil {
		log.Panicln(err)
	}
	defer f.Close()

	f.Write([]byte(`
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>aspects</title>
	<style type="text/css">
		td {
            min-width: 140px;
        }
		
        .atpeak {
            background-color: lightblue;
        }

        .start {
            border-width: 3px;
            border-style: solid;
            border-color: darkgreen;
        }

        .end {
            border-width: 3px;
            border-style: solid;
            border-color: darkmagenta;
        }

        .alt1 {
            background-color: #ddd;
        }

        .alt2 {
            background-color: #e7efef;
        }
		
		a.tooltip {outline:none; }
		a.tooltip strong {line-height:30px;}
		a.tooltip:hover {text-decoration:none;} 
		a.tooltip span {
		    z-index:10;display:none; padding:14px 20px;
		    margin-top:-30px; margin-left:28px;
		    width:300px; line-height:16px;
		}
		a.tooltip:hover span{
		    display:inline; position:absolute; color:#111;
		    border:1px solid #DCA; background:#fffAF0;}
		.callout {z-index:20;position:absolute;top:30px;border:0;left:-12px;}
		    
		/*CSS3 extras*/
		a.tooltip span
		{
		    border-radius:4px;
		    box-shadow: 5px 5px 8px #CCC;
		}
    </style>
</head>`))
	f.Write([]byte(`
<body>
    <table>`))

	mitem := celestial.Moment{}

	planetList := PlanetList2()
	var cross [][]planet.Planet
	for i := 0; i <= len(planetList)-2; i++ {
		i := i
		for j := i + 1; j <= len(planetList)-1; j++ {
			j := j
			cross = append(cross, []planet.Planet{planetList[i], planetList[j]})
		}
	}
	tagMaker := func(pl []planet.Planet, apc astrobserve.Aspect) string {
		return fmt.Sprintf("%s.%v", TagPlanets(pl), apc)
	}
	tags := []string{}
	for _, pp := range cross {
		for _, aspo := range astrobserve.AspectsList {
			if aspo == astrobserve.Unaspected {
				continue
			}

			t := tagMaker(pp, aspo)
			tags = append(tags, t)
		}
	}
	divit := func(x interface{}) string {
		return fmt.Sprintf("<div>%v</div>", x)
	}
	cell := func(apc celestial.Aspected) (sr string) {
		cssClass := ""
		switch apc.Flag {
		case celestial.Start:
			cssClass += " start"
		case celestial.End:
			cssClass += " end"
		}
		if apc.AtPeak == astrobserve.AtPeak {
			cssClass += " atpeak"
		}

		sr += fmt.Sprintf("\n<td class='%s'>", cssClass)
		//sr += divit(TagPlanets(apc.Participants))

		sr += `
<a href="#" class="tooltip">
    ` + TagPlanets(apc.Participants) + `
    <span>
        ~~~~~
    </span>
</a>		
		`

		sr += divit(fmt.Sprintf("%v / %v", apc.Aspect, apc.AtPeak))
		sr += divit(fmt.Sprintf("%v", apc.Flag))

		sr += "\n</td>"
		return
	}

	cond1 := bson.M{"aspected.multi.change.flag": bson.M{"$in": []int{2, 4}}}
	//	cond2 := bson.M{"aspected.atpeak": int32(astrobserve.AtPeak)}
	//	cond3 := bson.M{"$or": []bson.M{cond2, cond1}}
	cond3 := cond1
	itr := cl.Find(cond3).Sort("_id").Iter()

	f.Write([]byte("<tr>"))
	f.Write([]byte("\n<td></td>"))
	for _, st := range tags {
		f.Write([]byte(fmt.Sprintf("\n<td>%s</td>", st)))
	}
	f.Write([]byte("</tr>"))

	prepareTime := func(t time.Time) string {
		pt := pers.GregorianToPersian(int32(t.Year()), int32(t.Month()), int32(t.Day()))
		tz := 3.5
		if pt.Month <= 6 {
			tz = 4.5
		}
		torig := t
		t = t.Add(def.DurationFromHour(tz))
		pt = pers.GregorianToPersian(int32(t.Year()), int32(t.Month()), int32(t.Day()))

		gstr := fmt.Sprintf("%04d-%02d-%02d", t.Year(), t.Month(), t.Day())
		pstr := fmt.Sprintf("%04d-%02d-%02d", pt.Year, pt.Month, pt.Day)
		tstr := fmt.Sprintf("%02d:%02d:%02d", t.Hour(), t.Minute(), t.Second())

		return fmt.Sprintf("%s %s (%s) %v (RAW: %v)", pstr, tstr, gstr, t.Weekday(), torig)
	}

	toggle := 1
	for itr.Next(&mitem) {
		toggle = 1 - toggle
		cssClass := fmt.Sprintf("alt%d", toggle+1)

		tstr := prepareTime(mitem.Chart.Time)
		f.Write([]byte(fmt.Sprintf("<tr class='%s' data-tstr='%s'>", cssClass, tstr)))

		f.Write([]byte(fmt.Sprintf("\n<td>%v</td>", tstr)))

		partMap := make(map[string]string)
		for _, st := range tags {
			partMap[st] = "\n<td></td>"
		}

		for _, vasp := range mitem.Aspected {
			tag := tagMaker(vasp.Participants, vasp.Aspect)
			partMap[tag] = strings.Replace(cell(vasp), "~~~~~", tstr, -1)
		}

		for _, st := range tags {
			f.Write([]byte(partMap[st]))
		}

		f.Write([]byte(`</tr>`))
		f.Write([]byte("\n"))
	}

	/*
		for itr.Next(&mitem) {
			f.Write([]byte(`<tr>`))

			f.Write([]byte(fmt.Sprintf("<td>%v</td>", mitem.Chart.Time)))

			for _, vasp := range mitem.Aspected {
				f.Write([]byte("\n<td>"))

				stl := []string{}

				for _, vpa := range vasp.Participants {
					stl = append(stl, fmt.Sprintf("%v", vpa))
				}

				f.Write([]byte(strings.Join(stl, ", ")))

				f.Write([]byte(fmt.Sprintf(" - %v / %v", vasp.Aspect, vasp.AtPeak)))
				f.Write([]byte(fmt.Sprintf(" - %v", vasp.Flag)))
				f.Write([]byte("\n</td>"))
			}

			f.Write([]byte(`</tr>`))
			f.Write([]byte("\n"))
		}
	*/

	f.Write([]byte(`
</table>
</body>
</html>`))
}

func ContainsPlanets(plist1, plist2 []planet.Planet) bool {
	set := map[planet.Planet]int32{}

	for _, np := range plist1 {
		set[np] = 1
	}
	for _, op := range plist2 {
		_, exists := set[op]
		if !exists {
			return false
		}
	}

	return true
}

func TagPlanets(plist []planet.Planet) string {
	ints := []int{}
	for _, p := range plist {
		ints = append(ints, int(p))
	}
	sort.Ints(ints)
	strs := []string{}
	for _, i := range ints {
		strs = append(strs, fmt.Sprintf("%v", planet.Planet(i)))
	}
	return strings.Join(strs, ".")
}

//type PlanetPair2 struct {
//	Fst planet.Planet
//	Snd planet.Planet
//}

func PlanetList2() []planet.Planet {
	return []planet.Planet{
		planet.Su,
		planet.Mo,
		planet.Me,
		planet.Ve,
		planet.Ma,
		planet.Ju,
		planet.Sa,
		planet.Ur,
		planet.Ne,
		planet.Pl,
		planet.Ra,
		planet.Ke,
	}
}

func TestReport2() {
	NL := "\r\n"

	session, err := mgo.Dial("127.0.0.1:27072")
	if err != nil {
		log.Panicln(err)
	}
	defer session.Close()

	session.SetMode(mgo.Strong, true)
	db := session.DB("aster7")
	cl := db.C("moments")

	f, err := os.Create("zoo-test.txt")
	if err != nil {
		log.Panicln(err)
	}
	defer f.Close()

	mitem := celestial.Moment{}

	planetList := PlanetList2()
	var cross [][]planet.Planet
	for i := 0; i <= len(planetList)-2; i++ {
		i := i
		for j := i + 1; j <= len(planetList)-1; j++ {
			j := j
			cross = append(cross, []planet.Planet{planetList[i], planetList[j]})
		}
	}
	tagMaker := func(pl []planet.Planet, apc astrobserve.Aspect) string {
		return fmt.Sprintf("%s.%v", TagPlanets(pl), apc)
	}
	tags := []string{}
	for _, pp := range cross {
		for _, aspo := range astrobserve.AspectsList {
			if aspo == astrobserve.Unaspected {
				continue
			}

			t := tagMaker(pp, aspo)
			tags = append(tags, t)
		}
	}

	cond1 := bson.M{"aspected.multi.change.flag": bson.M{"$in": []int{2, 4}}}
	//	cond2 := bson.M{"aspected.atpeak": int32(astrobserve.AtPeak)}
	//	cond3 := bson.M{"$or": []bson.M{cond2, cond1}}
	cond3 := cond1
	itr := cl.Find(cond3).Sort("_id").Iter()

	var currentDay time.Time
	for itr.Next(&mitem) {
		rtime := mitem.Chart.Time.UTC().Add(time.Minute * 270)
		nDay := rtime.Add(time.Second * 86400).UTC().Truncate(time.Second * 86400)
		t := rtime
		tstr := fmt.Sprintf("%02d:%02d:%02d", t.Hour(), t.Minute(), t.Second())

		if nDay != currentDay {
			f.WriteString("================================================================================" + NL)
			currentDay = nDay

			pt := pers.GregorianToPersian(int32(t.Year()), int32(t.Month()), int32(t.Day()))
			gstr := fmt.Sprintf("%04d-%02d-%02d", t.Year(), t.Month(), t.Day())
			pstr := fmt.Sprintf("%04d-%02d-%02d", pt.Year, pt.Month, pt.Day)

			f.WriteString(fmt.Sprintf("%s %s %v"+NL+NL, pstr, gstr, t.Weekday()))
		}

		for _, vasp := range mitem.Aspected {
			if vasp.Flag != celestial.Start && vasp.Flag != celestial.End {
				continue
			}
			if vasp.Aspect == astrobserve.Unaspected {
				continue
			}

			signList := []string{}
			signCount := 0
		OUTER_SIGN:
			for _, pv := range vasp.Participants {
				for _, orp := range mitem.Chart.List() {
					if orp.AsterBook.Celestial.Planet != nil && *orp.AsterBook.Celestial.Planet == pv {
						sstr := fmt.Sprintf("%v in %v", *orp.AsterBook.Celestial.Planet, orp.AsterBook.Sign())
						signList = append(signList, sstr)
						signCount++
					}
					if signCount >= 2 {
						break OUTER_SIGN
					}
				}
			}

			signStr := strings.Join(signList, ", ")

			tag := tagMaker(vasp.Participants, vasp.Aspect)
			line := fmt.Sprintf("%-6s %-10s %-10s %-10s %-10s"+NL, vasp.Flag.String(), vasp.AtPeak.String(), tag, tstr, signStr)
			f.WriteString(line)
		}

		f.Write([]byte(NL))
	}
}
