open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Domain
open Serilog
open System.Runtime.Serialization.Formatters.Binary
open System.IO

[<EntryPoint>]
let main argv = 
    Log.Logger <- LoggerConfiguration()
        .Destructure.FSharpTypes()
        .WriteTo.Console()
        .CreateLogger()
    Log.Information( "Console application started" )

    printfn "%A" argv
    printfn "%A" (Library.hello {FirstName="John";LastName="Rambo"})
    0 // return an integer exit code
