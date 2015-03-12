module App

open System
open System.Text
open System.IO
open System.Reflection
open Sweph

let mapType x =
    match x with
    | t when t = typeof<Double> -> "float64"
    | t when t = typeof<Int32> -> "int32"
    | t when t = typeof<Int64> -> "int64"
    | t when t = typeof<StringBuilder> || t = typeof<String> -> "*string"
    | t when t.IsByRef && t.GetElementType() = typeof<Int32> -> "*int32"
    | t when t.IsByRef && t.GetElementType() = typeof<Int64> -> "*int64"
    | t when t.IsByRef && t.GetElementType() = typeof<Double> -> "*float64"
    | t when t.IsArray && t.GetElementType() = typeof<Double> -> "*[6]float64"
    | t -> t.Name

let mapCPattern x =
    match x with
    | t when t = typeof<Double> -> "{0} := C.double({1})"
    | t when t = typeof<Int32> -> "{0} := C.int32({1})"
    | t when t = typeof<Int64> -> "{0} := C.int64({1})"
    | t when t = typeof<StringBuilder> || t = typeof<String> -> "var {0} *C.char = C.CString(*{1})\r\n\tdefer C.free(unsafe.Pointer({0}))"
    | t when t.IsByRef && t.GetElementType() = typeof<Int32> -> "{0} := C.int32(*{1})"
    | t when t.IsByRef && t.GetElementType() = typeof<Int64> -> "{0} := C.int64(*{1})"
    | t when t.IsByRef && t.GetElementType() = typeof<Double> -> "{0} := C.double(*{1})"
    | t when t.IsArray && t.GetElementType() = typeof<Double> -> "{0} := (*C.double)(&{1}[0])"
    | t -> t.Name 

let mapCType x =
    match x with
    | t when t = typeof<Double> -> "C.double"
    | t when t = typeof<Int32> -> "C.int32"
    | t when t = typeof<Int64> -> "C.int64"
    | t when t = typeof<StringBuilder> || t = typeof<String> -> "*C.char"
    | t when t.IsByRef && t.GetElementType() = typeof<Int32> -> "*C.int32"
    | t when t.IsByRef && t.GetElementType() = typeof<Int64> -> "*C.int64"
    | t when t.IsByRef && t.GetElementType() = typeof<Double> -> "*C.double"
    | t when t.IsArray && t.GetElementType() = typeof<Double> -> "*C.double"
    | t -> t.Name 

[<EntryPoint>]
let main argv = 
    let __dk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let __fn = "sample-go.txt"
    let fp = Path.Combine(__dk, __fn)
    use wr = new StreamWriter(fp, false) 

    let root = typedefof<Sweph>
    let methods = root.GetMethods(BindingFlags.Public ||| BindingFlags.Static)

    let exclude = [] // ["swe_gauquelin_sector";"swe_house_pos";"swe_houses";"swe_houses_armc";"swe_houses_ex";"swe_rise_trans";"swe_rise_trans_true_hor"]
    
    for m in methods do
        let ok =
            match List.tryFind (fun xxp -> xxp = m.Name) exclude with
            | None -> true
            | _ -> false
        if ok then
            
            wr.Write("func {0}(", m.Name)

            let parameters = m.GetParameters()
            let mutable psl: string list = []
            for p in parameters do
                let sb = String.Format("{0} {1}", p.Name, mapType p.ParameterType)
                psl <- List.append psl [ sb ]
                ()
            wr.Write(String.Join(", ", List.toArray psl))
            wr.Write(") ")

            if m.ReturnType <> typeof<Void> then
                wr.Write(mapType m.ReturnType)
                ()
            wr.Write(" {")
            wr.Write("\r\n")
            for p in parameters do
                wr.Write("\t")
                wr.Write(mapCPattern p.ParameterType, "_" + p.Name, p.Name)
                wr.Write("\r\n")
                ()
            wr.Write("\r\n")
            wr.Write("\t")
            if m.ReturnType <> typeof<Void> then
                wr.Write("result := {0}(", mapType m.ReturnType)
            wr.Write("C.{0}", m.Name)
            psl <- []
            for p in parameters do
                if p.ParameterType.IsByRef then
                    let sbt = mapCType p.ParameterType
                    let sb = String.Format("({0})(&_{1})", sbt, p.Name)
                    psl <- List.append psl [ sb ]
                    ()
                else
                    let sb = String.Format("_{0}", p.Name)
                    psl <- List.append psl [ sb ]
            wr.Write("({0})", String.Join(", ", List.toArray psl))
            if m.ReturnType <> typeof<Void> then
                wr.Write(")")
            wr.Write("\r\n")
            for p in parameters do
                if p.ParameterType = typeof<StringBuilder> then
                    wr.Write("\t*{0} = string(C.GoString(_{0}))", p.Name)
                    wr.Write("\r\n")
                elif p.ParameterType.IsByRef then
                    wr.Write("\t*{0} = {1}(*_{0})", p.Name, (mapType p.ParameterType).Replace("*", ""))
                    wr.Write("\r\n")
            //wr.Write("\r\n")
            if m.ReturnType <> typeof<Void> then
                wr.Write("\treturn result\r\n")
            wr.WriteLine("}")
            wr.Write("\r\n")
            ()
        ()

    wr.Flush()
    0 
