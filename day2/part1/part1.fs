open System
open System.IO
open System.Text.RegularExpressions

module Part1 =

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

    let gameStones: GameDraw = { redCount = 12; greenCount = 13; blueCount = 14; }
    let rec isPossibleGame (draws: GameDraw list) =
        match draws with
        | [] -> true
        | draw :: rest ->
            let redPossible = draw.redCount <= gameStones.redCount
            let greenPossible = draw.greenCount <= gameStones.greenCount
            let bluePossible = draw.blueCount <= gameStones.blueCount
            if redPossible && greenPossible && bluePossible then
                isPossibleGame rest
            else
                false

    let rec possibleGames (games: Game list) (possGames: Game list) =
        match games with
        | [] -> possGames
        | game :: rest ->
            let possible = isPossibleGame game.rounds
            if possible then
                possibleGames rest (game :: possGames)
            else
                possibleGames rest possGames

    let rec sumGameIds (games: Game list) =
        match games with
        | [] -> 0
        | game :: rest ->
            game.id + sumGameIds rest

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        let games = splitGames lines
        let possibleGames = possibleGames games []
        sumGameIds possibleGames

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let sum = solve (argv.[0])
        printfn "Sum: %d" sum
        0
