module PlanetaryHours

open System
open System.IO
open System.Linq
open System.Globalization
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Astrobserve
open Factory
open Calculator

let private spheresList = Spheres |> List.ofSeq |> List.rev

let rec LordsOfHours () = 
    seq {
        yield! spheresList 
        yield! LordsOfHours ()
    }

let ComputePlanetaryHours() =
    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "planet-hour.csv"

    let fp = Path.Combine(__dk, __fn)
    use writer = new StreamWriter(fp, false) 

    let ephePath = "sweph-files"

    Open (Some ephePath)
    BasicSetup <| Config.Default() |> ignore

    let pos = { Longitude = 1.<deg> * (Dg(51.<deg>, 17.<mm>, 0.<s>)).Float(); Latitude = 1.<deg> * (Dg(35.<deg>, 44.<mm>, 0.<s>)).Float() } 
    let pcal = new PersianCalendar()
    
    let startDay: CalendarDay = Cd (2014<YY>, 12<MM>, 20<dd>, 0.<hh>)
    let Cd (y, m, d, h) as zx = startDay
    let Jd __jd as zx = JulianDay.Of y m d h
    let jd = ref __jd
    let Jd __jd2 as zxx = JulianDay.Of 2016<YY> 1<MM> 3<dd> 0.<hh>
    let endDay = __jd2

    let previous: ((JulianDay * JulianDay * (float * int * int * int * DateTime)) option ref) = ref None
    while endDay > !jd do
        let riseAt = ObserveTransit Su pos (Jd !jd) [ TransitFlag.Rise; TransitFlag.DiscCenter; TransitFlag.NoRefraction ]
        let setAt = ObserveTransit Su pos (Jd !jd) [ TransitFlag.Set; TransitFlag.DiscCenter; TransitFlag.NoRefraction ]
        let zoneDiff, pY, pM, pD, atDate = TimeZoneDiff <| CalendarDay.Of riseAt

        match !previous with
        | Some (p_riseAt, p_setAt, (p_zoneDiff, p_pY, p_pM, p_pD, p_atDate)) ->
            match (CalendarDay.Of p_riseAt, CalendarDay.Of p_setAt, CalendarDay.Of riseAt) with
            | Cd (RY, RM, RD, RH), Cd (SY, SM, SD, SH), Cd (NY, NM, ND, NH) ->
                let lordOfDay = WeekLords.[Week.FromSys(p_atDate.DayOfWeek)]

                let sunRise: CalendarDay = Cd (RY, RM, RD, RH)
                let sunSet: CalendarDay = Cd (SY, SM, SD, SH)
                let nextSunRise: CalendarDay = Cd (NY, NM, ND, NH)

                let dayHourLen = (((JulianDay.Of SY SM SD SH).ToDay() - (JulianDay.Of RY RM RD RH).ToDay()) |> dd.hh) / 12.
                let nightHourLen = (((JulianDay.Of NY NM ND NH).ToDay() - (JulianDay.Of SY SM SD SH).ToDay()) |> dd.hh) / 12.

                let lords24 = 
                    LordsOfHours().SkipWhile (fun x -> x <> lordOfDay) |> Seq.take 24
                let calSunRise = sunRise

                writer.WriteLine("[{0:yyyy-MM-dd}],[{1:000}-{2:00}-{3:00}],[{4}],[{5}],[{6}]", p_atDate, p_pY, p_pM, p_pD, Degree.Of((RH / 1.<hh> + p_zoneDiff) * 1.<deg>), Degree.Of((SH / 1.<hh> + p_zoneDiff) * 1.<deg>), p_atDate.DayOfWeek)
                for ix in [0 .. 23] do
                    let l = lords24 |> Seq.nth ix
                    if ix < 12 then
                        let Cd(ly, lm, ld, lh) as zx = calSunRise
                        let daily = ((JulianDay.Of ly lm ld lh).ShiftHours (float(ix) * dayHourLen)).ShiftHours (p_zoneDiff * 1.<hh>)
                        let Cd (xY, xM, xD, xH) as zx = CalendarDay.Of daily
                        writer.WriteLine("[{0}],[{1}],Day", Degree.Of (1.<deg> * xH / 1.<hh>), l)
                    else
                        let Cd (ly, lm, ld, lh) as zx = calSunRise
                        let nightly = ((JulianDay.Of ly lm ld lh).ShiftHours (12. * dayHourLen + float(ix - 12) * nightHourLen)).ShiftHours (p_zoneDiff * 1.<hh>)
                        let Cd (xY, xM, xD, xH) as zxx = CalendarDay.Of nightly
                        writer.WriteLine("[{0}],[{1}],Night", Degree.Of(1.<deg> * xH / 1.<hh>), l)
            ()
        | _ ->
            ()

        previous := Some (riseAt, setAt, TimeZoneDiff <| CalendarDay.Of riseAt)

        let Jd __jd1 as zxj = (Jd !jd).ShiftDays 1.<dd>
        jd := __jd1

    writer.Flush()
    Close()

