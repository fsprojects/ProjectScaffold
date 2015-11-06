open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Domain
open Serilog
open System.Runtime.Serialization.Formatters.Binary
open System.IO

let serializeBinary<'a> (x :'a) =
    let binFormatter = new BinaryFormatter()

    use stream = new MemoryStream()
    binFormatter.Serialize(stream, x)
    stream.ToArray()

let deserializeBinary<'a> (arr : byte[]) =
    let binFormatter = new BinaryFormatter()

    use stream = new MemoryStream(arr)
    binFormatter.Deserialize(stream) :?> 'a


[<EntryPoint>]
let main argv = 
    Log.Logger <- LoggerConfiguration()
        .Destructure.FSharpTypes()
        .WriteTo.Console()
        .CreateLogger()
    Log.Information( "Console application started" )

    printfn "%A" ( deserializeBinary<Person>( serializeBinary({FirstName="John";LastName="Rambo"}) ) )

    printfn "%A" argv
    printfn "%A" (Library.hello {FirstName="John";LastName="Rambo"})
    0 // return an integer exit code
