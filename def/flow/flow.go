package flow

import (
	. "asterisk/def"
	"fmt"
)

type Flow struct {
	Distance, SpeedInLongitude, SpeedInLatitude, SpeedInDistance float64
}

func (x Flow) String() string {
	return fmt.Sprintf("{dist: %f, in-lon: %s, in-lat: %s, in-dist: %s}", x.Distance, DegreeString(DurationFromHour(x.SpeedInLongitude)), DegreeString(DurationFromHour(x.SpeedInLatitude)), DegreeString(DurationFromHour(x.SpeedInDistance)))
}
