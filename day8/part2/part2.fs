open System
open System.IO
open System.Text.RegularExpressions

module Part1 =

    type Node = { label: string; childLeft: string; childRight: string; }

    let constructNodes (lines: string list) =
        let rec constructNodes' (lines: string list) (nodes: Node list) =
            match lines with
            | [] -> nodes
            | line :: rest ->
                let lineParts = line.Split(" = ")
                let label = lineParts.[0]
                let children = lineParts.[1].Replace("(","").Replace(")","").Split(", ")
                let childLeft = children.[0]
                let childRight = children.[1]
                let node = { label = label; childLeft = childLeft; childRight = childRight; }
                constructNodes' rest (nodes @ [node])
        constructNodes' lines []

    let findNode (label: string) (nodes: Node list) =
        let rec findNode' (label: string) (nodes: Node list) (node: Node option) =
            match nodes with
            | [] -> None
            | n :: rest ->
                if n.label = label then
                    Some(n)
                else
                    findNode' label rest node
        findNode' label nodes None

    let findStartNodes (nodes: Node list) =
        let rec findStartNodes' (nodes: Node list) (startNodes: Node list) =
            match nodes with
            | [] -> startNodes
            | n :: rest ->
                if n.label.EndsWith("A") then
                    findStartNodes' rest (startNodes @ [n])
                else
                    findStartNodes' rest startNodes
        findStartNodes' nodes []

    let hauntTree (nodes: Node list) (instructions: char list) =
        let rec hauntTree' (nodes: Node list) (instructions: char list) (currentNode: Node) (steps: int) =
            match currentNode.label.[2] with
            | 'Z' -> steps
            | _ ->
                let instruction = instructions.[0]
                let nextNode = 
                    match instruction with
                    | 'L' -> findNode currentNode.childLeft nodes
                    | 'R' -> findNode currentNode.childRight nodes
                    | _ -> failwith "Invalid instruction."
                match nextNode with
                | None -> failwith "Invalid instruction."
                | Some(n) -> 
                    let restInstructions = instructions.Tail @ [instruction]
                    hauntTree' nodes restInstructions n (steps + 1)
        let startNodes = findStartNodes nodes
        let mutable stepsList = []
        for startNode in startNodes do
            let steps = hauntTree' nodes instructions startNode 0
            stepsList <- stepsList @ [steps]
        stepsList

    // get the greatest common divisor of two integers
    let rec gcd (a: int) (b: int) =
        if b = 0 then
            a
        else
            gcd b (a % b)

    // get the least common multiple of a list of integers
    let getLCM (numbers: int list) =
        let rec getLCM' (numbers: int list) (lcm: int) =
            match numbers with
            | [] -> lcm
            | n :: rest ->
                let newLcm = lcm * n / (gcd lcm n)
                getLCM' rest newLcm
        getLCM' numbers 1

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        let instructions = List.ofSeq lines.[0]
        //printfn "%A" instructions
        let nodes = constructNodes (lines.Tail.Tail)
        //printfn "%A" nodes
        let spookySteps = hauntTree nodes instructions
        //printfn "%A" spookySteps
        getLCM spookySteps

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let steps = solve (argv.[0])
        printfn "Steps: %d" steps
        0
