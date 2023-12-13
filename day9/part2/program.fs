open System
open System.IO
open System.Text.RegularExpressions

module Part2 =

    let rec predictPrev (numbers: int list) =
        if numbers |> List.forall (fun n -> n = 0) then
            0
        else
            let differences = numbers |> List.pairwise |> List.map (fun (a, b) -> b - a)
            let firstNum = numbers.[0]
            firstNum - (predictPrev differences)

    let predictPrevAll (lines: string list) =
        let rec predictPrevAll' (lines: string list) (predictions: int list) =
            match lines with
            | [] -> predictions
            | line :: rest ->
                let lineParts = line.Split(" ")
                let numbers = List.ofArray(lineParts |> Array.map (fun s -> s.Trim()) |> Array.map int)
                let prediction = predictPrev numbers
                predictPrevAll' rest (predictions @ [prediction])
        predictPrevAll' lines []

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        //printfn "%A" lines
        let predictions = predictPrevAll lines
        //printfn "%A" predictions
        let sum = predictions |> List.sum
        sum

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let sum = solve (argv.[0])
        printfn "Sum: %d" sum
        0
