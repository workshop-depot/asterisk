package observatory

import (
	"github.com/dc0d/asterisk"
	"github.com/dc0d/gosweph"
)

//-----------------------------------------------------------------------------
// Queries

// ForPlanet .
type ForPlanet struct {
	Planet    asterisk.Planet
	JulianDay asterisk.JulianDay
	Answer    chan asterisk.Point
}

// ForAscendant .
type ForAscendant struct {
	Position  asterisk.Position
	JulianDay asterisk.JulianDay
	Asc       Angle
	Answer    chan float64
}

// ForTransit .
type ForTransit struct {
	Planet      asterisk.Planet
	Position    asterisk.Position
	JulianDay   asterisk.JulianDay
	TransitFlag asterisk.TransitFlag
	Answer      chan asterisk.JulianDay
}

//-----------------------------------------------------------------------------

// Angle like different kinds of asc - needs polishing
type Angle int32

func (a *Angle) String() (s string) {
	switch *a {
	case Asc:
		s = "Asc"
	case Mc:
		s = "Mc"
	case Armc:
		s = "Armc"
	case Vertex:
		s = "Vertex"
	case Equasc:
		s = "Equasc"
	case Coasc1:
		s = "Coasc1"
	case Coasc2:
		s = "Coasc2"
	case Polasc:
		s = "Polasc"
	case Nascmc:
		s = "Nascmc"
	}
	return
}

// Angle values
const (
	Asc    Angle = Angle(gosweph.SE_ASC)
	Mc     Angle = Angle(gosweph.SE_MC)
	Armc   Angle = Angle(gosweph.SE_ARMC)
	Vertex Angle = Angle(gosweph.SE_VERTEX)
	Equasc Angle = Angle(gosweph.SE_EQUASC)
	Coasc1 Angle = Angle(gosweph.SE_COASC1)
	Coasc2 Angle = Angle(gosweph.SE_COASC2)
	Polasc Angle = Angle(gosweph.SE_POLASC)
	Nascmc Angle = Angle(gosweph.SE_NASCMC)
)

//-----------------------------------------------------------------------------
