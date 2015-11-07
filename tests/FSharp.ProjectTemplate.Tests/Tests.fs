module FSharp.ProjectScaffold.Tests

open FSharp.ProjectTemplate
open NUnit.Framework
open FSharp.ProjectTemplate.Domain
open Serilog

Log.Logger <- LoggerConfiguration()
    .Destructure.FSharpTypes()
    .WriteTo.Console()
    .CreateLogger()
Log.Information( "Tests started" )

[<Test>]
let ``hello returns "Hello John Rambo" for {FirstName="John";LastName="Rambo"}`` () =
  let result = Library.api(Library.LoadFake, Library.SaveFake).Hello {FirstName="John";LastName="Rambo"}
  Assert.AreEqual("Hello John Rambo",result)