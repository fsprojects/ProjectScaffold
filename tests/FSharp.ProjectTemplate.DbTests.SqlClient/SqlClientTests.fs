namespace FSharp.ProjectTemplate.DbTests

module SqlClient =

    open FSharp.ProjectTemplate
    open NUnit.Framework
    open FSharp.ProjectTemplate.Domain
    open Serilog
    open FSharp.ProjectTemplate.SqlClient
    open System
    open FSharp.Data

    Log.Logger <- LoggerConfiguration()
        .Destructure.FSharpTypes()
        .WriteTo.Console()
        .CreateLogger()
    Log.Information( "Tests started" )

    type private EnsureDatabaseExistsCommand = SqlCommandProvider<"IF db_id('ProjectTemplate.Tests') IS NULL CREATE DATABASE [ProjectTemplate.Tests]", FSharp.ProjectTemplate.SqlClient.Impl.ConnectionString>

    [<Test>]
    let ``Ensure database exists`` () = 
      (new EnsureDatabaseExistsCommand()).Execute() |> ignore

    [<Test>]
    let ``simple SqlClient database crud is working`` () =
      let p = {FirstName="John";LastName="Rambo"}
      Impl.SavePersonLastSeen( p )
      let lastSeen = Impl.LoadPersonLastSeen( p ) |> Async.RunSynchronously
      Assert.AreEqual( DateTime.Now, lastSeen.Value )

      
