namespace FSharp.ProjectTemplate.NMemory

open NMemory
open NMemory.Tables
open FSharp.ProjectTemplate.Domain
open System.Linq.Expressions

type Expr = 
  static member Quote<'A, 'B>(e:Expression<System.Func<'A, 'B>>) = e

type MyDatabase() = 
    inherit Database()

    let mutable persons : ITable<Person> = null

    do
        persons <- base.Tables.Create<Person, string>( Expr.Quote<Person, string>( fun p -> p.FirstName+p.LastName ), IdentitySpecification<Person>(Expr.Quote<Person, int64>( fun p -> int64 (p.GetHashCode()) )) )

    member this.Persons 
        with get() = persons 
        and private set(value) =  persons <- value         
    
     
