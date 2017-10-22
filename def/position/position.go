package position

import (
	. "asterisk/def"
	"fmt"
)

type Position struct {
	Lon, Lat float64
}

func (x Position) String() string {
	return fmt.Sprintf("{lon: %s, lat: %s}", DegreeString(DurationFromHour(x.Lon)), DegreeString(DurationFromHour(x.Lat)))
}
