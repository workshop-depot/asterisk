package asterisk

import (
	"fmt"
	"math"
	"time"

	"github.com/dc0d/gosweph"
)

//-----------------------------------------------------------------------------

// TransitFlag .
type TransitFlag int32

// TransitFlag valid values
const (
	Rise           TransitFlag = TransitFlag(gosweph.SE_CALC_RISE)
	Set            TransitFlag = TransitFlag(gosweph.SE_CALC_SET)
	MTransit       TransitFlag = TransitFlag(gosweph.SE_CALC_MTRANSIT)
	ITransit       TransitFlag = TransitFlag(gosweph.SE_CALC_ITRANSIT)
	DiscCenter     TransitFlag = TransitFlag(gosweph.SE_BIT_DISC_CENTER)
	DiscBottom     TransitFlag = TransitFlag(gosweph.SE_BIT_DISC_BOTTOM)
	NoRefraction   TransitFlag = TransitFlag(gosweph.SE_BIT_NO_REFRACTION)
	CivilTwilight  TransitFlag = TransitFlag(gosweph.SE_BIT_CIVIL_TWILIGHT)
	NauticTwilight TransitFlag = TransitFlag(gosweph.SE_BIT_NAUTIC_TWILIGHT)
	AstroTwilight  TransitFlag = TransitFlag(gosweph.SE_BIT_ASTRO_TWILIGHT)
	FixedDiscSize  TransitFlag = TransitFlag(gosweph.SE_BIT_FIXED_DISC_SIZE)
)

//-----------------------------------------------------------------------------

// Degree .
type Degree float64

func (d Degree) String() string {
	return DegreeString(DurationFromHour(float64(d)))
}

//-----------------------------------------------------------------------------
// Sign

// Sign .
type Sign int32

// Sign valid values
const (
	Aries       Sign = 1
	Taurus      Sign = 2
	Gemini      Sign = 3
	Cancer      Sign = 4
	Leo         Sign = 5
	Virgo       Sign = 6
	Libra       Sign = 7
	Scorpio     Sign = 8
	Sagittarius Sign = 9
	Capricorn   Sign = 10
	Aquarius    Sign = 11
	Pisces      Sign = 12
)

func (s Sign) String() string {
	return SignStrings[s]
}

// ParseSign .
func ParseSign(str string) Sign {
	return SignParsed[str]
}

// SignOf .
func SignOf(lon float64) Sign {
	rem := Mod(lon, 30.)
	return Sign(math.Floor((lon-rem)/30.0) + 1.0)
}

// SignDegree .
func SignDegree(longitude float64) float64 {
	return Mod(longitude, 30.)
}

// SignStrings helps with Sign implementing Stringer
var SignStrings = map[Sign]string{
	Aries:       "Aries",
	Taurus:      "Taurus",
	Gemini:      "Gemini",
	Cancer:      "Cancer",
	Leo:         "Leo",
	Virgo:       "Virgo",
	Libra:       "Libra",
	Scorpio:     "Scorpio",
	Sagittarius: "Sagittarius",
	Capricorn:   "Capricorn",
	Aquarius:    "Aquarius",
	Pisces:      "Pisces",
}

// SignList .
var SignList = []Sign{
	Aries,
	Taurus,
	Gemini,
	Cancer,
	Leo,
	Virgo,
	Libra,
	Scorpio,
	Sagittarius,
	Capricorn,
	Aquarius,
	Pisces,
}

// SignParsed .
var SignParsed = map[string]Sign{
	"Aries":       Aries,
	"Taurus":      Taurus,
	"Gemini":      Gemini,
	"Cancer":      Cancer,
	"Leo":         Leo,
	"Virgo":       Virgo,
	"Libra":       Libra,
	"Scorpio":     Scorpio,
	"Sagittarius": Sagittarius,
	"Capricorn":   Capricorn,
	"Aquarius":    Aquarius,
	"Pisces":      Pisces,
}

//-----------------------------------------------------------------------------
// Position

// Position .
type Position struct {
	Lon, Lat float64
}

func (x Position) String() string {
	return fmt.Sprintf("{lon: %s, lat: %s}", DegreeString(DurationFromHour(x.Lon)), DegreeString(DurationFromHour(x.Lat)))
}

//-----------------------------------------------------------------------------
// Planet

// Planet .
type Planet int32

// Planet valid values
var (
	Su = Planet(1)
	Mo = Planet(2)
	Me = Planet(3)
	Ve = Planet(4)
	Ma = Planet(5)
	Ju = Planet(6)
	Sa = Planet(7)
	Ur = Planet(8)
	Ne = Planet(9)
	Pl = Planet(10)
	Ra = Planet(11)
	Ke = Planet(12)
)

func (s Planet) String() string {
	return PlanetStrings[s]
}

// Swe .
func (s Planet) Swe() int32 {
	switch s {
	case Su:
		return gosweph.SE_SUN
	case Mo:
		return gosweph.SE_MOON
	case Me:
		return gosweph.SE_MERCURY
	case Ve:
		return gosweph.SE_VENUS
	case Ma:
		return gosweph.SE_MARS
	case Ju:
		return gosweph.SE_JUPITER
	case Sa:
		return gosweph.SE_SATURN
	case Ur:
		return gosweph.SE_URANUS
	case Ne:
		return gosweph.SE_NEPTUNE
	case Pl:
		return gosweph.SE_PLUTO
	case Ra:
		return NodeType
	case Ke:
		return NodeType
	}

	panic("not a valid planet")
}

// ParsePlanet .
func ParsePlanet(str string) Planet {
	return PlanetParsed[str]
}

// PlanetStrings .
var PlanetStrings = map[Planet]string{
	Su: "Su",
	Mo: "Mo",
	Me: "Me",
	Ve: "Ve",
	Ma: "Ma",
	Ju: "Ju",
	Sa: "Sa",
	Ur: "Ur",
	Ne: "Ne",
	Pl: "Pl",
	Ra: "Ra",
	Ke: "Ke",
}

// PlanetList .
var PlanetList = []Planet{
	Su,
	Mo,
	Me,
	Ve,
	Ma,
	Ju,
	Sa,
	Ur,
	Ne,
	Pl,
	Ra,
	Ke,
}

// PlanetParsed .
var PlanetParsed = map[string]Planet{
	"Su": Su,
	"Mo": Mo,
	"Me": Me,
	"Ve": Ve,
	"Ma": Ma,
	"Ju": Ju,
	"Sa": Sa,
	"Ur": Ur,
	"Ne": Ne,
	"Pl": Pl,
	"Ra": Ra,
	"Ke": Ke,
}

// Spheres .
var Spheres = []Planet{
	Mo,
	Me,
	Ve,
	Su,
	Ma,
	Ju,
	Sa,
}

//-----------------------------------------------------------------------------
// Flow

// Flow .
type Flow struct {
	Distance, SpeedInLongitude, SpeedInLatitude, SpeedInDistance float64
}

func (x Flow) String() string {
	return fmt.Sprintf("{dist: %f, in-lon: %s, in-lat: %s, in-dist: %s}", x.Distance, DegreeString(DurationFromHour(x.SpeedInLongitude)), DegreeString(DurationFromHour(x.SpeedInLatitude)), DegreeString(DurationFromHour(x.SpeedInDistance)))
}

//-----------------------------------------------------------------------------
// Point

// Point .
type Point struct {
	ID       Planet
	Position Position
	Flow     Flow
}

func (p *Point) String() string {
	return fmt.Sprintf("%v %v %v", p.ID, &p.Position, &p.Flow)
}

//-----------------------------------------------------------------------------
// Direction

// Direction .
type Direction bool

func (d Direction) String() string {
	switch d {
	case true:
		return "Direct"
	case false:
		return "Reverse"
	}

	return ""
}

//-----------------------------------------------------------------------------
// House

// House .
type House int32

func (h House) String() string {
	return HouseStrings[h]
}

// ParseHouse .
func ParseHouse(str string) House {
	return HouseParsed[str]
}

// HouseOf .
func HouseOf(ascSign Sign, pointSign Sign) House {
	n := pointSign - ascSign + 1
	if n <= 0 {
		n = n + 12
	}
	return House(n)
}

// House valid values
const (
	H1  House = 1
	H2  House = 2
	H3  House = 3
	H4  House = 4
	H5  House = 5
	H6  House = 6
	H7  House = 7
	H8  House = 8
	H9  House = 9
	H10 House = 10
	H11 House = 11
	H12 House = 12
)

// HouseStrings .
var HouseStrings = map[House]string{
	H1:  "H1",
	H2:  "H2",
	H3:  "H3",
	H4:  "H4",
	H5:  "H5",
	H6:  "H6",
	H7:  "H7",
	H8:  "H8",
	H9:  "H9",
	H10: "H10",
	H11: "H11",
	H12: "H12",
}

// HouseList .
var HouseList = []House{
	H1,
	H2,
	H3,
	H4,
	H5,
	H6,
	H7,
	H8,
	H9,
	H10,
	H11,
	H12,
}

// HouseParsed .
var HouseParsed = map[string]House{
	"H1":  H1,
	"H2":  H2,
	"H3":  H3,
	"H4":  H4,
	"H5":  H5,
	"H6":  H6,
	"H7":  H7,
	"H8":  H8,
	"H9":  H9,
	"H10": H10,
	"H11": H11,
	"H12": H12,
}

//-----------------------------------------------------------------------------
// JulianDay

// JulianDay .
type JulianDay float64

// JulianDayOf input should be universal time
func JulianDayOf(t time.Time) JulianDay {
	year, month, day := int32(t.Year()), int32(t.Month()), int32(t.Day())
	hour := float64(t.Hour()) + float64(t.Minute())/60. + float64(t.Second())/3600. + float64(t.Nanosecond())/(1000.*1000.*1000.)/3600.
	hour = math.Floor(hour*10000000.) / 10000000.
	flag := gosweph.SE_GREG_CAL
	if (year*10000 + month*100 + day) < 15821015 {
		flag = gosweph.SE_JUL_CAL
	}
	return JulianDay(gosweph.Swe_julday(year, month, day, hour, flag))
}

// Shift .
func (x JulianDay) Shift(dt time.Duration) JulianDay {
	days := dt.Hours() / 24.
	return x + JulianDay(days)
}

// Time .
func (x JulianDay) Time() time.Time {
	var iyear, imonth, iday, ihour, imin int32
	var dsec float64

	gosweph.Swe_jdut1_to_utc(float64(x), gosweph.SE_GREG_CAL, &iyear, &imonth, &iday, &ihour, &imin, &dsec)

	sec := int(math.Floor(dsec))
	nsec := int((dsec - float64(sec)) * NanosecondFactor)
	return time.Date(int(iyear), time.Month(imonth), int(iday), int(ihour), int(imin), sec, nsec, time.UTC)
}

//-----------------------------------------------------------------------------
