namespace FSharp.ProjectScaffold.Tests

open Expecto
open FsCheck

module Tests =
    let config10k = { FsCheckConfig.defaultConfig with maxTest = 10000}

    [<Tests>]
    let testSimpleTests =

        testList "DomainTypes.Tag" [
            testCase "equality" <| fun () ->
                let result = Library.hello 42
                Expect.isTrue (result = 42) "Expected True"
        ]

