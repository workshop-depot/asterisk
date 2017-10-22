package astrobserve

import (
	"path/filepath"

	"github.com/dc0d/asterisk"
	"github.com/dc0d/gosweph"
)

func init() {
	go agent()
}

func IsDirect(speedInLongitude float64) bool {
	return speedInLongitude >= 0.
}

func IsNightly(asc, su float64) bool {
	sunLen := su
	ascLen := asc

	if sunLen < ascLen {
		sunLen = sunLen + 360.
	}

	if ascLen < sunLen && sunLen < ascLen+180. {
		return true
	} else {
		return false
	}
}

type ForPlanet struct {
	Planet    asterisk.Planet
	JulianDay asterisk.JulianDay
	Answer    chan asterisk.Point
}

type ForAscendant struct {
	Position  asterisk.Position
	JulianDay asterisk.JulianDay
	Asc       Angle
	Answer    chan float64
}

type ForTransit struct {
	Planet      asterisk.Planet
	Position    asterisk.Position
	JulianDay   asterisk.JulianDay
	TransitFlag asterisk.TransitFlag
	Answer      chan asterisk.JulianDay
}

type ForStop struct {
	Stop   bool
	Answer chan bool
}

var (
	PlanetChan    = make(chan ForPlanet, 1)
	AscendantChan = make(chan ForAscendant, 1)
	TransitChan   = make(chan ForTransit, 1)
	StopChan      = make(chan ForStop, 1)
)

func agent() {
	setup()
	defer dispose()
	for {
		select {
		case p := <-PlanetChan:
			pnt := observePoint(p.Planet, p.JulianDay)
			p.Answer <- pnt
		case a := <-AscendantChan:
			asc := observeAscendant(a.Position, a.JulianDay, a.Asc)
			a.Answer <- asc
		case t := <-TransitChan:
			trn := observeTransits(t.Planet, t.Position, t.JulianDay, int32(t.TransitFlag))
			t.Answer <- trn
		case st := <-StopChan:
			if st.Stop {
				st.Answer <- true
				return
			}
			//case <-time.After(time.Second): //TEMP:
			//	return
		}
	}
}

func dispose() {
	defer func() {
		if e := recover(); e != nil {
			// TODO:
		}
	}()

	gosweph.Swe_close()
}

func setup() {
	dir, err := filepath.Abs(filepath.Dir(asterisk.EphemerisPath))
	if err != nil {
		panic(err)
	}

	dir = filepath.Join(dir, asterisk.EphemerisPath)

	gosweph.Swe_set_sid_mode(int32(asterisk.SiderealMode), 0., 0.)
	gosweph.Swe_set_ephe_path(&dir)
}

func observeTransits(
	pln asterisk.Planet,
	position asterisk.Position,
	time asterisk.JulianDay,
	transitFlags int32) asterisk.JulianDay {
	jd := float64(time)
	tjd_ut := jd
	epheflag := asterisk.IFlag
	rsmi := transitFlags
	geopos := [3]float64{position.Lon, position.Lat, 0.}
	atpress := 0.
	attemp := 0.
	tret := 0.
	var serr string

	gosweph.Swe_rise_trans(tjd_ut, pln.Swe(), nil, epheflag, rsmi, &geopos, atpress, attemp, &tret, &serr)

	return asterisk.JulianDay(tret)
}

func observeAscendant(position asterisk.Position, time asterisk.JulianDay, asc Angle) float64 {
	_, ascmc := observeHouses(position, time)
	return ascmc[asc]
}

func observeHouses(position asterisk.Position, time asterisk.JulianDay) (cusps [13]float64, ascmc [10]float64) {
	jd := float64(time)
	_cusps := cusps
	_ascmc := ascmc
	gosweph.Swe_houses_ex(jd, asterisk.IFlag, position.Lat, position.Lon, int32(asterisk.HouseSystem), &_cusps, &_ascmc)

	cusps = _cusps
	ascmc = _ascmc
	return
}

func observePoint(pln asterisk.Planet, time asterisk.JulianDay) asterisk.Point {
	var xx [6]float64
	var serr string
	jd := float64(time)

	iflag := asterisk.IFlag
	gosweph.Swe_calc_ut(jd, pln.Swe(), iflag, &xx, &serr)

	var pointPosition asterisk.Position
	if pln == asterisk.Ke {
		lon := asterisk.PositiveMod(xx[0]+180., 360.)
		lat := xx[1] * -1.
		pointPosition = asterisk.Position{Lon: lon, Lat: lat}
	} else {
		lon := xx[0]
		lat := xx[1]
		pointPosition = asterisk.Position{Lon: lon, Lat: lat}
	}

	flow := asterisk.Flow{Distance: xx[2], SpeedInLongitude: xx[3], SpeedInLatitude: xx[4], SpeedInDistance: xx[5]}

	return asterisk.Point{ID: pln, Position: pointPosition, Flow: flow}
}
