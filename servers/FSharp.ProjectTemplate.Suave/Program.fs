module App =
    open FSharp.ProjectTemplate.Domain
    open Suave
    open Suave.Http
    open Suave.Http.Applicatives
    open Suave.Http.Successful
    open Suave.Web
    open Suave.Types
    open RestFul

    let createGreeting person =
        let greet : Person = person
        "Ahoj"

    [<EntryPoint>]
    let main argv = 
        let app = 
          choose
            [ 
              POST >>= choose
                [ path "/hello" >>= choose [ 
                    request (getResourceFromReq >> createGreeting >> JSON)  ] ] ]

        startWebServer defaultConfig app
        0
