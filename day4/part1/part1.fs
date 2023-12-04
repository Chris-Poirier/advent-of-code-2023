open System
open System.IO
open System.Text.RegularExpressions

module Part1 =

    type Card = { id: int; winning: int list; numbers: int list; }

    let extractId (line: string) =
        let regex = new Regex("Card +(\\d+)")
        let matches = regex.Match(line)
        if not matches.Success then
            failwith "Invalid card line format."
        Int32.Parse(matches.Groups.[1].Value)

    let number_string_to_int_list (numbers: string) = 
        let numbers_normal = Regex.Replace(numbers, "^ ", "0").Replace("  ", " 0")
        List.ofArray(numbers_normal.Split(" ") |> Array.map (fun s -> s.Trim()) |> Array.map int)

    let splitCards (lines: string list) =
        let rec splitCards' (lines: string list) (cards: Card list) =
            match lines with
            | [] -> cards
            | line :: rest ->
                let lineParts = line.Split(": ")
                let id = extractId(lineParts.[0])
                let allNumbers = lineParts.[1].Split(" | ")
                let winning = number_string_to_int_list(allNumbers.[0])
                let numbers = number_string_to_int_list(allNumbers.[1])
                let card = { id = id; winning = winning; numbers = numbers; }
                splitCards' rest (cards @ [card])
        splitCards' lines []

    let findMatches (winning: int list) (numbers: int list) =
        let rec findMatches' (winning: int list) (numbers: int list) (matches: int list) =
            match winning with
            | [] -> matches
            | winningNumber :: rest ->
                let matchCount = numbers |> List.filter (fun n -> n = winningNumber) |> List.length
                if matchCount > 0 then
                    findMatches' rest numbers (matches @ [winningNumber])
                else
                    findMatches' rest numbers matches
        findMatches' winning numbers []

    let getPoints (cards: Card list) =
        let rec getPoints' (cards: Card list) (points: int list) =
            match cards with
            | [] -> points
            | card :: rest ->
                let cardMatches = findMatches card.winning card.numbers
                let cardPoints = Math.Pow(2.0, float(cardMatches.Length - 1)) |> int
                getPoints' rest (points @ [cardPoints])
        getPoints' cards []

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        let cards = splitCards lines
        let points = getPoints cards
        List.sum points

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let sum = solve (argv.[0])
        printfn "Points: %d" sum
        0
