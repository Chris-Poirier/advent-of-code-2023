open System
open System.IO
open System.Text.RegularExpressions

module Part2 =

    type GameDraw = { redCount: int; greenCount: int; blueCount: int;}
    type Game = { id: int; rounds: GameDraw list; }

    let extractId (line: string) =
        let regex = new Regex("Game (\\d+)")
        let matches = regex.Match(line)
        if not matches.Success then
            failwith "Invalid game line format."
        Int32.Parse(matches.Groups.[1].Value)

    let extractDraw (draw: string) (round: GameDraw) =
        let regex = new Regex("(\\d+) (red|green|blue)")
        let matches = regex.Match(draw)
        if matches.Success then
            let count = Int32.Parse(matches.Groups.[1].Value)
            let color = matches.Groups.[2].Value
            match color with
            | "red" -> { round with redCount = count }
            | "green" -> { round with greenCount = count }
            | "blue" -> { round with blueCount = count }
            | _ -> failwith "Invalid draw format."
        else
            failwith "Invalid draw format."

    let rec extractRound (draws: string list) (round: GameDraw) =
        match draws with
        | [] -> round
        | draw :: rest ->
            let draw = extractDraw draw round
            extractRound rest draw

    let rec extractRounds (rounds: string list) =
        match rounds with
        | [] -> []
        | round :: rest ->
            let roundDraws = List.ofArray(round.Split(", "))
            let roundDraw = extractRound roundDraws { redCount = 0; greenCount = 0; blueCount = 0; }
            (extractRounds rest) @ [roundDraw]
            
    let rec splitGames (lines: string list) =
        match lines with
        | [] -> []
        | line :: rest ->
            let lineParts = line.Split(": ")
            let id = extractId lineParts.[0]
            let drawRounds = extractRounds (List.ofArray(lineParts.[1].Split("; ")))
            let game = { id = id; rounds = drawRounds }
            (splitGames rest) @ [game]

    let rec getGameMaxes (game: Game) (maxes: GameDraw) =
        match game.rounds with
        | [] -> maxes
        | round :: rest ->
            let redCount = if round.redCount > maxes.redCount then round.redCount else maxes.redCount
            let greenCount = if round.greenCount > maxes.greenCount then round.greenCount else maxes.greenCount
            let blueCount = if round.blueCount > maxes.blueCount then round.blueCount else maxes.blueCount
            getGameMaxes { game with rounds = rest } { redCount = redCount; greenCount = greenCount; blueCount = blueCount; }

    let rec getPowers (games: Game list) (powers: int list) =
        match games with
        | [] -> powers
        | game :: rest ->
            let gameMaxes = getGameMaxes game { redCount = 0; greenCount = 0; blueCount = 0; }
            let power = gameMaxes.redCount * gameMaxes.greenCount * gameMaxes.blueCount
            getPowers rest (powers @ [power])

    let rec sumPowers (powers: int list) =
        match powers with
        | [] -> 0
        | power :: rest ->
            power + sumPowers rest

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        let games = splitGames lines
        let powers = getPowers games []
        // for power in powers do
        //     printfn "%d" power
        sumPowers powers

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let sum = solve (argv.[0])
        printfn "Sum: %d" sum
        0
