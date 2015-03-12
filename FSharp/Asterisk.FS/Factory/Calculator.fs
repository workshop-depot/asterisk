module Calculator

open System
open System.IO
open System.Globalization
open System.Text
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Sweph.Constants
open Factory
open Studio

let IsDirect (speedInLongitude: float<deg/dd>) = speedInLongitude >= 0.<deg/dd>

let IsNightly (asc: float<deg>) (su: float<deg>) =
    let mutable sunLen = su
    let ascLen = asc

    if sunLen < ascLen then sunLen <- sunLen + 360.<deg>

    if ascLen < sunLen && sunLen < ascLen + 180.<deg> then 
        true
    else
        false

type Arc = { Start: float<deg>; End: float<deg> }

type Sub = { Ruler: Planet; Arc: Arc }

type Nakshatra = { Name: String; 
                   Ruler: Planet; 
                   Arc: Arc; 
                   Subs: Sub list }

let WeekLords = 
    Map
        [ (Saturday , Sa)
          (Sunday   , Su)
          (Monday   , Mo)
          (Tuesday  , Ma)
          (Wednesday, Me)
          (Thursday , Ju)
          (Friday   , Ve) ]

type Division =
    | D1 
    | D2 
    | D3 
    | D4 
    | D5 
    | D6 
    | D7 
    | D8 
    | D9 
    | D10
    | D11
    | D12
    | D16
    | D20
    | D24
    | D27
    | D30
    | D40
    | D45
    | D60
    member x.Name() = 
        match x with
        | D1  -> "Rasi"
        | D2  -> "Hora"
        | D3  -> "Drekkana"
        | D4  -> "Chaturthamsa"
        | D5  -> "Panchamsa"
        | D6  -> "Shashthamsa"
        | D7  -> "Saptamsa"
        | D8  -> "Ashtamsa"
        | D9  -> "Navamsa"
        | D10 -> "Dasamsa"
        | D11 -> "Rudramsa"
        | D12 -> "Dwadasamsa"
        | D16 -> "Shodasamsa"
        | D20 -> "Vimsamsa"
        | D24 -> "Chaturvimsamsa"
        | D27 -> "Nakshatramsa"
        | D30 -> "Trimsamsa"
        | D40 -> "Khavedamsa"
        | D45 -> "Akshavedamsa"
        | D60 -> "Shashtyamsa"
    interface INumber<int> with
        member x.Number() =
            match x with
            | D1  -> 1 
            | D2  -> 2 
            | D3  -> 3 
            | D4  -> 4 
            | D5  -> 5 
            | D6  -> 6 
            | D7  -> 7 
            | D8  -> 8 
            | D9  -> 9 
            | D10 -> 10
            | D11 -> 11
            | D12 -> 12
            | D16 -> 16
            | D20 -> 20
            | D24 -> 24
            | D27 -> 27
            | D30 -> 30
            | D40 -> 40
            | D45 -> 45
            | D60 -> 60

let Diptamsa = 
    Map
        [ (Su, 15.<deg>)
          (Mo, 12.<deg>)
          (Me, 7.<deg>)
          (Ve, 7.<deg>)
          (Ma, 8.<deg>)
          (Ju, 9.<deg>)
          (Sa, 9.<deg>)
          (Ur, 5.<deg>)
          (Ne, 5.<deg>)
          (Pl, 5.<deg>)
          (Ra, 8.<deg>)
          (Ke, 8.<deg>) ]

let TrirashiDay = 
    [ (Aries, Su)
      (Taurus, Ve)
      (Gemini, Sa)
      (Cancer, Ve)
      (Leo, Ju)
      (Virgo, Mo)
      (Libra, Me)
      (Scorpio, Ma)
      (Sagittarius, Sa)
      (Capricorn, Ma)
      (Aquarius, Ju)
      (Pisces, Mo) ]

let TrirashiNight =
    [ (Aries, Ju)
      (Taurus, Mo)
      (Gemini, Me)
      (Cancer, Ma)
      (Leo, Su)
      (Virgo, Ve)
      (Libra, Sa)
      (Scorpio, Ve)
      (Sagittarius, Sa)
      (Capricorn, Ma)
      (Aquarius, Ju)
      (Pisces, Mo) ]

type Trinity =
    | Movable
    | Fixed
    | Dual

let TrinityNature =
    [ (Aries, Movable)
      (Cancer, Movable)
      (Libra, Movable)
      (Capricorn, Movable)
      (Taurus, Fixed)
      (Leo, Fixed)
      (Scorpio, Fixed)
      (Aquarius, Fixed)
      (Gemini, Dual)
      (Virgo, Dual)
      (Sagittarius, Dual)
      (Pisces, Dual) ]

type Element =
    | Fire
    | Water
    | Air
    | Earth

let Elementals =
    [ (Aries, Fire)
      (Leo, Fire)
      (Sagittarius, Fire)
      (Taurus, Earth)
      (Virgo, Earth)
      (Capricorn, Earth)
      (Gemini, Air)
      (Libra, Air)
      (Aquarius, Air)
      (Cancer, Water)
      (Scorpio, Water)
      (Pisces, Water) ]

let RulingYears =
    [ (Ke, 7)
      (Ve, 20)
      (Su, 6)
      (Mo, 10)
      (Ma, 7)
      (Ra, 18)
      (Ju, 16)
      (Sa, 19)
      (Me, 17) ]

let Lordship = 
    Map
        [ (Aries, Ma)
          (Taurus, Ve)
          (Gemini, Me)
          (Cancer, Mo)
          (Leo, Su)
          (Virgo, Me)
          (Libra, Ve)
          (Scorpio, Ma)
          (Sagittarius, Ju)
          (Capricorn, Sa)
          (Aquarius, Sa)
          (Pisces, Ju) ]

let private _nakshatraLen = 360.<deg> / 27.
let private _subUnitLen = _nakshatraLen / 120.

let private InitNakshatras() =
    let nameAndRuler = 
        [ ("Ashvinī", Ke)
          ("Bharanī", Ve)
          ("Krittikā", Su)
          ("Rohini", Mo)
          ("Mrigashīrsha", Ma)
          ("Ārdrā", Ra)
          ("Punarvasu", Ju)
          ("Pushya", Sa)
          ("Āshleshā", Me)
          ("Maghā", Ke)
          ("Pūrva Phalgunī", Ve)
          ("Uttara Phalgunī", Su)
          ("Hasta", Mo)
          ("Chitrā", Ma)
          ("Svātī", Ra)
          ("Vishākhā", Ju)
          ("Anurādhā", Sa)
          ("Jyeshtha", Me)
          ("Mūla", Ke)
          ("Pūrva Ashādhā", Ve)
          ("Uttara Ashadha", Su)
          ("Shravana", Mo)
          ("Shravishthā", Ma)
          ("Shatabhishā", Ra)
          ("Pūrva Bhādrapadā", Ju)
          ("Uttara Bhādrapadā", Sa)
          ("Revatī", Me) ]

    let mutable nakshatras = []

    let mutable previousNakshatraArcEnd = 0.<deg>
    for name, ruler in nameAndRuler do
        let arc = { Start = previousNakshatraArcEnd; End = previousNakshatraArcEnd + _nakshatraLen }

        let mutable subs: Sub list = []

        let mutable previousSubArcEnd = arc.Start
        for k, v in RulingYears |> Seq.skipWhile (fun x -> fst x <> ruler) do
            let len = (v |> float) * _subUnitLen
            let sub = { Sub.Ruler = k; Arc = { Start = previousSubArcEnd; End = previousSubArcEnd + len } }
            subs <- sub :: subs
            previousSubArcEnd <- sub.Arc.End
        for k, v in RulingYears |> Seq.takeWhile (fun x -> fst x <> ruler) do
            let len = (v |> float) * _subUnitLen
            let sub = { Sub.Ruler = k; Arc = { Start = previousSubArcEnd; End = previousSubArcEnd + len } }
            subs <- sub :: subs
            previousSubArcEnd <- sub.Arc.End
            
        let nak = { Name = name; Ruler = ruler; Arc = arc; Subs = subs }
        nakshatras <- nak :: nakshatras
        previousNakshatraArcEnd <- nak.Arc.End

    nakshatras

let Nakshatras = InitNakshatras() @ List.empty

let FindNakshatra (lon: float<deg>) =
    let nakshatra = [ for n in Nakshatras do
                      if n.Arc.Start <= lon && lon < n.Arc.End then yield n ] |> List.head
    let sub = [ for subItem in nakshatra.Subs do
                if subItem.Arc.Start <= lon && lon < subItem.Arc.End then 
                    yield subItem ] |> List.head
    nakshatra, sub

type Dignity =
    { ExaltatedAt: Sign * float<deg>;
      DebilitatedAt: Sign * float<deg> }

let Dignities = 
    Map
        [ (Su, { ExaltatedAt = Aries, 10.<deg>; DebilitatedAt = Libra, 10.<deg> })
          (Mo, { ExaltatedAt = Taurus, 3.<deg>; DebilitatedAt = Scorpio, 3.<deg> })
          (Ma, { ExaltatedAt = Capricorn, 28.<deg>; DebilitatedAt = Cancer, 28.<deg> }) 
          (Me, { ExaltatedAt = Virgo, 15.<deg>; DebilitatedAt = Pisces, 15.<deg> }) 
          (Ju, { ExaltatedAt = Cancer, 5.<deg>; DebilitatedAt = Capricorn, 5.<deg> }) 
          (Ve, { ExaltatedAt = Pisces, 27.<deg>; DebilitatedAt = Virgo, 27.<deg> }) 
          (Sa, { ExaltatedAt = Libra, 20.<deg>; DebilitatedAt = Aries, 20.<deg> }) 
          (Ra, { ExaltatedAt = Gemini, -1.<deg>; DebilitatedAt = Sagittarius, -1.<deg> })
          (Ke, { ExaltatedAt = Sagittarius, -1.<deg>; DebilitatedAt = Gemini, -1.<deg> }) ]

let TimeZoneDiff (t: CalendarDay) =
    let pcal = new PersianCalendar()

    let Cd (y, m, d, h) as zx = t
    let atDate = (new DateTime(y / 1<YY>, m / 1<MM>, d / 1<dd>)).AddHours(h / 1.<hh>)

    let pY = pcal.GetYear(atDate)
    let pMon = pcal.GetMonth(atDate)
    let pDay = pcal.GetDayOfMonth(atDate)

    if pMon <= 6 then
        4.5, pY, pMon, pDay, atDate
    else 
        3.5, pY, pMon, pDay, atDate

type CelestialId = 
    | Body of Planet
    | Angle of int
    override x.ToString() =
        match x with
        | Body p -> p.ToString()
        | Angle i -> String.Format("Angle {0}", i)

type Site = 
    | Longitude of float<deg>
    | Disposition of Position * Flow

type AsterBook(id_: CelestialId, locus_: Site ) =
    let mutable _house: House option = None
    let mutable _ascSign: Sign option = None

    member x.Id = id_
    member x.Locus = locus_

    member x.Longitude 
        with get() =
            match x.Locus with
            | Disposition (p, _) -> p.Longitude
            | Longitude l -> l

    member private x._nakshatra = 
        lazy 
            FindNakshatra x.Longitude 
    member x.Nakshatra
        with get() =
            !& x._nakshatra
    
    member private x._sign =
        lazy
            Sign.Of x.Longitude 
    member x.Sign 
        with get () =
            !& x._sign

    member x.House (?ascSign) = 
        match _house, ascSign with
        | Some h, _ -> h
        | _, Some ascSg ->
            let h = House.Of ascSg x.Sign
            _house <- h |> Some
            h
        | _ -> failwith "_house is not initialized (fix it by setting ascSign)"

let (|InRange|_|) (degree: float) (boundry: float) (topDiff: float) (input: float) =
    let f = input
    let low = degree - boundry
    let high = degree + boundry
    let dd = Math.Abs(f - degree)

    let isin = low < f && f < high
    let isdeep = dd <= Math.Abs(topDiff)

    match isin, isdeep with
    | true, true -> Some (degree, true)
    | true, false -> Some (degree, false)
    | _ -> None

type Aspect =
    | Asp_60 
    | Asp_90 
    | Asp_120
    | Asp_180
    | Cj
    | Unaspected

    override x.ToString() =
        match x with
        | Asp_60      -> String.Format("{0}", "60" + Studio.``°``)
        | Asp_90      -> String.Format("{0}", "90" + Studio.``°``)
        | Asp_120     -> String.Format("{0}", "120" + Studio.``°``)
        | Asp_180     -> String.Format("{0}", "180" + Studio.``°``)
        | Cj          -> String.Format("{0}", "Conjunction")
        | Unaspected  -> String.Format("{0}", "Unaspected")

    static member private Codes =
        Map
            [ (Asp_60      , 60 )
              (Asp_90      , 90 )
              (Asp_120     , 120)
              (Asp_180     , 180)
              (Cj          , 0  )
              (Unaspected  , -1 ) ]

    static member private Decodes =
        Map
            [ (60 , Asp_60    )
              (90 , Asp_90    )
              (120, Asp_120   )
              (180, Asp_180   )
              (0  , Cj        )
              (-1 , Unaspected) ]

    static member private Numbers =
        Map
            [ (Asp_60       , 60.<deg> )
              (Asp_90       , 90.<deg> )
              (Asp_120      , 120.<deg>)
              (Asp_180      , 180.<deg>) 
              (Cj           , 0.<deg>  ) 
              (Unaspected   , -1.<deg> )  ]

    static member Denumbers =
        Map
            [ (60.<deg> , Asp_60    )
              (90.<deg> , Asp_90    )
              (120.<deg>, Asp_120   )
              (180.<deg>, Asp_180   )
              (0.<deg>  , Cj        )
              (-1.<deg> , Unaspected) ]

    static member Decode x =
        Aspect.Decodes.[x]

    static member Of (p1: CelestialId * float<deg>) (p2: CelestialId * float<deg>) =
        let diff = 
            match rangedIn (Math.Abs((snd p1 - snd p2 - 360.<deg>) / 1.<deg>)) 0. 360. with
            | v when v > 180. -> rangedIn (Math.Abs(v - 360.)) 0. 360.
            | v2 -> v2

        let topDiff = 0.001
        let aspDeg =
            match diff with
            | InRange 60.   6. topDiff x -> x |> Some
            | InRange 90.   6. topDiff x -> x |> Some
            | InRange 120.  6. topDiff x -> x |> Some
            | InRange 180.  8. topDiff x when 2 <> ([ fst p1; fst p2 ] |> List.filter (fun pa -> match pa with | Body pm -> pm = Ra || pm = Ke | _ -> false) |> List.length) -> 
                x |> Some
            | _ -> None

        match aspDeg with
        | Some (asp, atTop) -> (Aspect.Denumbers.[asp * 1.<deg>], atTop) |> Some
        | _ ->
            match (fst p1, fst p2) with
            | (Angle _, _) | (_, Angle _) -> None
            | (Body pla1, Body pla2) -> 
                let isCj = diff < ((Diptamsa.[pla1]) + (Diptamsa.[pla2])) / 1.<deg>
                let isTop = diff <= topDiff
                match isCj, isTop with
                | true, true -> (Cj, true) |> Some
                | true, false -> (Cj, false) |> Some
                | _ -> None

    interface ICoder with
        member x.Code() =
            Aspect.Codes.[x]
    interface INumber<float<deg>> with
        member x.Number() =
            Aspect.Numbers.[x]

[<CustomEquality; CustomComparison>]
type Aster =
    | T of CelestialId * AsterBook

    static member ef (T (id, _)) = id

    override x.Equals y = equalsOn Aster.ef x y
    override x.GetHashCode() = hashOn Aster.ef x
    interface System.IComparable with
        member x.CompareTo y = compareOn Aster.ef x y

type ChartSet<'T> = 
    { Asc: 'T;
      Su:  'T;
      Mo:  'T;
      Me:  'T;
      Ve:  'T;
      Ma:  'T;
      Ju:  'T;
      Sa:  'T;
      Ur:  'T;
      Ne:  'T;
      Pl:  'T;
      Ra:  'T;
      Ke:  'T }
    member x.All =
        [ yield x.Asc 
          yield x.Su
          yield x.Mo
          yield x.Me
          yield x.Ve
          yield x.Ma
          yield x.Ju
          yield x.Sa
          yield x.Ur
          yield x.Ne
          yield x.Pl
          yield x.Ra
          yield x.Ke ]

let private mansionLen = 1.<deg> * 360.0 / 28.0

[<CLIMutable>]
type IslamicMansion = 
    { Name: String;
      Arc: Arc }
    override x.ToString() =
        x.Name

    static member private _list = 
        let hpp x = 
            (!++ x) |> float
        
        lazy 
            seq {
                let currentMansion = ref 1

                yield { Name = "شَرَطَین - نَطح"  ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "بُطَین"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "ثُرَیّا"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "دَبَران"        ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "هَقعَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "هَنعَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "ذِراع"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "نَثرَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "طَرف"           ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "جَبهَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "زُبرَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "صَرفَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "عَوّاء"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "سِمّاک الاعزَل"   ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "غَفر"           ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "زُبانا"        ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "اِکلیل"        ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "قَلب"           ; Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "شولَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "نَعائم"        ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "بَلدَه"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "سَعدُ الذابِح"   ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "سَعدُ بُلَع"      ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "سَعدُ سُعود"     ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "سَعدُالاَخبیَّه"    ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "فَرغُ الاَوَّل"     ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "فَرغُ الثانی"   ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
                yield { Name = "رِشاء"         ;  Arc = { Start = float(!currentMansion - 1) * mansionLen; End = hpp currentMansion * mansionLen } }
            } |> Seq.toArray

    static member List
        with get() =
            !& (IslamicMansion._list)

    static member FindMansion (lon: float<deg>) = 
        Seq.tryFind (fun x -> x.Arc.Start <= lon && lon < x.Arc.End) (IslamicMansion.List)
