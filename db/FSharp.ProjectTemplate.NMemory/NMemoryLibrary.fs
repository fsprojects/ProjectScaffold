namespace FSharp.ProjectTemplate.NMemory

open NMemory
open NMemory.Tables
open FSharp.ProjectTemplate.Domain

type MyDatabase() = 
    inherit Database()

    let mutable myProp : ITable<Person> = null

    member this.Persons 
        with get() = myProp 
        and private set(value) =  myProp <- value          
