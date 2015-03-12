module Studio

let ``°`` = "°"

open System
open System.Globalization
open System.Security.Cryptography

type ``~`` = String

let text oj = 
    let o = box oj
    if o = null then System.String.Empty 
    else o.ToString()

let fmt f ([<ParamArray>] arr : obj[]) = ``~``.Format(f, arr)
let inline (%~) f ([<ParamArray>] arr : obj[]) = fmt f arr

let jns s ([<ParamArray>] arr : obj[]) = ``~``.Join(s, arr)
let inline (%~+) s ([<ParamArray>] arr : obj[]) = jns s arr

let inline (!~) o = text o

let inline (!~?) o = 
    match o with
    | Some a -> !~ a |> Some
    | _ -> None

let polishCsv o =
    let s = !~ o
    let ps = ref s
    if (!ps).Contains(",") then
        ps := fmt "\"{0}\"" [| !ps |]
    if (!ps).Contains("\"") then
        ps := (!ps).Replace("\"", "\"\"") 
        ps := fmt "\"{0}\"" [| !ps |]
    !ps

let csv ([<ParamArray>] arr : obj[]) = 
    let par = 
        seq {
            for s in arr do
                yield ((polishCsv s) :> Object)
        } |> List.ofSeq |> List.toArray
    jns "," par

let equalsOn f x (yobj:obj) =
    match yobj with
    | :? 'T as y -> (f x = f y)
    | _ -> false
 
let hashOn f x =  hash (f x)
 
let compareOn f x (yobj: obj) =
    match yobj with
    | :? 'T as y -> compare (f x) (f y)
    | _ -> invalidArg "yobj" "cannot compare values of different types"

let normalize (l: float) (factor: float) =
    let mutable n = l
    while n < 0.0 do
        n <- n + factor
    while n >= factor do
        n <- n - factor
    n

let rangedIn (x: float) (inclusiveStart: float) (exclusiveEnd: float) =
        let mutable d = x
        let len = exclusiveEnd - inclusiveStart
        while (d >= exclusiveEnd) do d <- d - len
        while (d < inclusiveStart) do d <- d + len
        d

type 'a chan = AsyncReplyChannel<'a>
type 'a agent = MailboxProcessor<'a>

let inline (!&) (l: Lazy<'a>) = l.Force()

let inline (!++) (i: int ref) =
    let current = !i
    incr i
    current 

let persianDateString (d: DateTime) =
    let pcal = PersianCalendar()
    let gd = d
    let py, pm, pd = pcal.GetYear(gd), pcal.GetMonth(gd), pcal.GetDayOfMonth(gd)
    String.Format("{0:0000}`{1:00}`{2:00}", py, pm, pd)

type DateTime with
    member x.Truncate (timeSpan: TimeSpan) =
        x.AddTicks(-(x.Ticks % timeSpan.Ticks))
    member x.ToPersianDateString() =
        persianDateString x

let listStr (sof: 'a -> String) l =
    String.Join(", ", l |> List.map sof |> Array.ofList)

let tupOf l =
    match l with
    | i1 :: i2 :: _ -> Some i1, Some i2
    | i1 :: _ -> Some i1, None
    | _ -> None, None

//---------------------------------------------------------------
// sequential Guid

type GuidType =
    /// MySQL & PostgreSQL
    | AsString
    /// Oracle
    | AsBinary
    /// SQL Server
    | AtEnd

type private Xuid() = 
    let _rng = new RNGCryptoServiceProvider()
    let mutable _counter: uint16 = uint16 0
    let max = UInt16.MaxValue - (uint16 2)
    let one = uint16 1
    let zero = uint16 0

    member x.Gen (t: GuidType) (d: DateTime option) =
        let dt =
            match d with
            | Some t -> t
            | _ -> DateTime.UtcNow

        _counter <- _counter + one
        if _counter >= max then _counter <- zero

        let randomBytes =  Array.zeroCreate<byte> 10
        _rng.GetBytes(randomBytes)

        let counterBytes = BitConverter.GetBytes(_counter)

        randomBytes.[0] <- counterBytes.[0]
        randomBytes.[1] <- counterBytes.[1]

        let timestamp = dt.Ticks / 10000L
        let timestampBytes = BitConverter.GetBytes(timestamp)

        if BitConverter.IsLittleEndian then Array.Reverse(timestampBytes)

        let guidBytes =  Array.zeroCreate<byte> 16

        match t with
        | AsString
        | AsBinary ->
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6)
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10)

            // If formatting as a string, we have to reverse the order
            // of the Data1 and Data2 blocks on little-endian systems.
            if t = AsString && BitConverter.IsLittleEndian then
                Array.Reverse(guidBytes, 0, 4)
                Array.Reverse(guidBytes, 4, 2)
            ()
        | AtEnd ->
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10)
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6)
            ()
        
        Guid(guidBytes)

type private xchan = Guid chan
type private xquery = GuidType * (DateTime option) * xchan
let private xagent = 
    agent.Start(
        fun inbox ->
            let gen = Xuid()

            let rec loop () = 
                async {
                    let! msg = inbox.Receive()

                    let t, dt, (chan: xchan) = msg
                    do chan.Reply <| gen.Gen t dt

                    return! loop ()
                }
            loop ())

let GenXuid (t: GuidType) (d: DateTime option) =
    xagent.PostAndReply(fun c -> t, d, c)

// sequential Guid
//---------------------------------------------------------------
