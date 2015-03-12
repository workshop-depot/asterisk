module App

open System
open System.Globalization
open System.Linq
open System.IO
open FSharp.Data
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

open Studio
open Factory
open CelestialEvents
open Calculator

let dataLog = NLog.LogManager.GetLogger("Data")
let appEventLog = NLog.LogManager.GetLogger("Event")

do
    dataLog.Info("")
    ()

let initTrigger = (do ()); true

//(*
let stringify (x: 'a option) =
    match x with
    | Some v -> v.ToString()
    | _ -> String.Empty

type events = CsvProvider<"""C:\Workshop-Temporal\aster\sample.csv""">

let pcal = new PersianCalendar()
let str2 (m: events.Row) (rowStyle: String) extraContent =
    let pMon = ref (pcal.GetMonth(m.``Date Time``))
    let td = 
        if !pMon <= 6 then
            4.5
        else 
            3.5

    let converted = m.``Date Time``.AddHours(td)

    let pY = pcal.GetYear(converted)
    pMon := pcal.GetMonth(converted)
    let pDay = pcal.GetDayOfMonth(converted)

    let persianDate = String.Format("{0:0000}-{1:00}-{2:00}", pY, !pMon, pDay)

    let atPeak = stringify(m.``At Peak``)

    let dateStr = String.Format("{0:yyyy-MM-dd}", converted)
    let timeStr = String.Format("{0:HH:mm:ss}", converted)
    (*
    let stres = ref <| String.Join(" , ", converted.DayOfWeek, timeStr, persianDate, dateStr, m.Planets, m.Aspects, "at peak: " + atPeak, m.Event, m.Mansion, m.Signs)
    if atPeak.ToLower() = true.ToString().ToLower() then
        stres := !stres + " ¯\_(ツ)_/¯"
    !stres
    *)
    let dlist = [converted.DayOfWeek.ToString(); timeStr; persianDate; dateStr; m.Planets; m.Aspects; "at peak: " + atPeak; m.Event; m.Mansion; m.Signs; extraContent]
    let cells = 
        seq {
            for d in dlist do
            let td = String.Format("<td style=\"{0}\">{1}</td>", (if atPeak.ToLower() = true.ToString().ToLower() then "background-color: lightblue" else ""), d)
            yield td
        }
    let tr = String.Format("<tr {0}>{1}</tr>", rowStyle, String.Join("\n", cells))
    tr

let extractAspects() = 
    let log = NLog.LogManager.GetCurrentClassLogger()

    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "aspects.html"
    let fp = Path.Combine(__dk, __fn)
    use wr = new StreamWriter(fp, false) 

    wr.WriteLine("""
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>aspects</title>
</head>
<body>
    <table>    
    """)

    use reader = new StreamReader("""C:\Workshop-Temporal\aster\sd.csv""")
    let source = events.Load(reader)

    let qAspects =
        query {
            for m in source.Rows do

            let planets = !~ m.Planets
            let aspects = !~ m.Aspects

            where (planets.Length >= 4 && aspects.Length >= 1)
            
            let tag = planets + aspects.PadLeft(12, '0')
            groupValBy m tag into g

            sortBy g.Key

            select g
        }

    let pqAspects =
        query {
            for mg in qAspects do
            let gfst = 
                query {
                    for m in mg do
                    sortBy (m.``Date Time``) 
                    select m
                } |> List.ofSeq
            let gsnd = gfst.Skip(1) |> List.ofSeq
            let paired = Seq.zip gfst gsnd |> List.ofSeq
            select paired
        }

    let last: events.Row ref = ref null
    for mmlx in pqAspects do
        let mml = mmlx

        if !last <> null then 
            wr.WriteLine(str2 !last "" "")
        wr.WriteLine("<tr><td colspan='11' style='border-top-width: 3px; border-top-color: #333;border-top-style: solid;'></td></tr>")
        
        let written = ref false
        for m111, m222 in mml do
            let mfst, msnd = m111, m222

            let sdiff = (msnd.``Date Time`` - mfst.``Date Time``).TotalSeconds * 1.<s>
            let isGap = sdiff > 15.<s>
            let samePeak = (msnd.``At Peak`` = mfst.``At Peak``)

            if msnd.Aspects <> mfst.Aspects then 
                wr.WriteLine(str2 msnd """ style="border-top-width: thick; border-top-color: black" """ "")
            if not !written then
                wr.WriteLine(str2 msnd "" "")
                written := true
            if not samePeak then
                wr.WriteLine(str2 mfst "" "")
                wr.WriteLine(str2 msnd "" "")
            if isGap then
                wr.WriteLine(str2 mfst "" "&lt;-")
                wr.WriteLine(str2 msnd """ style="background-color: lightyellow" """ " [] ")
                written := false
            
            last := msnd
            ()

        ()

    wr.WriteLine("""
    </table>
</body>
</html>    
    """)
    wr.Flush()

(*
let extractMansions() = 
    let log = NLog.LogManager.GetCurrentClassLogger()

    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "mansions.txt"
    let fp = Path.Combine(__dk, __fn)
    use wr = new StreamWriter(fp, false) 

    wr.WriteLine("""
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>aspects</title>
</head>
<body>
    <table>    
    """)

    use reader = new StreamReader("""C:\Workshop-Temporal\aster\sd.csv""")
    let source = events.Load(reader)

    let qMansions =
        query {
            for m in source.Rows do

            let planets = !~ m.Planets
            let mansions = !~ m.Mansion

            where (m.Event = "InMansion")
            
            let tag = planets + mansions.PadLeft(64, '0')
            groupValBy m tag into g

            sortBy g.Key

            select g
        }
    
    let pqMansions =
        query {
            for mg in qMansions do
            let gfst = 
                query {
                    for m in mg do
                    sortBy (m.``Date Time``) 
                    select m
                } |> List.ofSeq
            let gsnd = gfst.Skip(1) |> List.ofSeq
            let paired = Seq.zip gfst gsnd |> List.ofSeq
            select paired
        }

    let last: events.Row ref = ref null
    for mmlx in pqMansions do
        let mml = mmlx

        if !last <> null then 
            wr.WriteLine(str2 !last "" "")
        wr.WriteLine(new String('=', 140))
        
        let written = ref false
        for m111, m222 in mml do
            let mfst, msnd = m111, m222

            let sdiff = (msnd.``Date Time`` - mfst.``Date Time``).TotalSeconds * 1.<s>
            let isGap = sdiff > 15.<s>
            let samePeak = (msnd.``At Peak`` = mfst.``At Peak``)

            if msnd.Aspects <> mfst.Aspects then 
                wr.WriteLine(new String('-', 140))
                wr.WriteLine(str msnd)
            if not !written then
                wr.WriteLine(str msnd)
                written := true
            if not samePeak then
                wr.WriteLine(str mfst)
                wr.WriteLine(str msnd)
            if isGap then
                wr.WriteLine(String.Format("{0} <-", str mfst))
                wr.WriteLine(str msnd)
                written := false
            
            last := msnd
            ()

        ()

    wr.WriteLine("""
    </table>
</body>
</html>    
    """)
    wr.Flush()

let extractSigns() = 
    let log = NLog.LogManager.GetCurrentClassLogger()

    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "signs.txt"
    let fp = Path.Combine(__dk, __fn)
    use wr = new StreamWriter(fp, false) 

    wr.WriteLine("""
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>aspects</title>
</head>
<body>
    <table>    
    """)

    use reader = new StreamReader("""C:\Workshop-Temporal\aster\sd.csv""")
    let source = events.Load(reader)

    let qSigns =
        query {
            for m in source.Rows do

            let planets = !~ m.Planets
            let signs = !~ m.Signs

            where (m.Event = "InSign")
            
            let tag = planets + signs.PadLeft(64, '0')
            groupValBy m tag into g

            sortBy g.Key

            select g
        }
    
    let pqSigns =
        query {
            for mg in qSigns do
            let gfst = 
                query {
                    for m in mg do
                    sortBy (m.``Date Time``) 
                    select m
                } |> List.ofSeq
            let gsnd = gfst.Skip(1) |> List.ofSeq
            let paired = Seq.zip gfst gsnd |> List.ofSeq
            select paired
        }

    let last: events.Row ref = ref null
    for mmlx in pqSigns do
        let mml = mmlx

        if !last <> null then 
            wr.WriteLine(str !last)
        wr.WriteLine(new String('=', 140))
        
        let written = ref false
        for m111, m222 in mml do
            let mfst, msnd = m111, m222

            let sdiff = (msnd.``Date Time`` - mfst.``Date Time``).TotalSeconds * 1.<s>
            let isGap = sdiff > 15.<s>
            let samePeak = (msnd.``At Peak`` = mfst.``At Peak``)

            if msnd.Aspects <> mfst.Aspects then 
                wr.WriteLine(new String('-', 140))
                wr.WriteLine(str msnd)
            if not !written then
                wr.WriteLine(str msnd)
                written := true
            if not samePeak then
                wr.WriteLine(str mfst)
                wr.WriteLine(str msnd)
            if isGap then
                wr.WriteLine(String.Format("{0} <-", str mfst))
                wr.WriteLine(str msnd)
                written := false
            
            last := msnd
            ()

        ()

    wr.WriteLine("""
    </table>
</body>
</html>    
    """)
    wr.Flush()

let extractDirections() = 
    let log = NLog.LogManager.GetCurrentClassLogger()

    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "directions.txt"
    let fp = Path.Combine(__dk, __fn)
    use wr = new StreamWriter(fp, false) 

    wr.WriteLine("""
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>aspects</title>
</head>
<body>
    <table>    
    """)

    use reader = new StreamReader("""C:\Workshop-Temporal\aster\sd.csv""")
    let source = events.Load(reader)

    let qDirections =
        query {
            for m in source.Rows do

            let planets = !~ m.Planets
            let directions = !~ m.Directions

            where (m.Event = "DirectionChanged")
            
            let tag = planets + directions.PadLeft(64, '0')
            groupValBy m tag into g

            sortBy g.Key

            select g
        }
    
    let pqDirections =
        query {
            for mg in qDirections do
            let gfst = 
                query {
                    for m in mg do
                    sortBy (m.``Date Time``) 
                    select m
                } |> List.ofSeq
            let gsnd = gfst.Skip(1) |> List.ofSeq
            let paired = Seq.zip gfst gsnd |> List.ofSeq
            select paired
        }

    let last: events.Row ref = ref null
    for mmlx in pqDirections do
        let mml = mmlx

        if !last <> null then 
            wr.WriteLine(str !last)
        wr.WriteLine(new String('=', 140))
        
        let written = ref false
        for m111, m222 in mml do
            let mfst, msnd = m111, m222

            let sdiff = (msnd.``Date Time`` - mfst.``Date Time``).TotalSeconds * 1.<s>
            let isGap = sdiff > 15.<s>
            let samePeak = (msnd.``At Peak`` = mfst.``At Peak``)

            if msnd.Aspects <> mfst.Aspects then 
                wr.WriteLine(new String('-', 140))
                wr.WriteLine(str msnd)
            if not !written then
                wr.WriteLine(str msnd)
                written := true
            if not samePeak then
                wr.WriteLine(str mfst)
                wr.WriteLine(str msnd)
            if isGap then
                wr.WriteLine(String.Format("{0} <-", str mfst))
                wr.WriteLine(str msnd)
                written := false
            
            last := msnd
            ()

        ()

    wr.WriteLine("""
    </table>
</body>
</html>    
    """)
    wr.Flush()
*)

let generateSample() = 
    use reader = new StreamReader("""C:\Users\dc0d\Desktop\sd.csv""")
    use writer = new StreamWriter("""C:\Users\dc0d\Desktop\sample.csv""")
    
    let flag = ref true
    let counter = ref 0

    while !flag do
        counter := 1 + !counter
        let l = reader.ReadLine()
        if l = null || !counter > 2500 then
            flag := false
        elif not (String.IsNullOrWhiteSpace(l)) then
            writer.WriteLine(l)

    ()

[<EntryPoint>]
let main argv = 
    extractAspects()
    //extractMansions()
    //extractSigns()
    //extractDirections()
    0
