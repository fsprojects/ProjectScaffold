[<FunScript.JS>]
module AppCompose

open FunScript
open FunScript.TypeScript

// Allows writing jq?name for element access
let jq(selector : string) = Globals.Dollar.Invoke selector
let (?) jq name = jq("#" + name)

[<JS; JSEmit("log.info({0})")>]
let log_info (a) = failwith "never" |> ignore

[<JS; JSEmit("log.enableAll()")>]
let log_enable (b) = failwith "never" |> ignore

[<JS; JSEmit("app.showNotification('F# App Notification',{0})")>]
let hello (a) = failwith "never" |> ignore

[<JS; JSEmit("Office.cast.item.toItemCompose(Office.context.mailbox.item).subject.setAsync({0})")>]
let setSubject(a) =
    failwith "never" |> ignore

let main() = 
    log_enable ()
    log_info ( "application started" )
    jq?helloWorld.click(
        fun _ -> 
            log_info ( "button clicked" )
            hello( "Clicked!" ) :> obj
    )
