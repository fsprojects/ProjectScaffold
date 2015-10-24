open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Domain

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    printfn "%A" (Library.hello {FirstName="John";LastName="Rambo"})
    0 // return an integer exit code
