// definition & representation of planets
package planet

import (
	"asterisk/config"
	"gosweph"
)

type Planet int32

var (
	Su Planet = Planet(1)
	Mo Planet = Planet(2)
	Me Planet = Planet(3)
	Ve Planet = Planet(4)
	Ma Planet = Planet(5)
	Ju Planet = Planet(6)
	Sa Planet = Planet(7)
	Ur Planet = Planet(8)
	Ne Planet = Planet(9)
	Pl Planet = Planet(10)
	Ra Planet = Planet(11)
	Ke Planet = Planet(12)
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
