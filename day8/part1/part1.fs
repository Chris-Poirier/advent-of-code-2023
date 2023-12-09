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

    let walkTree (nodes: Node list) (instructions: char list) =
        let rec walkTree' (nodes: Node list) (instructions: char list) (currentNode: Node) (steps: int) =
            match currentNode.label with
            | "ZZZ" -> steps
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
                    walkTree' nodes restInstructions n (steps + 1)
        let startNode = findNode "AAA" nodes
        match startNode with
        | None -> failwith "Start node not found."
        | Some(n) -> walkTree' nodes instructions n 0

    let solve (filename: string) =
        if not (File.Exists(filename)) then
            failwith "File does not exist."
        let lines = List.ofArray(File.ReadAllLines(filename))
        let instructions = List.ofSeq lines.[0]
        //printfn "%A" instructions
        let nodes = constructNodes (lines.Tail.Tail)
        //printfn "%A" nodes
        walkTree nodes instructions
        // -1

    [<EntryPoint>]
    let main argv =
        if argv.Length <> 1 then
            failwith "Exactly 1 argument is required."
        let steps = solve (argv.[0])
        printfn "Steps: %d" steps
        0
