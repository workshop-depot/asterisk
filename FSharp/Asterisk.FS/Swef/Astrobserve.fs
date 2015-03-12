module Astrobserve

open System
open System.IO
open System.Text

open Sweph
open Sweph.Constants
open Factory
open Studio

let private init = 
    lazy
        swe_julday <- Sweph.swe_julday
        swe_jdut1_to_utc <- Sweph.swe_jdut1_to_utc
        true

do
    init.Force() |> ignore

let initTrigger = (do ()); true

let mutable private _opening_iflag : int    = (int Bag.SEFLG_SPEED) ||| (int Bag.SEFLG_MOSEPH)
let mutable private _positionType           = Bag.SEFLG_SIDEREAL
let mutable private _active_iflag  : int    = _opening_iflag ||| (int _positionType)   
let mutable private _houseSystem   : int    = Bag.EX_HOUSE_SYS_K
let mutable private _siderealMode  : int    = Bag.SE_SIDM_LAHIRI
let mutable private _ascendant     : int    = Bag.SE_ASC

let Open (ephemerisPath: string option) = 
    match ephemerisPath with
    | None -> ()
    | Some s -> 
        let ep = !~ s
        if Directory.Exists ep then
            Sweph.swe_set_ephe_path(ep) |> ignore
            _opening_iflag <- (_opening_iflag &&& (~~~ (int Bag.SEFLG_MOSEPH))) ||| (int Bag.SEFLG_SWIEPH)

let Close () =
    if Bag.SEFLG_SWIEPH = ((_opening_iflag |> Convert.ToInt64) &&& Bag.SEFLG_SWIEPH) then
        Sweph.swe_close()

type Config =
    { SiderealMode: int64; PositionType: int64; HouseSystem: int64; NodeType: int64 }
    static member Default() =
        let dv = { SiderealMode = int64 _siderealMode 
                   PositionType = _positionType 
                   HouseSystem  = int64 _houseSystem 
                   NodeType     = int64 nodeType }
        dv

let private _Setup (c: Config) =
    _siderealMode <- int c.SiderealMode
    Sweph.swe_set_sid_mode(_siderealMode, 0., 0.)

    _active_iflag <- _opening_iflag ||| (int c.PositionType) ||| (Bag.SEFLG_SPEED |> int)
    _houseSystem  <- int c.HouseSystem
    nodeType      <- int c.NodeType

    ()

let private _ObserveHouses (position: Position) (time: JulianDay) =
    let Jd jd as zx = time 
    let cusps = Array.zeroCreate<float> 13
    let ascmc = Array.zeroCreate<float> 10
    Sweph.swe_houses_ex(jd / 1.<dd>, _active_iflag, position.Latitude / 1.<deg>, position.Longitude / 1.<deg>, _houseSystem, cusps, ascmc) |> ignore
    (ascmc, cusps)

let private _ObserveAscendant (position: Position) (time: JulianDay) (asc: int) =
    let ascmc, cusps = _ObserveHouses position time
    ascmc.[asc] * 1.<deg>

let private _ObservePoint (id: Planet) (time: JulianDay) = 
    let xx = Array.zeroCreate<float> 6
    let serr = new StringBuilder(256)
    let Jd jd as zx = time
    Sweph.swe_calc_ut(jd / 1.<dd>, (id :> INumber<int>).Number(), _active_iflag, xx, serr) |> ignore
    // Sweph.swe_fixstar_ut(name, jd / 1.<dd>, _active_iflag |> Convert.ToInt64, xx, serr) |> Convert.ToInt32
    let pointPosition =
        match id with
        | Ke -> 
            let lon = (normalize (xx.[0] + 180.) 360.) * 1.<deg>
            let lat = xx.[1] * -1.<deg>
            { Longitude = lon; Latitude = lat } 
        | _ -> 
            let lon = xx.[0] * 1.<deg>
            let lat = xx.[1] * 1.<deg>
            { Longitude = lon; Latitude = lat } 
    { Id = id 
      Position = pointPosition 
      Flow = { Distance = xx.[2] * 1.<AU>; SpeedInLongitude = xx.[3] * 1.<deg/dd>; SpeedInLatitude = xx.[4] * 1.<deg/dd>; SpeedInDistance = xx.[5] * 1.<AU/dd> } }

let private _ObserveTransits (id: Planet) (position: Position) (time: JulianDay) (transitFlags: int32): JulianDay =
    let Jd jd as zx = time
    let tjd_ut = jd / 1.<dd>
    // ipl, starname <- id
    let epheflag = _active_iflag
    let rsmi = transitFlags
    let geopos = [| position.Longitude / 1.<deg>; position.Latitude / 1.<deg>; 0. |]
    let atpress = 0.
    let attemp = 0.
    let tret = ref 0.
    let serr = new StringBuilder(256)

    Sweph.swe_rise_trans(tjd_ut, (id :> INumber<int>).Number(), new StringBuilder(), epheflag, rsmi, geopos, atpress, attemp, tret, serr) |> ignore
    // Sweph.swe_rise_trans(tjd_ut, -1, name, epheflag, rsmi, geopos, atpress, attemp, tret, serr) |> ignore

    Jd <| !tret * 1.<dd>

type private pchan = Point chan
type private achan = float<deg> chan
type private tchan = JulianDay chan
type private schan = bool chan

/// channel batch ~ channels in a Go's select
type private AstroQuery = 
    | ForPlanet of Planet * JulianDay * pchan
    | ForAscendant of Position * JulianDay * int * achan
    | ForTransit of Planet * Position * JulianDay * (TransitFlag list) * tchan
    | ForSetup of Config * schan

let private astrobserve = 
    agent.Start(
        fun inbox ->
            let rec loop () = 
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | ForPlanet (id, t, chan) ->
                        do chan.Reply <| _ObservePoint id t
                    | ForAscendant (pos, t, asc, chan) ->
                        do chan.Reply <| _ObserveAscendant pos t asc
                    | ForTransit (pid, pos, t, trans, chan) ->
                        let transFlag = 
                            match trans with
                            | [] -> 
                                (TransitFlag.Rise :> INumber<int>).Number()
                            | _ ->
                                trans |> Seq.map (fun x -> (x :> INumber<int>).Number()) |> Seq.fold (fun x1 x2 -> x1 ||| x2) 0
                    
                        do chan.Reply <| _ObserveTransits pid pos t transFlag
                    | ForSetup (st, chan) ->
                        _Setup st
                        do chan.Reply <| true

                    return! loop ()
                }
            loop ())

let ObservePoint (id: Planet) (time: JulianDay) =
    let jd = time
    astrobserve.PostAndReply(fun c -> ForPlanet (id, jd, c))

let ObserveAscendant (pos: Position) (time: JulianDay) (asc: int) =
    let jd = time
    astrobserve.PostAndReply(fun chan -> ForAscendant (pos, jd, asc, chan))

let ObserveTransit pid position time transitFlags =
    astrobserve.PostAndReply(fun chan -> ForTransit (pid, position, time, transitFlags, chan))

let BasicSetup (c: Config) =
    astrobserve.PostAndReply(fun chan -> ForSetup (c, chan)) 
