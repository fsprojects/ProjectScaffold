namespace Shared

open System

type LatLong =
    { Latitude : float
      Longitude : float }
type Location =
    { Town : string
      Region : string
      LatLong : LatLong }

type PostCodeRequest =
    { PostCode : string }

type LocationResponse = { Postcode : string; Location : Location; DistanceToLondon : float }
type CrimeResponse = { Crime : string; Incidents : int }
type WeatherType =
    | Snow
    | Sleet
    | Hail
    | Thunder
    | HeavyRain
    | LightRain
    | Showers
    | HeavyCloud
    | LightCloud
    | Clear
    static member Parse =
        let weatherTypes = FSharp.Reflection.FSharpType.GetUnionCases typeof<WeatherType>
        fun (s:string) ->
            weatherTypes
            |> Array.find(fun w -> w.Name = s.Replace(" ", ""))
            |> fun u -> FSharp.Reflection.FSharpValue.MakeUnion(u, [||]) :?> WeatherType
    member this.Abbreviation =
        match this with
        | Snow -> "sn" | Sleet -> "sl" | Hail -> "h" | Thunder -> "t" | HeavyRain -> "hr"
        | LightRain -> "lr" | Showers -> "s" | HeavyCloud -> "hc" | LightCloud -> "lc" | Clear -> "c"

type WeatherResponse = { WeatherType : WeatherType; AverageTemperature : float }

/// Provides validation on data. Shared across both client and server.
module Validation =
    open System.Text.RegularExpressions
    let isValidPostcode postcode =
        Regex.IsMatch(postcode, @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})")