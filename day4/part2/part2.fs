open System
open System.IO
open System.Text.RegularExpressions

module Part1 =

    type Card = { id: int; winning: int list; numbers: int list; }
    type Multiplier = { multiplier: int; cardCount: int;}

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

    let getMatches (cards: Card list) =
        let rec getMatches' (cards: Card list) (matches: int list) =
            match cards with
            | [] -> matches
            | card :: rest ->
                let cardMatches = findMatches card.winning card.numbers
                getMatches' rest (matches @ [cardMatches.Length])
        getMatches' cards []

    let multiplierCards (multipliers: Multiplier list) =
        let rec multiplierCards' (multipliers: Multiplier list) (cards: int) =
            match multipliers with
            | [] -> cards
            | multiplier :: rest ->
                multiplierCards' rest (cards + multiplier.cardCount)
        multiplierCards' multipliers 0

    let reduceMultipliers (multipliers: Multiplier list) =
        let rec reduceMultipliers' (multipliers: Multiplier list) (reduced: Multiplier list) =
            match multipliers with
            | [] -> reduced
            | multiplier :: rest ->
                if multiplier.multiplier > 1 then
                    reduceMultipliers' rest (reduced @ [{ multiplier = multiplier.multiplier - 1; cardCount = multiplier.cardCount }])
                else
                    reduceMultipliers' rest reduced
        reduceMultipliers' multipliers []

    let countCards (matches: int list) =
        let rec countCards' (matches: int list) (multipliers: Multiplier list)(sum: int) =
            match matches with
            | [] -> sum
            | matchCount :: rest ->
                //printfn "Mult: %A" multipliers
                let numCards = 1 + multiplierCards multipliers
                //printfn "Num: %d" numCards
                let mutable newMultipliers = reduceMultipliers multipliers
                if matchCount > 0 then
                    newMultipliers <- newMultipliers @ [{ multiplier = matchCount; cardCount = numCards }]
                countCards' rest newMultipliers (sum + numCards)
        countCards' matches [] 0

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        let cards = splitCards lines
        let matches = getMatches cards
        countCards matches

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let sum = solve (argv.[0])
        printfn "Total Cards: %d" sum
        0
