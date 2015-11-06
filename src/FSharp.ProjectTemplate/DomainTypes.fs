namespace FSharp.ProjectTemplate.Domain

open System
open System.Runtime.Serialization

[<CLIMutable>]
type Person = {
    FirstName:string
    LastName:string
}
