module App =
    open Suave                 // always open suave
    open Suave.Http.Successful // for OK-result
    open Suave.Web             // for config
    open RestFul
    open FSharp.ProjectTemplate.Domain

    let createGreeting person =
        let greet : Person = person
        "Ahoj"

    [<EntryPoint>]
    let main argv = 
        let personWebPart = rest "greetings" {
            GetAll = ignore
            GetById = ignore
            IsExists = ignore
            Update = ignore
            UpdateById = ignore
            Delete = ignore
            Create = createGreeting
        }

        let app = choose[personWebPart;albumWebPart]                    

        startWebServer defaultConfig app
