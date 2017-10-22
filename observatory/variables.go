package observatory

import (
	"github.com/dc0d/errgo/sentinel"
)

//-----------------------------------------------------------------------------

// Errors
var (
	ErrAgentStopped        = sentinel.Errorf("AGENT STOPPED")
	ErrAgentAlreadyRunning = sentinel.Errorf("AGENT ALREADY RUNNING")
)

//-----------------------------------------------------------------------------

// Query channels
var (
	queryPlanet    = make(chan ForPlanet, 1)
	queryAscendant = make(chan ForAscendant, 1)
	queryTransit   = make(chan ForTransit, 1)
)

//-----------------------------------------------------------------------------

var (
	agentRunning = make(chan struct{}, 1)
)

//-----------------------------------------------------------------------------
