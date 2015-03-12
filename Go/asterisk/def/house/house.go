package house

import (
	. "asterisk/def/sign"
)

type House int32

func (h House) String() string {
	return HouseStrings[h]
}

func ParseHouse(str string) House {
	return HouseParsed[str]
}

func HouseOf(ascSign Sign, pointSign Sign) House {
	n := pointSign - ascSign + 1
	if n <= 0 {
		n = n + 12
	}
	return House(n)
}

const (
	H1  House = 1
	H2  House = 2
	H3  House = 3
	H4  House = 4
	H5  House = 5
	H6  House = 6
	H7  House = 7
	H8  House = 8
	H9  House = 9
	H10 House = 10
	H11 House = 11
	H12 House = 12
)

var HouseStrings = map[House]string{
	H1:  "H1",
	H2:  "H2",
	H3:  "H3",
	H4:  "H4",
	H5:  "H5",
	H6:  "H6",
	H7:  "H7",
	H8:  "H8",
	H9:  "H9",
	H10: "H10",
	H11: "H11",
	H12: "H12",
}

var HouseList = []House{
	H1,
	H2,
	H3,
	H4,
	H5,
	H6,
	H7,
	H8,
	H9,
	H10,
	H11,
	H12,
}

var HouseParsed = map[string]House{
	"H1":  H1,
	"H2":  H2,
	"H3":  H3,
	"H4":  H4,
	"H5":  H5,
	"H6":  H6,
	"H7":  H7,
	"H8":  H8,
	"H9":  H9,
	"H10": H10,
	"H11": H11,
	"H12": H12,
}
