module FSharp.ProjectScaffold.Tests

open FSharp.ProjectTemplate
open NUnit.Framework
open FSharp.ProjectTemplate.Domain

[<Test>]
let ``hello returns "Hello "`` () =
  let result = Library.hello {FirstName="John";LastName="Rambo"}
  printfn "%s" result
  Assert.AreEqual("Hello John Rambo",result)