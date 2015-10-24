namespace FSharp.ProjectTemplate

open FSharp.ProjectTemplate.Domain

/// Documentation for my library
///
/// ## Example
///
///     let h = Library.hello {"John";"Rambo"}
///     printfn "%s" h
///
module Library = 
  
  /// Returns Hello firstName lastName
  ///
  /// ## Parameters
  ///  - `person` - someone you would like to say hello to
  let hello (person : Person) = sprintf "Hello %s %s" person.FirstName person.LastName
