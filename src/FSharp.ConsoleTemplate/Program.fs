namespace FSharp.ProjectTemplate

open CommandLine
open System

module console1 =

    [<EntryPoint>]
    let main argv = 
        printfn "%A" argv

        let parsedCommand = parse (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name) argv

        match parsedCommand.Error with
            | Some e -> 
                printfn "%s" parsedCommand.Usage
            | None -> 
                printfn "%A" parsedCommand

        printfn "Hit any key to exit."
        System.Console.ReadKey() |> ignore
        0