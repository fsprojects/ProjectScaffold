namespace FSharp.ProjectTemplate

open FSharp.ProjectTemplate.Domain
open Serilog

/// Documentation for my library
///
/// ## Example
///
///     let h = Library.hello {"John";"Rambo"}
///     printfn "%s" h
///
module Library = 
  
    Log.Information( "Library FSharp.ProjectTemplate loaded" )

    /// Returns Hello firstName lastName, I saw you for the last time on 1.1.1970
    ///
    /// ## Parameters
    ///  - `person` - someone you would like to say hello to
    let hello (person : Person) = 
        sprintf "Hello %s %s" person.FirstName person.LastName

    let api = {
        Hello = hello 
    }
