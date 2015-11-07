namespace FSharp.ProjectTemplate.DbTests

module NMemory =

    open FSharp.ProjectTemplate
    open NUnit.Framework
    open FSharp.ProjectTemplate.Domain
    open Serilog
    open FSharp.ProjectTemplate.NMemory
    open System

    Log.Logger <- LoggerConfiguration()
        .Destructure.FSharpTypes()
        .WriteTo.Console()
        .CreateLogger()
    Log.Information( "Tests started" )

    [<Test>]
    let ``simple NMemory database crud is working`` () =
      let db = Impl.MyDatabase()
      db.Persons.Insert( {Id=int64 1;FirstName="John";LastName="Rambo";LastSeen=DateTime.Now} )
      Assert.AreEqual( 1, db.Persons.Count )

      
