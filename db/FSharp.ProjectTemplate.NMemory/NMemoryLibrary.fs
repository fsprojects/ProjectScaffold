namespace FSharp.ProjectTemplate.NMemory

open NMemory
open NMemory.Tables
open FSharp.ProjectTemplate.Domain
open System.Linq.Expressions
open System.Linq
open System

module Impl =
    type Expr = 
      static member Quote<'A, 'B>(e:Expression<System.Func<'A, 'B>>) = e

    [<CLIMutable>]
    type PersonDTO = {
        Id : int64
        FirstName:string
        LastName:string
        LastSeen:DateTime
    }

    type DatabaseStructure() = 
        inherit Database()

        let mutable persons : ITable<PersonDTO> = null

        do
            persons <- base.Tables.Create<PersonDTO, int64>( Expr.Quote<PersonDTO, int64>( fun p -> p.Id ), IdentitySpecification<PersonDTO>(Expr.Quote<PersonDTO, int64>( fun p -> p.Id )) )

        member this.Persons 
            with get() = persons 
            and private set(value) =  persons <- value      

    let find (db:DatabaseStructure) ( p : Person ) =
        query {
            for person in db.Persons do
            where (person.FirstName = p.FirstName && person.LastName = p.LastName)
            select person
        }

    let LoadPersonLastSeen (db:DatabaseStructure) ( p : Person ) =
        let found = find db p
        if found.Count() > 0 then
            Some(found.First().LastSeen)
        else
            None

    let SavePersonLastSeen (db:DatabaseStructure) ( p : Person ) =
        let found = find db p
        if found.Count() > 0 then
            db.Persons.Delete( found.First() )
        db.Persons.Insert( { Id = DateTime.Now.Ticks; FirstName = p.FirstName; LastName = p.LastName; LastSeen = DateTime.Now } )

    type Database() = 
        inherit DatabaseStructure()

        interface FSharp.ProjectTemplate.IHelloPersistency with
            member this.Load(p:Person) = LoadPersonLastSeen this p
            member this.Save(p:Person) = SavePersonLastSeen this p

