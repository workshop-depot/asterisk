package asterisk

import (
	"math"
	"strings"
	"time"
)

//-----------------------------------------------------------------------------

// Round .
func Round(val float64, roundOn float64, places int) (newVal float64) {
	var round float64
	pow := math.Pow(10, float64(places))
	digit := pow * val
	_, div := math.Modf(digit)
	if div >= roundOn {
		round = math.Ceil(digit)
	} else {
		round = math.Floor(digit)
	}
	newVal = round / pow
	return
}

//-----------------------------------------------------------------------------

// Mod % operator for float64
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

//-----------------------------------------------------------------------------

// DurationFromHour .
func DurationFromHour(hour float64) time.Duration {
	return time.Duration(int64(hour*HourNanosecondFactor)) * time.Nanosecond
}

//-----------------------------------------------------------------------------

// DurationFromMinute .
func DurationFromMinute(minute float64) time.Duration {
	return time.Duration(int64(minute*MinuteNanosecondFactor)) * time.Nanosecond
}

//-----------------------------------------------------------------------------

// DurationFromSecond .
func DurationFromSecond(second float64) time.Duration {
	return time.Duration(int64(second*NanosecondFactor)) * time.Nanosecond
}

//-----------------------------------------------------------------------------

// DegreeString .
func DegreeString(d time.Duration) string {
	sec := d.Seconds()
	sec = Round(sec, .5, 0)
	d = time.Duration(sec * 1000000000)

	su := d.String()
	s := strings.Replace(su, "h", "°", 1)
	s = strings.Replace(s, "m", "'", 1)
	s = strings.Replace(s, "s", "\"", 1)
	return s
}

//-----------------------------------------------------------------------------

// FitIn .
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

//-----------------------------------------------------------------------------

// PositiveMod .
func PositiveMod(l float64, factor float64) float64 {
	var n = l
	for n < 0.0 {
		n = n + factor
	}
	for n >= factor {
		n = n - factor
	}
	return n
}

//-----------------------------------------------------------------------------
