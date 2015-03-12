// no dependencies in asterisk; uses only `gosweph`
package config

import (
	"gosweph"
)

var (
	SiderealMode  int32  = gosweph.SE_SIDM_LAHIRI
	PositionType  int64  = gosweph.SEFLG_SIDEREAL
	IFlag         int32  = int32(gosweph.SEFLG_SPEED) | int32(gosweph.SEFLG_SWIEPH) | int32(PositionType)
	HouseSystem   int32  = gosweph.EX_HOUSE_SYS_K
	NodeType      int32  = gosweph.SE_TRUE_NODE
	Asc           int32  = gosweph.SE_ASC
	EphemerisPath string = "sweph-files"
)
