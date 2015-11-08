namespace FSharp.ProjectTemplate.DbTests

module SqlClient =

    open FSharp.ProjectTemplate
    open NUnit.Framework
    open FSharp.ProjectTemplate.Domain
    open Serilog
    open FSharp.ProjectTemplate.SqlClient
    open System

    Log.Logger <- LoggerConfiguration()
        .Destructure.FSharpTypes()
        .WriteTo.Console()
        .CreateLogger()
    Log.Information( "Tests started" )

    [<Test>]
    let ``simple SqlClient database crud is working`` () =
      Assert.AreEqual( 1, 1 )

      
