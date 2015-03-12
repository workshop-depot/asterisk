open System.IO
open System

let showHead (fp: String) (n: int) =
    use reader = new StreamReader(fp)
    for i = 1 to n do
        Console.WriteLine(reader.ReadLine())
    ()

// showHead """C:\Workshop-Temporal\aster\sd.csv""" 10