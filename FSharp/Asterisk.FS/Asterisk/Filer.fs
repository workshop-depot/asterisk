module Filer

open System
open System.Linq
open System.IO
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Studio
open CelestialEventsView
open Factory
open Astrobserve
open CelestialEvents

let Source (pos: Position) (startDate: DateTime) (endDate: DateTime) (step: float<s>) = 
    let log = NLog.LogManager.GetCurrentClassLogger()
    
    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "sd.csv"

    let fp = Path.Combine(__dk, __fn)

    use writer = new StreamWriter(fp, false)

    writer.WriteLine(Moment.CsvHeader())
    for cev in (ChainOfEvents pos startDate endDate step) do //.Take(1000)
        writer.WriteLine(cev)

    writer.Flush()
