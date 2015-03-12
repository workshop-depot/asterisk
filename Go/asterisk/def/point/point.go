package point

import (
	. "asterisk/def/flow"
	. "asterisk/def/planet"
	. "asterisk/def/position"
	"fmt"
)

type Point struct {
	Id       Planet
	Position Position
	Flow     Flow
}

func (p *Point) String() string {
	return fmt.Sprintf("%v %v %v", p.Id, &p.Position, &p.Flow)
}
