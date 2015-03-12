package julian

import (
	. "asterisk/def"
	"gosweph"
	"math"
	"time"
)

type JulianDay float64

// input should be universal time
func JulianDayOf(t time.Time) JulianDay {
	year, month, day := int32(t.Year()), int32(t.Month()), int32(t.Day())
	hour := float64(t.Hour()) + float64(t.Minute())/60. + float64(t.Second())/3600. + float64(t.Nanosecond())/(1000.*1000.*1000.)/3600.
	hour = math.Floor(hour*10000000.) / 10000000.
	flag := gosweph.SE_GREG_CAL
	if (year*10000 + month*100 + day) < 15821015 {
		flag = gosweph.SE_JUL_CAL
	}
	return JulianDay(gosweph.Swe_julday(year, month, day, hour, flag))
}

func (x JulianDay) Shift(dt time.Duration) JulianDay {
	days := dt.Hours() / 24.
	return x + JulianDay(days)
}

func (x JulianDay) Time() time.Time {
	var iyear, imonth, iday, ihour, imin int32
	var dsec float64

	gosweph.Swe_jdut1_to_utc(float64(x), gosweph.SE_GREG_CAL, &iyear, &imonth, &iday, &ihour, &imin, &dsec)

	sec := int(math.Floor(dsec))
	nsec := int((dsec - float64(sec)) * NanosecondFactor)
	return time.Date(int(iyear), time.Month(imonth), int(iday), int(ihour), int(imin), sec, nsec, time.UTC)
}
