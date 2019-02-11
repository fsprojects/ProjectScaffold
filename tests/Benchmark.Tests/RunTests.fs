namespace Benchmark.Tests

open Expecto

module RunTests =

    [<EntryPoint>]
    let main args =
        [
            Tests.runTestsWithArgs defaultConfig args Benchmarks.benchmarkSample
        ] 
        |> List.sum
