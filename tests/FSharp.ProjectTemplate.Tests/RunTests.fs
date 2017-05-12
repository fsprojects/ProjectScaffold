namespace FSharp.ProjectScaffold.Tests

open Expecto

module Tests =

    [<EntryPoint>]
    let main args =

        Tests.runTestsWithArgs defaultConfig args Tests.testSimpleTests |> ignore

        0

