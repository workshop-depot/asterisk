package astrobserve

import (
	"asterisk/config"
	. "asterisk/def"
	. "asterisk/def/flow"
	. "asterisk/def/julian"
	. "asterisk/def/planet"
	. "asterisk/def/point"
	. "asterisk/def/position"
	"gosweph"
	"path/filepath"
	"studio"
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
	Planet    Planet
	JulianDay JulianDay
	Answer    chan Point
}

type ForAscendant struct {
	Position  Position
	JulianDay JulianDay
	Asc       Angle
	Answer    chan float64
}

type ForTransit struct {
	Planet      Planet
	Position    Position
	JulianDay   JulianDay
	TransitFlag TransitFlag
	Answer      chan JulianDay
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
	studio.SafeCall(func() { gosweph.Swe_close() })
}

func setup() {
	dir, err := filepath.Abs(filepath.Dir(config.EphemerisPath))
	if err != nil {
		panic(err)
	}

	dir = filepath.Join(dir, config.EphemerisPath)

	gosweph.Swe_set_sid_mode(int32(config.SiderealMode), 0., 0.)
	gosweph.Swe_set_ephe_path(&dir)
}

func observeTransits(pln Planet, position Position, time JulianDay, transitFlags int32) JulianDay {
	jd := float64(time)
	tjd_ut := jd
	epheflag := config.IFlag
	rsmi := transitFlags
	geopos := [3]float64{position.Lon, position.Lat, 0.}
	atpress := 0.
	attemp := 0.
	tret := 0.
	var serr string

	gosweph.Swe_rise_trans(tjd_ut, pln.Swe(), nil, epheflag, rsmi, &geopos, atpress, attemp, &tret, &serr)

	return JulianDay(tret)
}

func observeAscendant(position Position, time JulianDay, asc Angle) float64 {
	_, ascmc := observeHouses(position, time)
	return ascmc[asc]
}

func observeHouses(position Position, time JulianDay) (cusps [13]float64, ascmc [10]float64) {
	jd := float64(time)
	_cusps := cusps
	_ascmc := ascmc
	gosweph.Swe_houses_ex(jd, config.IFlag, position.Lat, position.Lon, int32(config.HouseSystem), &_cusps, &_ascmc)

	cusps = _cusps
	ascmc = _ascmc
	return
}

func observePoint(pln Planet, time JulianDay) Point {
	var xx [6]float64
	var serr string
	jd := float64(time)

	iflag := config.IFlag
	gosweph.Swe_calc_ut(jd, pln.Swe(), iflag, &xx, &serr)

	var pointPosition Position
	if pln == Ke {
		lon := PositiveMod(xx[0]+180., 360.)
		lat := xx[1] * -1.
		pointPosition = Position{Lon: lon, Lat: lat}
	} else {
		lon := xx[0]
		lat := xx[1]
		pointPosition = Position{Lon: lon, Lat: lat}
	}

	flow := Flow{Distance: xx[2], SpeedInLongitude: xx[3], SpeedInLatitude: xx[4], SpeedInDistance: xx[5]}

	return Point{Id: pln, Position: pointPosition, Flow: flow}
}
