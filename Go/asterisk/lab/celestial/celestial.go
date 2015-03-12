package celestial

import (
	"asterisk/astrobserve"
	"asterisk/def/direction"
	"asterisk/def/planet"
	"asterisk/def/sign"
	"fmt"
	"gopkg.in/mgo.v2/bson"
	"strings"
	pers "studio/persian/time"
	"time"
)

type Moment struct {
	Id           bson.ObjectId `bson:"_id,omitempty"`
	PersianMonth pers.PersianMonth

	Chart       astrobserve.ChartSet
	Aspected    []Aspected
	Signed      []Signed
	Mansioned   []Mansioned
	Directioned []Directioned
}

type Flag int32

func (f Flag) String() string {
	switch f {
	case None:
		return "None"
	case Start:
		return "Start"
	case Ongoing:
		return "Ongoing"
	case End:
		return "End"
	}
	return ""
}

const (
	None Flag = iota + 1
	Start
	Ongoing
	End
)

type Change struct {
	At   time.Time
	Flag Flag
}

// multi planet change event
type Multi struct {
	Change
	Participants []planet.Planet
}

func (m Multi) String() string {
	s := "{"
	s += fmt.Sprintf("%+v ", m.Change)
	s += "[ "

	parts := []string{}
	for _, p := range m.Participants {
		parts = append(parts, fmt.Sprintf("%v", p))
	}
	s += strings.Join(parts, ", ")

	s += " ]}"
	return s
}

// single planet change event
type Single struct {
	Change
	Participant planet.Planet
}

func (m Single) String() string {
	s := "{"
	s += fmt.Sprintf("%+v ", m.Change)
	s += "[ "

	s += fmt.Sprintf("%v", m.Participant)

	s += " ]}"
	return s
}

type Aspected struct {
	Multi
	Aspect astrobserve.Aspect
	AtPeak astrobserve.Peak
}

func (asp *Aspected) CompareAspect(v *Aspected) bool {
	if v == nil {
		return false
	}
	if len(v.Participants) != len(asp.Participants) {
		return false
	}
	if asp.Aspect != v.Aspect {
		return false
	}

	set := map[planet.Planet]int32{}

	for _, np := range asp.Participants {
		set[np] = 1
	}
	for _, op := range v.Participants {
		_, exists := set[op]
		if !exists {
			return false
		}
	}

	return true
}

type Signed struct {
	Single
	Sign sign.Sign
}

type Mansioned struct {
	Single
	Mansion astrobserve.IslamicMansion
}

type Directioned struct {
	Single
	Direction direction.Direction
}
