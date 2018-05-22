// definition & representation of planets
package planet

import (
	"github.com/dc0d/asterisk/Go/asterisk/config"
	"github.com/dc0d/gosweph"
)

type Planet int32

const (
	Su Planet = iota + 1
	Mo
	Me
	Ve
	Ma
	Ju
	Sa
	Ur
	Ne
	Pl
	Ra
	Ke
)

func (s Planet) String() string {
	return PlanetStrings[s]
}

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
		return config.NodeType
	case Ke:
		return config.NodeType
	}

	panic("not a valid planet")
}

func ParsePlanet(str string) Planet {
	return PlanetParsed[str]
}

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

var Spheres = []Planet{
	Mo,
	Me,
	Ve,
	Su,
	Ma,
	Ju,
	Sa,
}
