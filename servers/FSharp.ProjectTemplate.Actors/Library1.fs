namespace FSharp.ProjectTemplate.Actors

open Orleankka
open Orleankka.FSharp
open Orleankka.Playground
open FSharp.ProjectTemplate
open FSharp.ProjectTemplate.Domain

type Greeter() = 
    inherit Actor<Domain.HelloMessage>()

    override this.Receive message reply = task {
        match message with
        | Greet who -> reply (Library.hello who)
        | Hi -> reply "Hello from F#!"
    }
