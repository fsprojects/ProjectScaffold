namespace FSharp.ProjectTemplate.DbTests

open NUnit.Framework
open Serilog

[<SetUpFixture>]
type SetupTest() =
    [<SetUp>]
    let ``start logging`` =
        Log.Logger <- LoggerConfiguration()
            .Destructure.FSharpTypes()
            .MinimumLevel.Debug() //uncomment to see the full debug in the console
            .WriteTo.ColoredConsole()
            .CreateLogger()
        Log.Information( "Tests started" )

module NMemory =

    open FSharp.ProjectTemplate
    open FSharp.ProjectTemplate.Domain
    open FSharp.ProjectTemplate.NMemory
    open System

    [<Test>]
    let ``simple NMemory database crud is working`` () =
      let db = Impl.Database()
      db.Persons.Insert( {Id=int64 1;FirstName="John";LastName="Rambo";LastSeen=DateTime.Now} )
      Assert.AreEqual( 1, db.Persons.Count )

      
