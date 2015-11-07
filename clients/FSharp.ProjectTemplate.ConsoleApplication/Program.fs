open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Domain
open Serilog
open System.Runtime.Serialization.Formatters.Binary
open System.IO
open System

[<EntryPoint>]
let main argv = 
    Log.Logger <- LoggerConfiguration()
        .Destructure.FSharpTypes()
        .WriteTo.Console()
        .CreateLogger()
    Log.Information( "Console application started" )

    printfn "%A" argv
    printfn "%A" (Library.api(Library.LoadFake, Library.SaveFake).Hello {FirstName="John";LastName="Rambo"})
    0 // return an integer exit code
