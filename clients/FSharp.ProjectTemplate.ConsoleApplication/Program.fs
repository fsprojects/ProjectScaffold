open FSharp.ProjectTemplate

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    printfn "%A" (Library.hello 1)
    0 // return an integer exit code
