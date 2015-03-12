module CelestialEvents

open System
open System.Globalization

open Calculator
open Factory
open Studio

type CelestialEvent = 
    | Aspected  
    | Rised  
    | Set
    | InSign
    | InMansion
    | DirectionChanged
    static member private Strings =
        Map
            [ (Aspected          , "Aspected"        )
              (Rised             , "Rised"           )
              (Set               , "Set"             )
              (InSign            , "InSign"          )
              (InMansion         , "InMansion"       )
              (DirectionChanged  , "DirectionChanged") ]
    static member private Parsed =
        Map
            [ ("Aspected"        ,        Aspected        )
              ("Rised"           ,        Rised           )
              ("Set"             ,        Set             )
              ("InSign"          ,        InSign          )
              ("InMansion"       ,        InMansion       )
              ("DirectionChanged",        DirectionChanged) ]
    static member private Numbers =
        Map
            [ (Aspected          , 1)
              (Rised             , 2)
              (Set               , 3)
              (InSign            , 4)
              (InMansion         , 5)
              (DirectionChanged  , 6) ]
    static member private Denumbers =
        Map
            [ (1, Aspected        )
              (2, Rised           )
              (3, Set             )
              (4, InSign          )
              (5, InMansion       )
              (6, DirectionChanged) ]
    override x.ToString() =
        CelestialEvent.Strings.[x]
    static member Denumber(c: int) =
        CelestialEvent.Denumbers.[c]
    static member Decode x =
        CelestialEvent.Denumbers.[x]
    static member Parse s =
        CelestialEvent.Parsed.[s]
    interface ICoder with
        member x.Code() =
            CelestialEvent.Numbers.[x]
    interface INumber<int> with
        member x.Number() =
            CelestialEvent.Numbers.[x]

[<CLIMutable>]
type Moment =
    {
        Event: CelestialEvent
        Jd: JulianDay
        Planets: Planet list
        Signs: Sign list
        Directions: Direction list
        Aspects: Aspect list
        Mansion: IslamicMansion option
        AtPeak: bool option
    }
    override m1.ToString() =
        let _, _, _, _, gd = TimeZoneDiff <| CalendarDay.Of m1.Jd

        let weekDay = gd.DayOfWeek
        let pdStr = persianDateString gd
        let gdStr = String.Format("{0:yyyy-MM-dd HH:mm:ss}", gd)
        let e = m1.Event
        let pl, sl, dl, al = (m1.Planets |> List.ofSeq |> listStr (fun pa -> pa.ToString())), (m1.Signs |> List.ofSeq |> listStr (fun sa -> sa.ToString())), (m1.Directions |> List.ofSeq |> listStr (fun da -> da.ToString())), (m1.Aspects |> List.ofSeq |> listStr (fun aaa -> aaa.ToString()))
        let mans = match m1.Mansion with | Some man -> !~ man | _ -> Studio.``~``.Empty
        let atp = match m1.AtPeak with | Some bb -> bb.ToString() | _ -> String.Empty
        
        csv [| weekDay; pdStr; gdStr; e; atp; pl; sl; dl; al; mans |]
    static member CsvHeader() =
        csv [| "Week Day"; "Persian Date"; "Date Time"; "Event"; "At Peak"; "Planets"; "Signs"; "Directions"; "Aspects"; "Mansion" |]

