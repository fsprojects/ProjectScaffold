namespace FSharp.ProjectTemplate.NMemory

open NMemory
open NMemory.Tables
open FSharp.ProjectTemplate.Domain

type MyDatabase() = 
    inherit Database()

    let mutable persons : ITable<Person> = null

    do
        persons <- base.Tables.Create<Person, string>(

    member this.Persons 
        with get() = persons 
        and private set(value) =  persons <- value         
    
     
