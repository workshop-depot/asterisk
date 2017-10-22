package direction

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
