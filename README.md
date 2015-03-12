# asterisk
This code base basically contains a pile of Astrological abstractions and calculations in F# and Go. I've written these in C# and Python too. I've gave up Python because of dreadful ways it provides for FFI (and at last I've used another wrapper library for Swiss Ephemeris). F# is a great language for making abstractions - but it can easily seduce oneself into over-abstracting things. I'm happy with Go so far; but having no GUI tools is let's say measurably saddening.

There are enough abstraction for one to get started with [Swiss Ephemeris](http://www.astro.com/swisseph/) in F# (#fsharp) and Go (#golang). One can calculate a chart, planet hours and some events like start, max and end of angles between planets, in a time period like a month.

For extracting times of angles between planets; in F# a memory resident CSV shaped data source is used, via CSV Type Provider (so it consumes lots of RAM) and Go version is using MongoDB using mgo.

This code base is Alpha quality and need polishing but the calculations - for what it does so far - are precise and correct.

This code base is licensed as MIT but [Swiss Ephemeris](http://www.astro.com/swisseph/) resides under a different license.