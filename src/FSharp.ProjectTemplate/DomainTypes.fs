namespace FSharp.ProjectTemplate

module Domain =

    [<CLIMutable>]
    type Person = {
        FirstName:string
        LastName:string
    }

    type HelloMessage = 
       | Greet of Person
       | Hi
    
    // the "use-cases"
    type Hello = 
        Person -> string

    // the functions exported from the implementation
    // for the clients and servers to use.
    type API = {
        Hello : Hello
    }
