module CelestialEventsView

open System
open System.IO
open System.Linq
open System.Globalization
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Studio
open Factory
open Calculator
open Computer
open CelestialEvents

let ChainOfEvents (pos: Position) (startDate: DateTime) (endDate: DateTime) (step: float<s>) =
    let startDay: JulianDay = JulianDay.Of (1<YY> * startDate.Year) (1<MM> * startDate.Month) (1<dd> * startDate.Day) 0.<hh>
    let endDay: JulianDay = JulianDay.Of (1<YY> * endDate.Year) (1<MM> * endDate.Month) (1<dd> * endDate.Day) 0.<hh>
    
    let jd = ref <| startDay
    let charts =
        seq {
            while endDay >= !jd do
                let chart = For !jd pos

                yield chart, !jd

                jd := (!jd).ShiftSeconds step
        }

    let lastChart: ChartSet<Aster> option ref = ref None

    seq {
        for chx in charts do
            let chart, jd = chx
            let gd = CalendarDay.Of jd

            let chall = chart.All
            let cross =
                    seq {
                        for i in [ 0 .. (chall.Length - 2) ] do
                            for j in [ (i + 1) .. (chall.Length - 1) ] do
                                yield (chall.[i], chall.[j])
                    }
            for T (id1, book1), T (id2, book2) in cross do
                match id1, id2 with
                | Body p1, Body p2 -> 
                    let aspVal = Aspect.Of (id1, book1.Longitude) (id2, book2.Longitude)
                    match aspVal with
                    | Some (asp, peak) -> 
                        yield { Event = Aspected;
                                Jd = jd;
                                Planets = p1 :: p2 :: []
                                Signs = (book1.Sign) :: (book2.Sign) :: [];
                                Directions = [];
                                Aspects = asp :: [];
                                Mansion = None;
                                AtPeak = Some peak }
                        ()
                    | _ -> ()
                    ()
                | _ -> ()
                ()

            match !lastChart with
            | Some(ch) ->
                for T (id1, book1), T (id2, book2) in Seq.zip ch.All chart.All do
                    match book1.Locus, book2.Locus with
                    | Disposition (p1, f1), Disposition (p2, f2) -> 
                        let d1 = IsDirect f1.SpeedInLongitude
                        let d2 = IsDirect f2.SpeedInLongitude
                        if d1 <> d2 then
                            match id2 with
                            | Body p ->
                                yield { Event = DirectionChanged;
                                        Jd = jd;
                                        Planets = p :: [];
                                        Signs = [];
                                        Directions = (Direction.Of d1) :: (Direction.Of d2) :: [];
                                        Aspects = [];
                                        Mansion = None;
                                        AtPeak = None }
                            | _ -> ()

                        let s1 = book1.Sign
                        let s2 = book2.Sign
                        if s1 <> s2 then
                            match id2 with
                            | Body p ->
                                yield { Event = InSign;
                                        Jd = jd;
                                        Planets = p :: [];
                                        Signs = s1 :: s2 :: [];
                                        Directions = [];
                                        Aspects = [];
                                        Mansion = None;
                                        AtPeak = None }
                            | _ -> ()

                        match id2 with
                        | Body p ->
                            yield { Event = InMansion;
                                    Jd = jd;
                                    Planets = p :: [];
                                    Signs = s2 :: [];
                                    Directions = [];
                                    Aspects = [];
                                    Mansion = IslamicMansion.FindMansion(book2.Longitude);
                                    AtPeak = None }
                        | _ -> ()
                    | _ -> ()
                    ()
                ()
            | _ -> ()

            lastChart := chart |> Some
    }
