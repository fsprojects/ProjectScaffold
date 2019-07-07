[<AutoOpen>]
module ViewHelpers

open Fable.React
open Fable.React.Props

open Fulma

module KeyCode =
    let enter = 13.
    let upArrow = 38.
    let downArrow =  40.

let onKeyDown keyCode action =
    OnKeyDown (fun (ev: Browser.Types.KeyboardEvent) ->
        if ev.keyCode = keyCode then
            ev.preventDefault()
            action ev)

let btn txt onClick = 
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ] 
        [ str txt ]

let lbl txt = Label.label [] [ str txt ]

let intersperse sep ls =
    List.foldBack (fun x -> function
        | [] -> [x]
        | xs -> x::sep::xs) ls []

let safeComponents =
    let components =
        [ "Saturn", "https://saturnframework.github.io/docs/"
          "Fable", "http://fable.io"
          "Elmish", "https://fable-elmish.github.io/"
          "Fulma", "https://mangelmaxime.github.io/Fulma" ]
        |> List.map (fun (desc,link) -> a [ Href link ] [ str desc ] )
        |> intersperse (str ", ")
        |> span []

    p []
        [ strong [] [ a [ Href "https://safe-stack.github.io/" ] [ str "SAFE Template" ] ]
          str " powered by: "
          components ]

module Result =
    let defaultValue v r = match r with Ok x -> x | Error _ -> v