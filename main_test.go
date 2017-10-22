package main

import (
	. "asterisk/astrobserve"
	"asterisk/config"
	. "asterisk/def"
	. "asterisk/def/flow"
	. "asterisk/def/julian"
	. "asterisk/def/planet"
	. "asterisk/def/point"
	. "asterisk/def/position"
	. "asterisk/def/sign"
	"fmt"
	"studio"
	"testing"
	"time"
)

func timeTrack(start time.Time, name string) {
	elapsed := time.Since(start)
	fmt.Printf("%s took %s\n", name, elapsed)
}

func TestChartSetFor(test *testing.T) {
	defer timeTrack(time.Now(), "ChartSet.For")

	done := make(chan bool, 1)
	go F3(done)
	<-done
}

func F3(done chan bool) {
	defer timeTrack(time.Now(), "For (of ChartSet)")
	t := time.Date(1976, time.January, 9, 23, 55, 30, 0, time.UTC)
	t = t.Add(DurationFromHour(-3.5))

	p := Position{Lon: (47. + 5./60.), Lat: (34. + 19./60.)}

	cset := &ChartSet{}
	cset.For(t, p)

	fmt.Println("----------")
	for _, pli := range cset.List() {
		v := pli
		fmt.Printf("%+v\n", v)
	}
	fmt.Println("----------")

	done <- true
}

func TestChartSet(test *testing.T) {
	done := make(chan bool, 1)
	go F2(done)
	<-done
}

func F2(done chan bool) {
	t := time.Date(1976, time.January, 9, 23, 55, 30, 0, time.UTC)
	t = t.Add(DurationFromHour(-3.5))
	jd := JulianDayOf(t)

	cset := &ChartSet{}

	fa := ForAscendant{Position: Position{Lon: (47. + 5./60.), Lat: (34. + 19./60.)}, JulianDay: jd, Asc: Angle(config.Asc), Answer: make(chan float64, 1)}
	AscendantChan <- fa
	ascd := <-fa.Answer
	ascBuff := Asc
	cel := Celestial{Angle: &ascBuff}
	a := Aster{cel, AsterBook{Celestial: cel, Site: Site{Longitude: &ascd}}}
	a.AsterBook.InitHouse(a.AsterBook.Sign())
	cset.Asc = a

	for _, pltx := range PlanetList {
		plt := pltx
		fp := ForPlanet{Planet: plt, JulianDay: jd, Answer: make(chan Point, 1)}
		PlanetChan <- fp
		pnt := <-fp.Answer
		cel := Celestial{Planet: &plt}

		a := &Aster{cel, AsterBook{Celestial: cel, Site: Site{Disposition: &Disposition{Position: pnt.Position, Flow: pnt.Flow}}}}
		a.AsterBook.InitHouse(cset.Asc.AsterBook.Sign())

		cset.SetPlanet(plt, a)
	}

	for _, pli := range cset.List() {
		v := pli
		fmt.Printf("%+v\n", v)
	}

	done <- true
}

func TestFunctions(test *testing.T) {
	done := make(chan bool, 1)
	go F1(done)
	<-done
}

func F1(done chan bool) {
	t := time.Date(1976, time.January, 9, 23, 55, 30, 0, time.UTC)
	t = t.Add(DurationFromHour(-3.5))
	jd := JulianDayOf(t)

	fp := ForPlanet{Planet: Su, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt := <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Mo, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Ma, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Me, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Ju, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Ve, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Sa, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Ra, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fp = ForPlanet{Planet: Ke, JulianDay: jd, Answer: make(chan Point, 1)}
	PlanetChan <- fp
	pnt = <-fp.Answer
	fmt.Printf("%+v\r\n", pnt)
	fmt.Println(DegreeString(DurationFromHour(SignDegree(pnt.Position.Lon))))

	fa := ForAscendant{Position: Position{Lon: (47. + 5./60.), Lat: (34. + 19./60.)}, JulianDay: jd, Asc: Angle(config.Asc), Answer: make(chan float64, 1)}
	AscendantChan <- fa
	ascd := <-fa.Answer
	fmt.Println(DegreeString(DurationFromHour(SignDegree(ascd))))

	done <- true
}

func TestLab(t *testing.T) {
	var p Planet
	fmt.Println(p)
}

func TestDurationFunctions(test *testing.T) {
	var dt1, dt2, dt3 time.Duration

	dt1 = DurationFromHour(1.)
	dt2 = DurationFromMinute(60.)
	dt3 = DurationFromSecond(3600.)

	if dt1 != dt2 || dt3 != dt2 {
		test.FailNow()
	}
}

func TestMod(test *testing.T) {
	var f1, f2 float64
	f1, f2 = 4.9, .7
	f3 := Mod(f1, f2)

	r := studio.Round(f3, .5, 3)

	if r != f2 {
		test.Fail()
	}
}

//func TestDone(test *testing.T) {
//	go func() {
//		stoping <- true
//		<-stopped
//	}()
//}

//var stoping = make(chan bool, 1)
//var stopped = make(chan bool, 1)

//func init() {
//	go func() {
//		select {
//		case <-stoping:
//		case <-time.After(time.Second * 7):
//		}

//		stop := ForStop{Stop: true, Answer: make(chan bool, 1)}
//		defer func() {
//			StopChan <- stop
//			<-stop.Answer
//			close(stopped)
//		}()
//	}()
//}
