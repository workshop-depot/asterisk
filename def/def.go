// common definitions used in astro calculations
package def

import (
	"gosweph"
	"math"
	"strings"
	"studio"
	"time"
)

const (
	HourNanosecondFactor   = 3600000000000
	MinuteNanosecondFactor = HourNanosecondFactor / 60
	NanosecondFactor       = MinuteNanosecondFactor / 60
)

// % operator for float64
func Mod(f float64, n float64) float64 {
	lf := math.Abs(f)
	for lf > n {
		lf = lf - n
	}
	if f < 0 {
		lf = -1. * lf
	}
	return lf
}

func DurationFromHour(hour float64) time.Duration {
	return time.Duration(int64(hour*HourNanosecondFactor)) * time.Nanosecond
}

func DurationFromMinute(minute float64) time.Duration {
	return time.Duration(int64(minute*MinuteNanosecondFactor)) * time.Nanosecond
}

func DurationFromSecond(second float64) time.Duration {
	return time.Duration(int64(second*NanosecondFactor)) * time.Nanosecond
}

func DegreeString(d time.Duration) string {
	sec := d.Seconds()
	sec = studio.Round(sec, .5, 0)
	d = time.Duration(sec * 1000000000)

	su := d.String()
	s := strings.Replace(su, "h", "°", 1)
	s = strings.Replace(s, "m", "'", 1)
	s = strings.Replace(s, "s", "\"", 1)
	return s
}

func FitIn(x float64, inclusiveStart float64, exclusiveEnd float64) float64 {
	d := x
	dx := exclusiveEnd - inclusiveStart
	for d >= exclusiveEnd {
		d = d - dx
	}
	for d < inclusiveStart {
		d = d + dx
	}
	return d
}

func PositiveMod(l float64, factor float64) float64 {
	var n float64 = l
	for n < 0.0 {
		n = n + factor
	}
	for n >= factor {
		n = n - factor
	}
	return n
}

type TransitFlag int32

const (
	Rise           TransitFlag = TransitFlag(gosweph.SE_CALC_RISE)
	Set            TransitFlag = TransitFlag(gosweph.SE_CALC_SET)
	MTransit       TransitFlag = TransitFlag(gosweph.SE_CALC_MTRANSIT)
	ITransit       TransitFlag = TransitFlag(gosweph.SE_CALC_ITRANSIT)
	DiscCenter     TransitFlag = TransitFlag(gosweph.SE_BIT_DISC_CENTER)
	DiscBottom     TransitFlag = TransitFlag(gosweph.SE_BIT_DISC_BOTTOM)
	NoRefraction   TransitFlag = TransitFlag(gosweph.SE_BIT_NO_REFRACTION)
	CivilTwilight  TransitFlag = TransitFlag(gosweph.SE_BIT_CIVIL_TWILIGHT)
	NauticTwilight TransitFlag = TransitFlag(gosweph.SE_BIT_NAUTIC_TWILIGHT)
	AstroTwilight  TransitFlag = TransitFlag(gosweph.SE_BIT_ASTRO_TWILIGHT)
	FixedDiscSize  TransitFlag = TransitFlag(gosweph.SE_BIT_FIXED_DISC_SIZE)
)

type Degree float64

func (d Degree) String() string {
	return DegreeString(DurationFromHour(float64(d)))
}
