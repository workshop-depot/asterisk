module Factory

open System
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Studio
open Sweph.Constants

/// minute
[<Measure>]
type mm =
  static member mm(s) = s / 60.<s> * 1.<mm>
  static member s(mm) = mm * 60.<s> / 1.<mm>

/// hour
[<Measure>]
type hh =
  static member hh(mm) = mm / 60.<mm> * 1.<hh>
  static member mi(hh) = hh * 60.<mm> / 1.<hh>

/// day
[<Measure>]
type dd =
  static member dd(hh) = hh / 24.<hh> * 1.<dd>
  static member hh(dd) = dd * 24.<hh> / 1.<dd>

[<Measure>]
type week =
  static member week(dd) = dd / 7.<dd> * 1.<week>
  static member dd(week) = week * 7.<dd> / 1.<week>

// months and years has not fixed sizes

/// month
[<Measure>]
type MM

/// year
[<Measure>]
type YY

/// degree
[<Measure>]
type deg =
  static member deg(mm) = mm / 60.<mm> * 1.<deg>
  static member mi(deg) = deg * 60.<mm> / 1.<deg>

[<Measure>]
type AU

type ICoder =
    abstract Code: unit -> int

type INumber<'a> =
    abstract Number: unit -> 'a

let mutable nodeType : int = Bag.SE_TRUE_NODE
let mutable swe_julday = fun (year: int, month: int, day: int, hour: float, gregflag: int) -> 1.
let mutable swe_jdut1_to_utc = fun (tjd_ut: float, gregflag: int, iyear: int ref, imonth: int ref, iday: int ref, ihour: int ref, imin: int ref, dsec: float ref) -> ()

type Sign =
    | Aries
    | Taurus
    | Gemini
    | Cancer
    | Leo
    | Virgo
    | Libra
    | Scorpio
    | Sagittarius
    | Capricorn
    | Aquarius
    | Pisces
    static member private Strings = 
        Map
            [ (Aries,       "Aries")
              (Taurus,      "Taurus")
              (Gemini,      "Gemini")
              (Cancer,      "Cancer")
              (Leo,         "Leo")
              (Virgo,       "Virgo")
              (Libra,       "Libra")
              (Scorpio,     "Scorpio")
              (Sagittarius, "Sagittarius")
              (Capricorn,   "Capricorn")
              (Aquarius,    "Aquarius")
              (Pisces,      "Pisces") ]
    static member private Numbers =
        Map
            [ (Aries,       1)
              (Taurus,      2)
              (Gemini,      3)
              (Cancer,      4)
              (Leo,         5)
              (Virgo,       6)
              (Libra,       7)
              (Scorpio,     8)
              (Sagittarius, 9)
              (Capricorn,   10)
              (Aquarius,    11)
              (Pisces,      12) ]
    static member List = 
        seq {
            yield Aries
            yield Taurus
            yield Gemini
            yield Cancer
            yield Leo
            yield Virgo
            yield Libra
            yield Scorpio
            yield Sagittarius
            yield Capricorn
            yield Aquarius
            yield Pisces
        } |> Seq.toArray
    static member private Parsed =
        Map
            [ ("Aries",       Aries)
              ("Taurus",      Taurus)
              ("Gemini",      Gemini)
              ("Cancer",      Cancer)
              ("Leo",         Leo)
              ("Virgo",       Virgo)
              ("Libra",       Libra)
              ("Scorpio",     Scorpio)
              ("Sagittarius", Sagittarius)
              ("Capricorn",   Capricorn)
              ("Aquarius",    Aquarius)
              ("Pisces",      Pisces) ]
    static member private Values =
        Map
            [ (1  ,   Aries        )
              (2  ,   Taurus       )
              (3  ,   Gemini       )
              (4  ,   Cancer       )
              (5  ,   Leo          )
              (6  ,   Virgo        )
              (7  ,   Libra        )
              (8  ,   Scorpio      )
              (9  ,   Sagittarius  )
              (10 ,   Capricorn    )
              (11 ,   Aquarius     )
              (12 ,   Pisces       ) ]
    static member Decode x =
        Sign.Values.[x]
    static member Denumber x =
        Sign.Values.[x]
    static member Parse x =
        Sign.Parsed.[x]
    static member Of (l: float<deg>) =
        let lon = l / 1.<deg>
        Sign.Denumber <| Convert.ToInt32 (Math.Floor((lon - (lon % 30.)) / 30.0) + 1.0) 
    static member Degree (l: float<deg>) =
        l % 30.<deg>
    override x.ToString() =
        Sign.Strings.[x]
    interface ICoder with
        member x.Code() =
            Sign.Numbers.[x]
    interface INumber<int> with
        member x.Number() =
            Sign.Numbers.[x]

type House = 
    | H1 
    | H2 
    | H3 
    | H4 
    | H5 
    | H6 
    | H7 
    | H8 
    | H9 
    | H10
    | H11
    | H12
    static member private Strings = 
        Map
            [ (H1,  "H1")
              (H2,  "H2")
              (H3,  "H3")
              (H4,  "H4")
              (H5,  "H5")
              (H6,  "H6")
              (H7,  "H7")
              (H8,  "H8")
              (H9,  "H9")
              (H10, "H10")
              (H11, "H11")
              (H12, "H12") ]
    static member private Numbers =
        Map
            [ (H1,   1)
              (H2,   2)
              (H3,   3)
              (H4,   4)
              (H5,   5)
              (H6,   6)
              (H7,   7)
              (H8,   8)
              (H9,   9)
              (H10,  10)
              (H11,  11)
              (H12,  12) ]
    static member List = 
        seq {
            yield H1
            yield H2
            yield H3
            yield H4
            yield H5
            yield H6
            yield H7
            yield H8
            yield H9
            yield H10
            yield H11
            yield H12
        } |> Seq.toArray
    static member private Parsed =
        Map
            [ ("H1",  H1)
              ("H2",  H2)
              ("H3",  H3)
              ("H4",  H4)
              ("H5",  H5)
              ("H6",  H6)
              ("H7",  H7)
              ("H8",  H8)
              ("H9",  H9)
              ("H10", H10)
              ("H11", H11)
              ("H12", H12) ]
    static member private Values =
        Map
            [ (1 , H1 )
              (2 , H2 )
              (3 , H3 )
              (4 , H4 )
              (5 , H5 )
              (6 , H6 )
              (7 , H7 )
              (8 , H8 )
              (9 , H9 )
              (10, H10)
              (11, H11)
              (12, H12) ]
    static member Decode x =
        House.Values.[x]
    static member Denumber x =
        House.Values.[x]
    static member Parse x =
        House.Parsed.[x]
    static member Of (ascSign: Sign) (pointSign: Sign) =
        let mutable n = (pointSign :> INumber<int>).Number() - (ascSign :> INumber<int>).Number() + 1
        if n <= 0 then
            n <- n + 12
        House.Denumber n
    override x.ToString() =
        House.Strings.[x]
    interface ICoder with
        member x.Code() =
            House.Numbers.[x]
    interface INumber<int> with
        member x.Number() =
            House.Numbers.[x]

type Planet = 
    | Su
    | Mo
    | Me
    | Ve
    | Ma
    | Ju
    | Sa
    | Ur
    | Ne
    | Pl
    | Ra
    | Ke  
    static member private Strings = 
        Map
            [ (Su,  "Su")
              (Mo,  "Mo")
              (Me,  "Me")
              (Ve,  "Ve")
              (Ma,  "Ma")
              (Ju,  "Ju")
              (Sa,  "Sa")
              (Ur,  "Ur")
              (Ne,  "Ne")
              (Pl,  "Pl")
              (Ra,  "Ra")
              (Ke,  "Ke") ]
    static member private Numbers =
        Map
            [ (Su,  Bag.SE_SUN     )
              (Mo,  Bag.SE_MOON    )
              (Me,  Bag.SE_MERCURY )
              (Ve,  Bag.SE_VENUS   )
              (Ma,  Bag.SE_MARS    )
              (Ju,  Bag.SE_JUPITER )
              (Sa,  Bag.SE_SATURN  )
              (Ur,  Bag.SE_URANUS  )
              (Ne,  Bag.SE_NEPTUNE )
              (Pl,  Bag.SE_PLUTO   )
              (Ra,  nodeType        )
              (Ke,  nodeType        ) ]
    static member List = 
        seq {
            yield Su
            yield Mo
            yield Me
            yield Ve
            yield Ma
            yield Ju
            yield Sa
            yield Ur
            yield Ne
            yield Pl
            yield Ra
            yield Ke
        } |> Seq.toArray
    static member private Parsed =
        Map
            [ ("Su",  Su)
              ("Mo",  Mo)
              ("Me",  Me)
              ("Ve",  Ve)
              ("Ma",  Ma)
              ("Ju",  Ju)
              ("Sa",  Sa)
              ("Ur",  Ur)
              ("Ne",  Ne)
              ("Pl",  Pl)
              ("Ra",  Ra)
              ("Ke",  Ke) ]
    static member private Coded =
        Map
            [ (Su , 1)
              (Mo , 2)
              (Me , 3)
              (Ve , 4)
              (Ma , 5)
              (Ju , 6)
              (Sa , 7)
              (Ur , 8)
              (Ne , 9)
              (Pl , 10)
              (Ra , 11)
              (Ke , 12) ]
    static member private Decoded =
        Map
            [ (1  , Su)
              (2  , Mo)
              (3  , Me)
              (4  , Ve)
              (5  , Ma)
              (6  , Ju)
              (7  , Sa)
              (8  , Ur)
              (9  , Ne)
              (10 , Pl)
              (11 , Ra)
              (12 , Ke) ]
    static member Decode x =
        Planet.Decoded.[x]
    (*
    // can not get denumbered since Ra & Ke has the same number
    static member Denumber x =
        Planet.Values.[x]
    *)
    static member Parse x =
        Planet.Parsed.[x]
    override x.ToString() =
        Planet.Strings.[x]
    interface ICoder with
        member x.Code() =
            Planet.Coded.[x]
    interface INumber<int> with
        member x.Number() =
            Planet.Numbers.[x]

let Spheres =
    seq {
        yield Mo
        yield Me
        yield Ve
        yield Su
        yield Ma
        yield Ju
        yield Sa
    }

type Degree =
    | Dg of float<deg> * float<mm> * float<s>
    override x.ToString() =
        let Dg (d, m, s) as zx = x
        fmt "{0:00}:{1:00}:{2:00}"  [| d; m; s |]
    member x.Float() =
        let Dg (d, m, s) as zx = x
        d / 1.<deg> + m / 60.<mm> + s / 3600.<s>
    static member Of (fla: float<deg>)  =
        let l = fla / 1.<deg>
        let fl = Math.Floor(l)
        let mutable fr = l - fl

        fr <- fr * 60.0
        let m = Math.Floor(normalize fr 60.0)
        fr <- fr - m
        fr <- fr * 60.0
        let s = Math.Floor(normalize  fr 60.0)

        Dg (fl * 1.<deg>, m * 1.<mm>, s * 1.<s>)

type JulianDay =
    | Jd of float<dd>
    static member Of (year: int<YY>) (month: int<MM>) (day: int<dd>) (hour: float<hh>) =
        let y = year / 1<YY>
        let m = month / 1<MM>
        let d = day / 1<dd>
        let h = hour / 1.<hh>

        let mutable flag = Bag.SE_GREG_CAL
        if (y * 10000 + m * 100 + d) < 15821015 then
            flag <- Bag.SE_JUL_CAL

        Jd <| swe_julday(y, m, d, h, flag) * 1.<dd>

    member x.ToDay() =
        let Jd jd as zx = x
        jd

    member x.ShiftSeconds (s: float<s>) = 
        let Jd jd as zx = x
        Jd (s |> mm.mm |> hh.hh |> dd.dd |> (+) jd )

    member x.ShiftMinutes (m: float<mm>) = 
        let Jd jd as zx = x
        Jd (m |> hh.hh |> dd.dd |> (+) jd )

    member x.ShiftHours (h: float<hh>) = 
        let Jd jd as zx = x
        Jd (h |> dd.dd |> (+) jd )

    member x.ShiftDays (d: float<dd>) = 
        let Jd jd as zx = x
        Jd (d |> (+) jd )

    member x.ShiftWeeks (w: float<week>) = 
        let Jd jd as zx = x
        Jd (w |> week.dd |> (+) jd)

type CalendarDay = 
    | Cd of int<YY> * int<MM> * int<dd> * float<hh>
    override x.ToString() =
        let Cd (year, month, day, hour) as zx = x
        let Dg (h, m, s) as zxx = Degree.Of (1.<deg> * hour / 1.<hh>) 
        "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00.0}" %~ [|year; month; day; h; m; s|]
    static member Of (jdarg: JulianDay): CalendarDay =
        let year = ref 0
        let month = ref 0
        let day = ref 0
        let hour = ref 0
        let minutes = ref 0
        let dsec = ref 0.

        let Jd jd as zx = jdarg

        swe_jdut1_to_utc(jd / 1.<dd>, Bag.SE_GREG_CAL, year, month, day, hour, minutes, dsec)
        let fl = (Dg (Convert.ToDouble(!hour) * 1.<deg>, Convert.ToDouble(!minutes) * 1.<mm>, !dsec * 1.<s>)).Float()
        Cd (!year * 1<YY>, !month * 1<MM>, !day * 1<dd>, fl * 1.<hh>)
    member x.ToDateTime() =
        let Cd (y, m, d, h) as zx = x
        DateTime(y / 1<YY>, m / 1<MM>, d / 1<dd>).AddHours(h / 1.<hh>)

type Position =
    { Longitude: float<deg>; Latitude: float<deg> }    
    override x.ToString() =
        "lon: {0}, lat: {1}" %~ [| Degree.Of(x.Longitude); Degree.Of(x.Latitude) |]

type Flow =
    { Distance: float<AU>; SpeedInLongitude: float<deg/dd>; SpeedInLatitude: float<deg/dd>; SpeedInDistance: float<AU/dd> }
    override x.ToString() =
        "dist: {0:0.00000}, in-lon: {1}, in-lat: {2}, in-dist: {3}" %~ [|x.Distance; Degree.Of(x.SpeedInLongitude * 1.<dd>); Degree.Of(x.SpeedInLatitude * 1.<dd>); Degree.Of(x.SpeedInDistance * 1.<deg> / 1.<AU/dd>)|]

type Point = 
    { Id: Planet; Position: Position; Flow: Flow }
    override x.ToString() =
        "id: {0}, pos: {1}, flow: {2}" %~ [| x.Id; x.Position; x.Flow |]

type TransitFlag = 
    | Rise
    | Set
    | MTransit
    | ITransit
    | DiscCenter
    | DiscBottom
    | NoRefraction
    | CivilTwilight
    | NauticTwilight
    | AstroTwilight
    | FixedDiscSize
    static member private Numbers =
        Map [ (Rise           , Bag.SE_CALC_RISE          ) 
              (Set            , Bag.SE_CALC_SET           ) 
              (MTransit       , Bag.SE_CALC_MTRANSIT      ) 
              (ITransit       , Bag.SE_CALC_ITRANSIT      ) 
              (DiscCenter     , Bag.SE_BIT_DISC_CENTER    ) 
              (DiscBottom     , Bag.SE_BIT_DISC_BOTTOM    ) 
              (NoRefraction   , Bag.SE_BIT_NO_REFRACTION  ) 
              (CivilTwilight  , Bag.SE_BIT_CIVIL_TWILIGHT ) 
              (NauticTwilight , Bag.SE_BIT_NAUTIC_TWILIGHT) 
              (AstroTwilight  , Bag.SE_BIT_ASTRO_TWILIGHT ) 
              (FixedDiscSize  , Bag.SE_BIT_FIXED_DISC_SIZE) ]
    interface INumber<int> with
        member x.Number() =
            TransitFlag.Numbers.[x]

type Week =
    | Saturday
    | Sunday
    | Monday
    | Tuesday
    | Wednesday
    | Thursday
    | Friday
    member x.ToSys() =
        match x with
        | Saturday  -> DayOfWeek.Saturday 
        | Sunday    -> DayOfWeek.Sunday   
        | Monday    -> DayOfWeek.Monday   
        | Tuesday   -> DayOfWeek.Tuesday  
        | Wednesday -> DayOfWeek.Wednesday
        | Thursday  -> DayOfWeek.Thursday 
        | Friday    -> DayOfWeek.Friday   
    static member FromSys d =
        match d with
        | DayOfWeek.Saturday  -> Saturday 
        | DayOfWeek.Sunday    -> Sunday   
        | DayOfWeek.Monday    -> Monday   
        | DayOfWeek.Tuesday   -> Tuesday  
        | DayOfWeek.Wednesday -> Wednesday
        | DayOfWeek.Thursday  -> Thursday 
        | DayOfWeek.Friday    -> Friday   
        | _ -> failwith <| String.Format("{0} is not a day of week", d)

let DayOf (hour: float<hh>) (minute: float<mm>) (second: float<s>) =
    (hour / 24.<hh> + minute / 1440.<mm> + second / 86400.<s>) * 1.<dd>

type Direction =
    | Direct
    | Reverse
    override x.ToString() =
        match x with
        | Direct -> "Direct"
        | Reverse -> "Reverse"

    static member Of b =
        match b with
        | true -> Direct
        | false -> Reverse

    static member Of b =
        match b with
        | Some bo -> Direction.Of bo |> Some
        | _ -> None
    
    static member Decode x =
        match x with
        | 1 -> Direct
        | 2 -> Reverse
        | _ -> failwith <| "not a Direction {0}" %~ [| x |]

    interface ICoder with
        member x.Code() =
            match x with
            | Direct  -> 1
            | Reverse -> 2

