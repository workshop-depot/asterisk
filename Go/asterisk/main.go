// Julian Day -> JulianDay
// Calendar Day -> time.Time
// Degree -> time.Duration
package main

import (
	"asterisk/lab"
	"fmt"
	"runtime"
	"studio"
	"time"
)

func main() {
	fmt.Println("start...")
	defer studio.TimeTrack(time.Now(), "TestDb")
	//lab.TestDb1()
	//lab.TestReport1()
	lab.TestReport2()
}

func init() {
	runtime.GOMAXPROCS(runtime.NumCPU() * 2)
}
