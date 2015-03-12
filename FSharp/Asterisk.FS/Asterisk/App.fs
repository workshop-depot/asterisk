module App

open System
open System.Linq
open System.Diagnostics
open System.IO
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Studio
open CelestialEventsView
open Factory
open Astrobserve
open Filer

let dataLog = NLog.LogManager.GetLogger("Data")
let appEventLog = NLog.LogManager.GetLogger("Event")

do
    dataLog.Info("")
    let ephePath = "sweph-files"
    Open (Some ephePath)
    BasicSetup (Config.Default()) |> ignore
    ()

let initTrigger = (do ()); true

[<EntryPoint>]
let main argv = 
    let log = NLog.LogManager.GetCurrentClassLogger()
    log.Info("start")
    
    let pos = { Longitude = 1.<deg> * (Dg (51.<deg>, 17.<mm>, 0.<s>)).Float(); Latitude = 1.<deg> * (Dg (35.<deg>, 44.<mm>, 0.<s>)).Float() }

    try
        let sw = Stopwatch()

        sw.Start()
        Source pos (DateTime(2015, 2, 20)) (DateTime(2015, 3, 21)) 10.<s>
        sw.Stop()

        log.Info("total time: {0}", sw.Elapsed)
    finally
        Close()
        log.Info("end")

    0 // return an integer exit code
