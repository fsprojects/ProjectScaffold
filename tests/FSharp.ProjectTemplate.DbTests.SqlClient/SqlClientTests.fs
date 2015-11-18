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

module SqlClient =

    open FSharp.ProjectTemplate
    open FSharp.ProjectTemplate.Domain
    open FSharp.ProjectTemplate.SqlClient
    open System
    open FSharp.Data

    type private EnsureDatabaseExistsCommand = SqlCommandProvider<"IF db_id('ProjectTemplate.Tests') IS NULL CREATE DATABASE [ProjectTemplate.Tests]", FSharp.ProjectTemplate.SqlClient.Impl.ConnectionString>

    [<Test>]
    let ``Ensure database exists`` () = 
      (new EnsureDatabaseExistsCommand()).Execute() |> ignore
      Impl.createSchema ()

    [<Test>]
    let ``simple SqlClient database crud is working`` () =
      let p = {FirstName="John";LastName="Rambo"}
      Impl.SavePersonLastSeen( p )
      let lastSeen = Impl.LoadPersonLastSeen( p ) |> Async.RunSynchronously
      Assert.LessOrEqual( DateTime.Now - lastSeen.Value, TimeSpan.FromSeconds(float 1) )
      
