namespace FSharp.ProjectTemplate.Actors

open Orleankka
open Orleankka.FSharp
open Orleankka.Playground
open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Domain
open System

[<AbstractClass>]
type Greeter() = 
    inherit Actor<Domain.HelloMessage>()

    abstract member SaveLastHello : Person -> unit
    abstract member LoadLastHello : Person -> DateTime option

    override this.Receive message reply = task {
        match message with
        | Greet who -> reply (Library.api(this.LoadLastHello, this.SaveLastHello).Hello who)
        | Hi -> reply "Hello from F#!"
    }
