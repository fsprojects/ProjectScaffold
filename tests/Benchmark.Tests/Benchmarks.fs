namespace Benchmark.Tests

open Expecto
open System

module Benchmarks =
    let inline repeat10 f a =
        let mutable v = f a
        v <- f a
        v <- f a
        v <- f a
        v <- f a
        v <- f a
        v <- f a
        v <- f a
        v <- f a
        v <- f a
        v

    let inline repeat100 f a = repeat10 (repeat10 f) a
    let inline repeat1000 f a = repeat10 (repeat100 f) a
    let inline repeat10000 f a = repeat10 (repeat1000 f) a
    let inline repeat100000 f a = repeat10 (repeat10000 f) a
    let inline repeat1000000 f a = repeat10 (repeat100000 f) a
    
    [<Tests>]
    let benchmarkSample =
        testList "sample benchmarks" [
            test "sample" {
            let mapArray (xs : string[]) =
                xs
                |> Array.map ( fun x -> sprintf "asdfsdf %s" x )

            let mapArray() = mapArray [|"asdf"; "asfdg"; "sdg"; "dfgh"; "fgjk"|]
            Expect.isFasterThan
                (mapArray >> ignore |> repeat10000)
                (mapArray >> ignore |> repeat1000000)
                "repeat10000 is faster than repeat1000000" }
        ]
