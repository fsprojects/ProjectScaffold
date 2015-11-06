namespace FSharp.ProjectTemplate.Domain

open System
open Newtonsoft.Json
open System.Runtime.Serialization

[<DataContract>]
type Person = {
    [<field : DataMember>]
    FirstName:string
    [<field : DataMember>]
    LastName:string
}
