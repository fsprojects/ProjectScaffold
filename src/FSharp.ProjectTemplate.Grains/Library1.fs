namespace FSharp.ProjectTemplate.Grains

open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Interfaces
open System
open System.Threading.Tasks
open Orleans

type Grain1() = 
    inherit Orleans.Grain()

    interface IHello with

        override this.SayHello(greeting:string) =
            Task.FromResult<string>("This comes from F#!")