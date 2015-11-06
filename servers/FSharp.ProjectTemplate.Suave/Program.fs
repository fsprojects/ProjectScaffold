module App =
    open FSharp.ProjectTemplate.Domain
    open Suave
    open Suave.Http
    open Suave.Http.Applicatives
    open Suave.Http.Successful
    open Suave.Web
    open Suave.Types
    open RestFul
    open System
    open System.Reflection
    open Orleankka.Playground
    open FSharp.ProjectTemplate.Actors
    open Orleankka.FSharp

    [<EntryPoint>]
    let main argv = 
        use system = Orleankka.ActorSystem.Configure()
                                .Playground()
                                .Register(Assembly.GetExecutingAssembly())
                                .Done()

        let createGreeting (person: Person) =
            let greeter = system.ActorOf(typeof<Greeter>, "")
            let job() = task {
                do! greeter <! Greet(person)
            }
            Task.run(job) |> ignore
            "Ahoj"

        let app = 
          choose
            [ 
              POST >>= choose
                [ path "/hello" >>= choose [ 
                    request (getResourceFromReq >> createGreeting >> JSON)  ] ] ]


        startWebServer defaultConfig app
        0
