module App

open Elmish

open Fable
open Fable.FontAwesome
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Fable.Recharts
open Fable.Recharts.Props

open Thoth.Json
open Thoth.Fetch

open Fulma

open Shared

/// The different elements of the completed report.
type Report =
    { Location : LocationResponse
      Crimes : CrimeResponse array 
      Weather : WeatherResponse
      }

type ServerState = Idle | Loading | ServerError of string

/// The overall data model driving the view.
type Model =
    { Postcode : string
      ValidationError : string option
      ServerState : ServerState
      Report : Report option 
      }

/// The different types of messages in the system.
type Msg =
    | GetReport
    | PostcodeChanged of string
    | GotReport of Report
    | ErrorMsg of exn
    | Clear

/// The init function is called to start the message pump with an initial view.
let init () =
    { Postcode = null
      Report = None
      ValidationError = None
      ServerState = Idle }, Cmd.ofMsg (PostcodeChanged "")

let decoderForLatLong =
        Decode.object (fun get ->
            {
                Latitude = get.Required.Field "Latitude" Decode.float
                Longitude = get.Required.Field "Longitude" Decode.float
            }
        )

let decoderForLocation =
    Decode.object (fun get ->
        {
            Town = get.Required.Field "Town" Decode.string
            Region = get.Required.Field "Region" Decode.string
            LatLong = get.Required.Field "LatLong" decoderForLatLong
        }
    )

let decoderForLocationResponse : (string -> obj -> Result<LocationResponse, _>) =
    Decode.object (fun get ->
        {
            Postcode = get.Required.Field "Postcode" Decode.string
            Location = get.Required.Field "Location" decoderForLocation
            DistanceToLondon = get.Required.Field "DistanceToLondon" Decode.float
        }
    )

let decoderForCrimeResponse =
    Decode.object (fun get ->
        [| {
            Crime = get.Required.Field "Crime" Decode.string
            Incidents =  get.Required.Field "Incidents" Decode.int
        } |]
    )

let decoderForWeatherResponse =
    Decode.object (fun get ->
        {
            WeatherType = get.Required.Field "WeatherType" Decode.string |> WeatherType.Parse
            AverageTemperature = get.Required.Field "AverageTemperature" Decode.float
        }
    )

//let inline getJson<'T> (response: Fetch.Types.Response) = 
//    response.text() 
//    |> Promise.map  Thoth.Json.Decode.Auto.unsafeFromString<'T>

let getResponse postcode = promise {
    let! location =
        Fetch.tryPost ("/api/distance", postcode, decoderForLocationResponse)
        |> Promise.map  (Result.defaultValue {
                    Postcode = ""
                    Location = {Town = ""; Region = ""; LatLong = {Latitude = 0.; Longitude = 0.}}
                    DistanceToLondon = 0.
                }
        )
    let! crimes =
        Fetch.tryFetchAs ((sprintf "api/crime/%s" postcode), decoderForCrimeResponse)
        |> Promise.map  (Result.defaultValue [||])
    let! weather =
        Fetch.tryFetchAs ((sprintf "api/weather/%s" postcode), decoderForWeatherResponse)
        |> Promise.map (Result.defaultValue { WeatherType = WeatherType.Clear; AverageTemperature = 0. })
    
    (* Task 4.5 WEATHER: Fetch the weather from the API endpoint you created.
       Then, save its value into the Report below. You'll need to add a new
       field to the Report type first, though! *)

    return { Location = location ; Crimes = crimes; Weather = weather } }

/// The update function knows how to update the model given a message.
let update msg model =
    match model, msg with
    | { ValidationError = None; Postcode = postcode }, GetReport ->
        { model with ServerState = Loading }, Cmd.OfPromise.either getResponse postcode GotReport ErrorMsg
    | _, GetReport -> model, Cmd.none
    | _, GotReport response ->
        { model with
            ValidationError = None
            Report = Some response
            ServerState = Idle }, Cmd.none
    | _, PostcodeChanged p ->
        { model with
            Postcode = p
            (* Task 2.2 Validation. Use the Validation.validatePostcode function to implement client-side form validation.
               Note that the validation is the same shared code that runs on the server! *)
            ValidationError =
            if Validation.isValidPostcode p then
                None
            else
                Some "invalid post code"
                }, Cmd.none
    | _, ErrorMsg e -> { model with ServerState = ServerError e.Message }, Cmd.none
    | _, Clear -> init()

[<AutoOpen>]
module ViewParts =
    let basicTile title options content =
        Tile.tile options [
            Notification.notification [ Notification.Props [ Style [ Height "100%"; Width "100%" ] ] ]
                (Heading.h2 [] [ str title ] :: content)
        ]
    let childTile title content =
        Tile.child [ ] [
            Notification.notification [ Notification.Props [ Style [ Height "100%"; Width "100%" ] ] ]
                (Heading.h2 [ ] [ str title ] :: content)
        ]

    let crimeTile crimes =
        let cleanData = crimes |> Array.map (fun c -> { c with Crime = c.Crime.[0..0].ToUpper() + c.Crime.[1..].Replace('-', ' ') } )
        basicTile "Crime" [ ] [
            barChart
                [ Chart.Data cleanData
                  Chart.Width 600.
                  Chart.Height 500.
                  Chart.Layout Vertical ]
                [ xaxis [ Cartesian.Type "number" ] []
                  yaxis [ Cartesian.Type "category"; Cartesian.DataKey "Crime"; Cartesian.Width 200. ] []
                  bar [ Cartesian.DataKey "Incidents" ] [] ]
        ]

    let getBingMapUrl latLong =
        sprintf "https://www.bing.com/maps/embed?h=400&w=800&cp=%f~%f&lvl=11&typ=s&FORM=MBEDV8" latLong.Latitude latLong.Longitude

    let bingMapTile (latLong:LatLong) =
        basicTile "Map" [ Tile.Size Tile.Is12 ] [
            iframe [
                Style [ Height 410; Width 810 ]
                Src (getBingMapUrl latLong)
                (* Task 3.1 MAPS: Use the getBingMapUrl function to build a valid maps URL using the supplied LatLong.
                   You can use it to add a Src attribute to this iframe. *)
            ] [ ]
        ]

    let weatherTile weatherReport =
        childTile "Weather" [
            Level.level [ ] [
                Level.item [ Level.Item.HasTextCentered ] [
                    div [ ] [
                        Level.heading [ ] [
                            Image.image [ Image.Is128x128 ] [
                                img [ Src(sprintf "https://www.metaweather.com/static/img/weather/%s.svg" weatherReport.WeatherType.Abbreviation) ]
                            ]
                        ]
                        Level.title [ ] [
                            Heading.h3 [ Heading.Is4; Heading.Props [ Style [ Width "100%" ] ] ] [
                                (* Task 4.8 WEATHER: Get the temperature from the given weather report
                                   and display it here instead of an empty string. *)
                                str <| sprintf "Temperature %s°" (weatherReport.AverageTemperature.ToString("F0"))
                            ]
                        ]
                    ]
                ]
            ]
        ]
    let locationTile model =
        childTile "Location" [
            div [ ] [
                Heading.h3 [ ] [ str model.Location.Location.Town ]
                Heading.h4 [ ] [ str model.Location.Location.Region ]
                Heading.h4 [ ] [ sprintf "%.1fKM to London" model.Location.DistanceToLondon |> str ]
            ]
        ]


/// The view function knows how to render the UI given a model, as well as to dispatch new messages based on user actions.
let view model dispatch =
    div [] [
        Hero.hero [ Hero.Color Color.IsInfo ] [
            Hero.body [ ] [
                Container.container [ Container.IsFluid
                                      Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
                    Heading.h1 [ ] [
                        str "UK Location Data Mashup"
                    ]
                ]
            ]
        ]

        Container.container [] [
            yield
                Field.div [] [
                    Label.label [] [ str "Postcode" ]
                    Control.div [ Control.HasIconLeft; Control.HasIconRight ] [
                        Input.text
                            [ Input.Placeholder "Ex: EC2A 4NE"
                              Input.Value model.Postcode
                              Input.Modifiers [ Modifier.TextTransform TextTransform.UpperCase ]
                              Input.Color (if model.ValidationError.IsSome then Color.IsDanger else Color.IsSuccess)
                              Input.Props [ OnChange (fun ev -> dispatch (PostcodeChanged !!ev.target?value)); onKeyDown KeyCode.enter (fun _ -> dispatch GetReport) ] ]
                        Fulma.Icon.icon [ Icon.Size IsSmall; Icon.IsLeft ] [ Fa.i [ Fa.Solid.Home ] [] ]
                        (match model with
                         | { ValidationError = Some _ } ->
                            Icon.icon [ Icon.Size IsSmall; Icon.IsRight ] [ Fa.i [ Fa.Solid.Exclamation ] [] ]
                         | { ValidationError = None } ->
                            Icon.icon [ Icon.Size IsSmall; Icon.IsRight ] [ Fa.i [ Fa.Solid.Check ] [] ])
                    ]
                    Help.help
                       [ Help.Color (if model.ValidationError.IsNone then IsSuccess else IsDanger) ]
                       [ str (model.ValidationError |> Option.defaultValue "") ]
                ]
            yield
                Field.div [ Field.IsGrouped ] [
                    Level.level [ ] [
                        Level.left [] [
                            Level.item [] [
                                Button.button
                                    [ Button.IsFullWidth
                                      Button.Color IsPrimary
                                      Button.OnClick (fun _ -> dispatch GetReport)
                                      Button.Disabled (model.ValidationError.IsSome)
                                      Button.IsLoading (model.ServerState = ServerState.Loading) ]
                                    [ str "Submit" ] ] ] ]
                    str "\u00a0"  //&nbsp;
                    Button.button
                        [ 
                          Button.Color IsPrimary
                          Button.OnClick (fun _ -> dispatch Clear)
                          Button.Disabled (model.ValidationError.IsSome)
                          ]
                        [ str "Clear" ]

                ]

            match model with
            | { Report = None; ServerState = (Idle | Loading) } -> ()
            | { ServerState = ServerError error } ->
                yield
                    Field.div [] [
                        Tag.list [ Tag.List.HasAddons; Tag.List.IsCentered ] [
                            Tag.tag [ Tag.Color Color.IsDanger; Tag.Size IsMedium ] [
                                str error
                            ]
                        ]
                    ]
            | { Report = Some model } ->
                yield
                    Tile.ancestor [ ] [
                        Tile.parent [ Tile.Size Tile.Is12 ] [
                            bingMapTile model.Location.Location.LatLong
                        ]
                    ]
                yield
                    Tile.ancestor [ ] [
                        Tile.parent [ Tile.IsVertical; Tile.Size Tile.Is4 ] [
                            locationTile model
                            weatherTile model.Weather
                            (* Task 4.6 WEATHER: Generate the view code for the weather tile
                               using the weatherTile function, supplying the weather report
                               from the model, and include it here as part of the list *)
                        ]
                        Tile.parent [ Tile.Size Tile.Is8 ] [
                            crimeTile model.Crimes
                        ]
                  ]
        ]

        br [ ]

        Footer.footer [] [
            Content.content
                [ Content.Modifiers [ Fulma.Modifier.TextAlignment(Screen.All, TextAlignment.Centered) ] ]
                [ safeComponents ]
        ]
    ]