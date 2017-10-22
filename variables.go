package asterisk

import "github.com/dc0d/gosweph"

//-----------------------------------------------------------------------------

// Time fractures
const (
	HourNanosecondFactor   = 3600000000000
	MinuteNanosecondFactor = HourNanosecondFactor / 60
	NanosecondFactor       = MinuteNanosecondFactor / 60
)

//-----------------------------------------------------------------------------

// Config (global)
// TODO: remove global variables
var (
	SiderealMode     = gosweph.SE_SIDM_LAHIRI
	PositionType     = gosweph.SEFLG_SIDEREAL
	IFlag            = int32(gosweph.SEFLG_SPEED) | int32(gosweph.SEFLG_SWIEPH) | int32(PositionType)
	HouseSystem      = gosweph.EX_HOUSE_SYS_K
	NodeType         = gosweph.SE_TRUE_NODE
	Asc              = gosweph.SE_ASC
	EphemerisPath   = "sweph-files"
)

//-----------------------------------------------------------------------------
