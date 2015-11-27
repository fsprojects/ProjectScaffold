namespace FSharp.ProjectTemplate

open FSharp.ProjectTemplate.Domain
open Serilog
open System

/// Documentation for my library
///
/// ## Example
///
///     let h = Library.hello {"John";"Rambo"}
///     printfn "%s" h
///
module Library = 
  
    Log.Information( "Library FSharp.ProjectTemplate loaded" )

    // the support methods
    type SaveLastHello =
        Person -> unit
    type LoadLastHello =
        Person -> DateTime option

    /// Returns Hello firstName lastName, I saw you for the last time on 1.1.1970
    ///
    /// ## Parameters
    ///  - `person` - someone you would like to say hello to
    let hello (loadLastHello:LoadLastHello, saveLastHello:SaveLastHello) (person : Person) = 
        let lastHello = loadLastHello(person)
        let result = 
            match lastHello with
            | Some lastHelloTime -> sprintf "Hello %s %s, I saw you for the last time on %O" person.FirstName person.LastName lastHelloTime
            | None -> sprintf "Hello %s %s" person.FirstName person.LastName
        saveLastHello (person)
        result

    let SaveFake (p : Person) = 0 |> ignore
    let LoadFake (p : Person) = None

    let api (loadLastHello:LoadLastHello, saveLastHello:SaveLastHello) = {
        Hello = hello (loadLastHello, saveLastHello)
    }

/// Interface to be implemented by persistent layer and combined together in the client or server if needed
///
///
type public IHelloPersistency =
   abstract member Load: Person -> DateTime option
   abstract member Save :  Person -> unit
