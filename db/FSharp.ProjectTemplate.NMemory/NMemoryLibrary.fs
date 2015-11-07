namespace FSharp.ProjectTemplate.NMemory

open NMemory
open NMemory.Tables
open FSharp.ProjectTemplate.Domain
open System.Linq.Expressions

type Expr = 
  static member Quote<'A, 'B>(e:Expression<System.Func<'A, 'B>>) = e

[<CLIMutable>]
type PersonDTO = {
    Id : int64
    FirstName:string
    LastName:string
}

type MyDatabase() = 
    inherit Database()

    let mutable persons : ITable<PersonDTO> = null

    do
        persons <- base.Tables.Create<PersonDTO, int64>( Expr.Quote<PersonDTO, int64>( fun p -> p.Id ), IdentitySpecification<PersonDTO>(Expr.Quote<PersonDTO, int64>( fun p -> p.Id )) )

    member this.Persons 
        with get() = persons 
        and private set(value) =  persons <- value         
    
     
