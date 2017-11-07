package astrobserve

import (
	"fmt"
	"math"
	"time"

	"github.com/dc0d/asterisk"
	"github.com/dc0d/asterisk/observatory"
)

type Arc struct{ Start, End float64 }

func (a Arc) String() string {
	return fmt.Sprintf("{Start: %v, End: %v}", asterisk.Degree(a.Start), asterisk.Degree(a.End))
}

type Sub struct {
	Ruler asterisk.Planet
	Arc   Arc
}

type Nakshatra struct {
	Name  string
	Ruler asterisk.Planet
	Arc   Arc
	Subs  []Sub
}

// /*
// type weekLord struct {
// 	day  time.Weekday
// 	lord Planet
// }

// var WeekLords = []weekLord{
// 	weekLord{time.Saturday, Sa},
// 	weekLord{time.Sunday, Su},
// 	weekLord{time.Monday, Mo},
// 	weekLord{time.Tuesday, Ma},
// 	weekLord{time.Wednesday, Me},
// 	weekLord{time.Thursday, Ju},
// 	weekLord{time.Friday, Ve},
// }
// */

var Diptamsa = map[asterisk.Planet]float64{
	asterisk.Su: 15.,
	asterisk.Mo: 12.,
	asterisk.Me: 7.,
	asterisk.Ve: 7.,
	asterisk.Ma: 8.,
	asterisk.Ju: 9.,
	asterisk.Sa: 9.,
	asterisk.Ur: 5.,
	asterisk.Ne: 5.,
	asterisk.Pl: 5.,
	asterisk.Ra: 8.,
	asterisk.Ke: 8.,
}

// /*
// var TrirashiDay = map[Sign]Planet{
// 	Aries:       Su,
// 	Taurus:      Ve,
// 	Gemini:      Sa,
// 	Cancer:      Ve,
// 	Leo:         Ju,
// 	Virgo:       Mo,
// 	Libra:       Me,
// 	Scorpio:     Ma,
// 	Sagittarius: Sa,
// 	Capricorn:   Ma,
// 	Aquarius:    Ju,
// 	Pisces:      Mo,
// }
// */

// /*
// var TrirashiNight = map[Sign]Planet{
// 	Aries:       Ju,
// 	Taurus:      Mo,
// 	Gemini:      Me,
// 	Cancer:      Ma,
// 	Leo:         Su,
// 	Virgo:       Ve,
// 	Libra:       Sa,
// 	Scorpio:     Ve,
// 	Sagittarius: Sa,
// 	Capricorn:   Ma,
// 	Aquarius:    Ju,
// 	Pisces:      Mo,
// }
// */

// /*
// type Trinity int32

// const (
// 	Movable Trinity = 1
// 	Fixed   Trinity = 2
// 	Dual    Trinity = 3
// )

// var TrinityNature = map[Sign]Trinity{
// 	Aries:       Movable,
// 	Cancer:      Movable,
// 	Libra:       Movable,
// 	Capricorn:   Movable,
// 	Taurus:      Fixed,
// 	Leo:         Fixed,
// 	Scorpio:     Fixed,
// 	Aquarius:    Fixed,
// 	Gemini:      Dual,
// 	Virgo:       Dual,
// 	Sagittarius: Dual,
// 	Pisces:      Dual,
// }
// */

// /*
// type Element int32

// const (
// 	Fire  Element = 1
// 	Water Element = 2
// 	Air   Element = 3
// 	Earth Element = 4
// )

// var Elementals = map[Sign]Element{
// 	Aries:       Fire,
// 	Leo:         Fire,
// 	Sagittarius: Fire,
// 	Taurus:      Earth,
// 	Virgo:       Earth,
// 	Capricorn:   Earth,
// 	Gemini:      Air,
// 	Libra:       Air,
// 	Aquarius:    Air,
// 	Cancer:      Water,
// 	Scorpio:     Water,
// 	Pisces:      Water,
// }
// */

type RulingYears struct {
	Planet asterisk.Planet
	Years  int32
}

var RulingYearsList = []RulingYears{
	RulingYears{asterisk.Ke, 7},
	RulingYears{asterisk.Ve, 20},
	RulingYears{asterisk.Su, 6},
	RulingYears{asterisk.Mo, 10},
	RulingYears{asterisk.Ma, 7},
	RulingYears{asterisk.Ra, 18},
	RulingYears{asterisk.Ju, 16},
	RulingYears{asterisk.Sa, 19},
	RulingYears{asterisk.Me, 17},
}

// /*
// var Lordship = map[Sign]Planet{
// 	Aries:       Ma,
// 	Taurus:      Ve,
// 	Gemini:      Me,
// 	Cancer:      Mo,
// 	Leo:         Su,
// 	Virgo:       Me,
// 	Libra:       Ve,
// 	Scorpio:     Ma,
// 	Sagittarius: Ju,
// 	Capricorn:   Sa,
// 	Aquarius:    Sa,
// 	Pisces:      Ju,
// }
// */

var _nakshatraLen = 360. / 27.
var _subUnitLen = _nakshatraLen / 120.

type nakinf struct {
	name  string
	ruler asterisk.Planet
}

var nakshatraNameAndRuler = []nakinf{
	nakinf{"Ashvinī", asterisk.Ke},
	nakinf{"Bharanī", asterisk.Ve},
	nakinf{"Krittikā", asterisk.Su},
	nakinf{"Rohini", asterisk.Mo},
	nakinf{"Mrigashīrsha", asterisk.Ma},
	nakinf{"Ārdrā", asterisk.Ra},
	nakinf{"Punarvasu", asterisk.Ju},
	nakinf{"Pushya", asterisk.Sa},
	nakinf{"Āshleshā", asterisk.Me},
	nakinf{"Maghā", asterisk.Ke},
	nakinf{"Pūrva Phalgunī", asterisk.Ve},
	nakinf{"Uttara Phalgunī", asterisk.Su},
	nakinf{"Hasta", asterisk.Mo},
	nakinf{"Chitrā", asterisk.Ma},
	nakinf{"Svātī", asterisk.Ra},
	nakinf{"Vishākhā", asterisk.Ju},
	nakinf{"Anurādhā", asterisk.Sa},
	nakinf{"Jyeshtha", asterisk.Me},
	nakinf{"Mūla", asterisk.Ke},
	nakinf{"Pūrva Ashādhā", asterisk.Ve},
	nakinf{"Uttara Ashadha", asterisk.Su},
	nakinf{"Shravana", asterisk.Mo},
	nakinf{"Shravishthā", asterisk.Ma},
	nakinf{"Shatabhishā", asterisk.Ra},
	nakinf{"Pūrva Bhādrapadā", asterisk.Ju},
	nakinf{"Uttara Bhādrapadā", asterisk.Sa},
	nakinf{"Revatī", asterisk.Me},
}

func initNakshatras() []Nakshatra {
	nakshatras := []Nakshatra{}

	previousNakshatraArcEnd := 0.
	for _, nknf := range nakshatraNameAndRuler {
		name, ruler := nknf.name, nknf.ruler

		arc := Arc{Start: float64(previousNakshatraArcEnd), End: float64(previousNakshatraArcEnd + _nakshatraLen)}
		subs := []Sub{}

		previousSubArcEnd := float64(arc.Start)
		ignored := false
		for _, ry := range RulingYearsList {
			k, v := ry.Planet, ry.Years

			if !ignored && k != ruler {
				continue
			}
			ignored = true

			slen := float64(v) * _subUnitLen
			sub := Sub{Ruler: k, Arc: Arc{Start: float64(previousSubArcEnd), End: float64(previousSubArcEnd + slen)}}
			subs = append([]Sub{sub}, subs...)
			previousSubArcEnd = float64(sub.Arc.End)
		}
		ignored = false
		for _, ry := range RulingYearsList {
			k, v := ry.Planet, ry.Years

			if !ignored && k == ruler {
				continue
			}
			ignored = true

			slen := float64(v) * _subUnitLen
			sub := Sub{Ruler: k, Arc: Arc{Start: float64(previousSubArcEnd), End: float64(previousSubArcEnd + slen)}}
			subs = append([]Sub{sub}, subs...)
			previousSubArcEnd = float64(sub.Arc.End)
		}

		nak := Nakshatra{Name: name, Ruler: ruler, Arc: arc, Subs: subs}
		nakshatras = append([]Nakshatra{nak}, nakshatras...)
		previousNakshatraArcEnd = float64(nak.Arc.End)
	}

	return nakshatras
}

var Nakshatras = initNakshatras()

func FindNakshatra(lon float64) (nakshatra Nakshatra, sub Sub) {
	//fmt.Printf("%v\n",   asterisk.Degree(lon))
	for _, n := range Nakshatras {
		n := n
		if n.Arc.Start <= lon && lon < n.Arc.End {
			nakshatra = n
			break
		}
	}

	for _, subItem := range nakshatra.Subs {
		subItem := subItem
		if subItem.Arc.Start <= lon && lon < subItem.Arc.End {
			sub = subItem
			break
		}
	}

	return
}

type AtSign struct {
	Sign    asterisk.Sign
	float64 float64
}

type Dignity struct {
	ExaltatedAt, DebilitatedAt AtSign
}

// /*
// var Dignities = map[Planet]Dignity{
// 	Su: Dignity{ExaltatedAt: AtSign{Sign: Aries, float64: 10.}, DebilitatedAt: AtSign{Sign: Libra, float64: 10.}},
// 	Mo: Dignity{ExaltatedAt: AtSign{Sign: Taurus, float64: 3.}, DebilitatedAt: AtSign{Sign: Scorpio, float64: 3.}},
// 	Ma: Dignity{ExaltatedAt: AtSign{Sign: Capricorn, float64: 28.}, DebilitatedAt: AtSign{Sign: Cancer, float64: 28.}},
// 	Me: Dignity{ExaltatedAt: AtSign{Sign: Virgo, float64: 15.}, DebilitatedAt: AtSign{Sign: Pisces, float64: 15.}},
// 	Ju: Dignity{ExaltatedAt: AtSign{Sign: Cancer, float64: 5.}, DebilitatedAt: AtSign{Sign: Capricorn, float64: 5.}},
// 	Ve: Dignity{ExaltatedAt: AtSign{Sign: Pisces, float64: 27.}, DebilitatedAt: AtSign{Sign: Virgo, float64: 27.}},
// 	Sa: Dignity{ExaltatedAt: AtSign{Sign: Libra, float64: 20.}, DebilitatedAt: AtSign{Sign: Aries, float64: 20.}},
// 	Ra: Dignity{ExaltatedAt: AtSign{Sign: Gemini, float64: -1.}, DebilitatedAt: AtSign{Sign: Sagittarius, float64: -1.}},
// 	Ke: Dignity{ExaltatedAt: AtSign{Sign: Sagittarius, float64: -1.}, DebilitatedAt: AtSign{Sign: Gemini, float64: -1.}},
// }
// */

type Celestial struct {
	Planet *asterisk.Planet
	Angle  *observatory.Angle
}

func (c Celestial) String() string {
	if c.Planet != nil {
		return fmt.Sprintf("{Planet: %v}", c.Planet)
	}
	if c.Angle != nil {
		return fmt.Sprintf("{Angle: %v}", c.Angle)
	}
	return fmt.Sprintf("%+v", c)
}

type Disposition struct {
	Position asterisk.Position
	Flow     asterisk.Flow
}

func (d *Disposition) String() string {
	return fmt.Sprintf("Position: %+v, Flow: %+v", d.Position, d.Flow)
}

type Site struct {
	Longitude   *float64
	Disposition *Disposition
}

func (s Site) String() string {
	if s.Longitude != nil {
		return fmt.Sprintf("{Longitude: %s}", asterisk.DegreeString(asterisk.DurationFromHour(*s.Longitude)))
	}

	return fmt.Sprintf("{%+v}", s.Disposition)
}

type AsterBook struct {
	Celestial Celestial
	Site      Site

	nak        *Nakshatra
	sub        *Sub
	sign       *asterisk.Sign
	house      *asterisk.House
	lon        *float64
	signDegree *float64
	mansion    *IslamicMansion
}

func (ab AsterBook) String() string {
	return fmt.Sprintf("{Celestial: %+v, Site: %+v, Longitude: %s, Nakshatra: %s, Sign: %v, Sign   asterisk.Degree: %v, House: %v, Manstion: %v}",
		ab.Celestial,
		ab.Site,
		asterisk.Degree(ab.Longitude()),
		fmt.Sprintf("%s/%s (%s)", ab.Nakshatra().Ruler, ab.Sub().Ruler, ab.Nakshatra().Name),
		ab.Sign(),
		asterisk.Degree(ab.SignDegree()),
		ab.House(),
		ab.Mansion())
}

func (book *AsterBook) Build() *AsterBook {
	book.House()
	book.Longitude()
	book.Mansion()
	book.Nakshatra()
	book.Sign()
	book.SignDegree()
	book.Sub()
	return book
}

func (book *AsterBook) Mansion() IslamicMansion {
	if book.mansion == nil {
		v := FindMansion(book.Longitude())
		book.mansion = &v
	}

	return *book.mansion
}

func (book *AsterBook) SignDegree() float64 {
	if book.signDegree != nil {
		return *book.signDegree
	}
	l := book.Longitude()
	sd := asterisk.SignDegree(l)
	book.signDegree = &sd
	return sd
}

func (book *AsterBook) Longitude() float64 {
	if book.lon != nil {
		return *book.lon
	}

	var l float64
	if book.Site.Disposition != nil {
		l = float64(book.Site.Disposition.Position.Lon)
	} else {
		l = *book.Site.Longitude
	}
	book.lon = &l
	return *book.lon
}

func (book *AsterBook) Nakshatra() Nakshatra {
	if book.nak != nil {
		return *book.nak
	}

	if book.nak == nil {
		bufNak, bufSub := FindNakshatra(book.Longitude())
		book.nak = &bufNak
		book.sub = &bufSub
	}
	return *book.nak
}

func (book *AsterBook) Sub() Sub {
	if book.sub != nil {
		return *book.sub
	}
	book.Nakshatra()
	return *book.sub
}

func (book *AsterBook) Sign() asterisk.Sign {
	if book.sign != nil {
		return *book.sign
	}

	sn := asterisk.SignOf(book.Longitude())
	book.sign = &sn
	return *book.sign
}

func (book *AsterBook) InitHouse(ascSign asterisk.Sign) asterisk.House {
	if book.house != nil {
		return *book.house
	}

	hl := asterisk.HouseOf(ascSign, book.Sign())
	book.house = &hl
	return *book.house
}

func (book *AsterBook) House() *asterisk.House {
	return book.house
}

type Peak int32

func (p Peak) String() string {
	switch p {
	case NoPeak:
		return ""
	case AtPeak:
		return "At Peak"
	}

	return ""
}

const (
	NoPeak Peak = iota
	AtPeak
)

func InRange(aspect Aspect, boundry, peakDiff, input float64) (found Aspect, atPeak Peak) {
	found = Unaspected
	atPeak = NoPeak

	boundry = math.Abs(boundry)
	input = math.Abs(input)

	degree := float64(aspect)

	var low = degree - boundry
	var high = degree + boundry
	var dd = math.Abs(input - degree)

	var isin = low <= input && input <= high
	var isdeep = dd <= math.Abs(peakDiff)

	if !isin {
		return
	}

	found = aspect
	if isdeep {
		atPeak = AtPeak
	} else {
		atPeak = NoPeak
	}

	return
}

type Division int32

const (
	D1  Division = 1
	D2  Division = 2
	D3  Division = 3
	D4  Division = 4
	D5  Division = 5
	D6  Division = 6
	D7  Division = 7
	D8  Division = 8
	D9  Division = 9
	D10 Division = 10
	D11 Division = 11
	D12 Division = 12
	D16 Division = 16
	D20 Division = 20
	D24 Division = 24
	D27 Division = 27
	D30 Division = 30
	D40 Division = 40
	D45 Division = 45
	D60 Division = 60
)

func (d *Division) String() string {
	return DivisionStrings[*d]
}

var DivisionStrings = map[Division]string{
	D1:  "Rasi",
	D2:  "Hora",
	D3:  "Drekkana",
	D4:  "Chaturthamsa",
	D5:  "Panchamsa",
	D6:  "Shashthamsa",
	D7:  "Saptamsa",
	D8:  "Ashtamsa",
	D9:  "Navamsa",
	D10: "Dasamsa",
	D11: "Rudramsa",
	D12: "Dwadasamsa",
	D16: "Shodasamsa",
	D20: "Vimsamsa",
	D24: "Chaturvimsamsa",
	D27: "Nakshatramsa",
	D30: "Trimsamsa",
	D40: "Khavedamsa",
	D45: "Akshavedamsa",
	D60: "Shashtyamsa",
}

type Aspect int32

const (
	Asp_60     Aspect = 60
	Asp_90     Aspect = 90
	Asp_120    Aspect = 120
	Asp_180    Aspect = 180
	Cj         Aspect = 0
	Unaspected Aspect = -1
)

var AspectsList = []Aspect{
	Asp_60,
	Asp_90,
	Asp_120,
	Asp_180,
	Cj,
	Unaspected,
}

func (a Aspect) String() string {
	return AspectStrings[a]
}

var AspectStrings = map[Aspect]string{
	Asp_60:     fmt.Sprintf("%d%s", 60, "°"),
	Asp_90:     fmt.Sprintf("%d%s", 90, "°"),
	Asp_120:    fmt.Sprintf("%d%s", 120, "°"),
	Asp_180:    fmt.Sprintf("%d%s", 180, "°"),
	Cj:         fmt.Sprintf("%s", "Cj"),
	Unaspected: fmt.Sprintf("%s", "ø"),
}

type CeLon struct {
	Celestial Celestial
	Lon       float64
}

const (
	peakDiff = 0.001
)

func AspectOf(p1, p2 CeLon) (aspDeg Aspect, aspPeak Peak) {
	aspDeg, aspPeak = Unaspected, NoPeak

	v := asterisk.FitIn(math.Abs(p1.Lon-p2.Lon-360.), 0., 360.)
	if v > 180. {
		v = asterisk.FitIn(math.Abs(v-360.), 0., 360.)
	}

	diff := v

	if asp, p := InRange(60., 6., peakDiff, diff); asp != Unaspected {
		aspDeg, aspPeak = asp, p
	} else if asp, p := InRange(90., 6., peakDiff, diff); asp != Unaspected {
		aspDeg, aspPeak = Asp_90, p
	} else if asp, p := InRange(120., 6., peakDiff, diff); asp != Unaspected {
		aspDeg, aspPeak = Asp_120, p
	} else if asp, p := InRange(180., 8., peakDiff, diff); asp != Unaspected {
		cond1 := (*p1.Celestial.Planet == asterisk.Ra) && (*p2.Celestial.Planet == asterisk.Ke)
		cond2 := (*p1.Celestial.Planet == asterisk.Ke) && (*p2.Celestial.Planet == asterisk.Ra)
		if cond1 || cond2 {
			aspDeg, aspPeak = Unaspected, p
		} else {
			aspDeg, aspPeak = Asp_180, p
		}
	}

	if aspDeg == Unaspected {
		if p1.Celestial.Angle != nil || p2.Celestial.Angle != nil {
			//NOP
		} else if diff < (Diptamsa[asterisk.Planet(*p1.Celestial.Planet)] + Diptamsa[asterisk.Planet(*p2.Celestial.Planet)]) {
			aspDeg = Cj
			if diff <= peakDiff {
				aspPeak = AtPeak
			}
		}
	}

	return
}

type Aster struct {
	Celestial Celestial
	AsterBook AsterBook
}

type ChartSet struct {
	Asc Aster
	Su  Aster
	Mo  Aster
	Me  Aster
	Ve  Aster
	Ma  Aster
	Ju  Aster
	Sa  Aster
	Ur  Aster
	Ne  Aster
	Pl  Aster
	Ra  Aster
	Ke  Aster

	Position asterisk.Position
	Time     time.Time
}

func (ch *ChartSet) For(utc time.Time, pos asterisk.Position) *ChartSet {
	cset := ch
	jd := asterisk.JulianDayOf(utc)

	fa := observatory.ForAscendant{
		Position:  pos,
		JulianDay: jd,
		Asc:       observatory.Angle(observatory.Asc),
		Answer:    make(chan float64, 1)}
	AscendantChan <- fa
	ascd := <-fa.Answer
	ascBuff := Asc
	cel := Celestial{Angle: &ascBuff}
	a := Aster{cel, AsterBook{Celestial: cel, Site: Site{Longitude: &ascd}}}
	a.AsterBook.InitHouse(a.AsterBook.Sign())
	cset.Asc = a

	for _, pltx := range asterisk.PlanetList {
		plt := pltx
		fp := ForPlanet{Planet: plt, JulianDay: jd, Answer: make(chan asterisk.Point, 1)}
		PlanetChan <- fp
		pnt := <-fp.Answer
		cel := Celestial{Planet: &plt}

		a := &Aster{cel, AsterBook{Celestial: cel, Site: Site{Disposition: &Disposition{Position: pnt.Position, Flow: pnt.Flow}}}}
		a.AsterBook.InitHouse(cset.Asc.AsterBook.Sign())

		cset.SetPlanet(plt, a)
	}

	for _, b := range cset.List() {
		b.AsterBook.Build()
	}

	ch.Position = pos
	ch.Time = utc

	return ch
}

func (ch *ChartSet) SetPlanet(plt asterisk.Planet, a *Aster) {
	switch plt {
	case asterisk.Su:
		ch.Su = *a
	case asterisk.Mo:
		ch.Mo = *a
	case asterisk.Me:
		ch.Me = *a
	case asterisk.Ve:
		ch.Ve = *a
	case asterisk.Ma:
		ch.Ma = *a
	case asterisk.Ju:
		ch.Ju = *a
	case asterisk.Sa:
		ch.Sa = *a
	case asterisk.Ur:
		ch.Ur = *a
	case asterisk.Ne:
		ch.Ne = *a
	case asterisk.Pl:
		ch.Pl = *a
	case asterisk.Ra:
		ch.Ra = *a
	case asterisk.Ke:
		ch.Ke = *a
	}
}

func (ch *ChartSet) List() []Aster {
	return []Aster{
		ch.Asc,
		ch.Su,
		ch.Mo,
		ch.Me,
		ch.Ve,
		ch.Ma,
		ch.Ju,
		ch.Sa,
		ch.Ur,
		ch.Ne,
		ch.Pl,
		ch.Ra,
		ch.Ke,
	}
}

func (ch *ChartSet) Planets() []Aster {
	return []Aster{
		ch.Su,
		ch.Mo,
		ch.Me,
		ch.Ve,
		ch.Ma,
		ch.Ju,
		ch.Sa,
		ch.Ur,
		ch.Ne,
		ch.Pl,
		ch.Ra,
		ch.Ke,
	}
}

type IslamicMansion struct {
	Name string
	Arc  Arc
}

func (m IslamicMansion) String() string {
	return fmt.Sprintf("{Name: %v, Arc: %v}", m.Name, m.Arc)
}

var mansionLen = 360.0 / 28.0

func initMansions() []IslamicMansion {
	var iss []IslamicMansion

	currentMansion := 1

	iss = append(iss, IslamicMansion{Name: "شَرَطَین - نَطح", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "بُطَین", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "ثُرَیّا", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "دَبَران", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "هَقعَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "هَنعَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "ذِراع", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "نَثرَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "طَرف", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "جَبهَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "زُبرَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "صَرفَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "عَوّاء", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "سِمّاک الاعزَل", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "غَفر", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "زُبانا", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "اِکلیل", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "قَلب", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "شولَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "نَعائم", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "بَلدَه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "سَعدُ الذابِح", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "سَعدُ بُلَع", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "سَعدُ سُعود", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "سَعدُالاَخبیَّه", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "فَرغُ الاَوَّل", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "فَرغُ الثانی", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++
	iss = append(iss, IslamicMansion{Name: "رِشاء", Arc: Arc{Start: float64(currentMansion-1) * mansionLen, End: float64(currentMansion) * mansionLen}})
	currentMansion++

	return iss
}

var MansionList = initMansions()

func FindMansion(lon float64) (iss IslamicMansion) {
	//Seq.tryFind (fun x -> x.Arc.Start <= lon && lon < x.Arc.End) (IslamicMansion.List)
	for _, v := range MansionList {
		v := v
		if v.Arc.Start <= lon && lon < v.Arc.End {
			iss = v
			break
		}
	}

	return
}
