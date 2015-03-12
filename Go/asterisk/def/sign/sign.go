package sign

import "math"
import . "asterisk/def"

type Sign int32

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

func ParseSign(str string) Sign {
	return SignParsed[str]
}

func SignOf(lon float64) Sign {
	rem := Mod(lon, 30.)
	return Sign(math.Floor((lon-rem)/30.0) + 1.0)
}

func SignDegree(longitude float64) float64 {
	return Mod(longitude, 30.)
}

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
