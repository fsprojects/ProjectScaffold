module DataAccess

open FSharp.Control.Tasks
open FSharp.Data
open Shared

[<AutoOpen>]
module GeoLocation =
    open FSharp.Data.UnitSystems.SI.UnitNames
    type PostcodesIO = JsonProvider<"http://api.postcodes.io/postcodes/EC2A4NE">

    let getLocation postcode = task {
        let! postcode = postcode |> sprintf "http://api.postcodes.io/postcodes/%s" |> PostcodesIO.AsyncLoad
        return
            { LatLong = { Latitude = float postcode.Result.Latitude; Longitude = float postcode.Result.Longitude }
              Town = postcode.Result.AdminDistrict
              Region = postcode.Result.Nuts } }

    let getDistanceBetweenPositions pos1 pos2 =
        let lat1, lng1 = pos1.Latitude, pos1.Longitude
        let lat2, lng2 = pos2.Latitude, pos2.Longitude
        let inline degreesToRadians degrees = System.Math.PI * float degrees / 180.0
        let R = 6371000.0
        let phi1 = degreesToRadians lat1
        let phi2 = degreesToRadians lat2
        let deltaPhi = degreesToRadians (lat2 - lat1)
        let deltaLambda = degreesToRadians (lng2 - lng1)
        let a = sin (deltaPhi / 2.0) * sin (deltaPhi / 2.0) + cos phi1 * cos phi2 * sin (deltaLambda / 2.0) * sin (deltaLambda / 2.0)
        let c = 2.0 * atan2 (sqrt a) (sqrt (1.0 - a))
        R * c * 1.<meter>

[<AutoOpen>]
module Crime =
    type PoliceUkCrime = JsonProvider<"https://data.police.uk/api/crimes-street/all-crime?lat=51.5074&lng=0.1278">
    let getCrimesNearPosition location =
        (location.Latitude, location.Longitude)
        ||> sprintf "https://data.police.uk/api/crimes-street/all-crime?lat=%f&lng=%f"
        |> PoliceUkCrime.AsyncLoad
        |> Async.StartAsTask
[<AutoOpen>]
module Weather = //NOTE: switch to static json for TP if breaks at compile time becuase sites unaccessible
    type MetaWeatherSearch = JsonProvider<"https://www.metaweather.com/api/location/search/?lattlong=51.5074,0.1278">

    let MetaWeatherSearchJson =
        """[{"distance":17633,"title":"London","location_type":"City","woeid":44418,"latt_long":"51.506321,-0.12714"},{"distance":40258,"title":"Southend-on-Sea","location_type":"City","woeid":35375,"latt_long":"51.548328,0.706400"},{"distance":57621,"title":"Luton","location_type":"City","woeid":27997,"latt_long":"51.894932,-0.428090"},{"distance":77579,"title":"Brighton","location_type":"City","woeid":13911,"latt_long":"50.828869,-0.134140"},{"distance":78016,"title":"Reading","location_type":"City","woeid":32997,"latt_long":"51.452381,-0.996030"},{"distance":78051,"title":"Cambridge","location_type":"City","woeid":14979,"latt_long":"52.209702,0.111420"},{"distance":93380,"title":"Ipswich","location_type":"City","woeid":24522,"latt_long":"52.054138,1.159280"},{"distance":99603,"title":"Oxford","location_type":"City","woeid":31278,"latt_long":"51.756199,-1.259490"},{"distance":107491,"title":"Northampton","location_type":"City","woeid":30599,"latt_long":"52.244869,-0.886160"},{"distance":115244,"title":"Portsmouth","location_type":"City","woeid":32452,"latt_long":"50.804008,-1.087280"}]"""
    type MetaWeatherLocation = JsonProvider<"https://www.metaweather.com/api/location/1393672">

    let MetaWeatherLocationJson = 
        """{"consolidated_weather":[{"id":6414316297256960,"weather_state_name":"Heavy Rain","weather_state_abbr":"hr","wind_direction_compass":"WSW","created":"2019-07-07T19:47:56.413375Z","applicable_date":"2019-07-07","min_temp":22.975,"max_temp":28.8,"the_temp":28.845,"wind_speed":4.366532359304329,"wind_direction":238.91932500795312,"air_pressure":1011.1800000000001,"humidity":81,"visibility":10.479735842678757,"predictability":77},{"id":6399504754409472,"weather_state_name":"Heavy Rain","weather_state_abbr":"hr","wind_direction_compass":"SW","created":"2019-07-07T19:47:59.366532Z","applicable_date":"2019-07-08","min_temp":22.065,"max_temp":24.905,"the_temp":25.630000000000003,"wind_speed":5.81516063863305,"wind_direction":215.70420444405676,"air_pressure":1014.255,"humidity":89,"visibility":7.1376908852302545,"predictability":77},{"id":5086975176474624,"weather_state_name":"Heavy Rain","weather_state_abbr":"hr","wind_direction_compass":"SW","created":"2019-07-07T19:48:02.492308Z","applicable_date":"2019-07-09","min_temp":21.21,"max_temp":24.445,"the_temp":24.314999999999998,"wind_speed":3.9285333318857867,"wind_direction":227.664738050777,"air_pressure":1016.485,"humidity":92,"visibility":9.510396782788515,"predictability":77},{"id":5516152669208576,"weather_state_name":"Heavy Rain","weather_state_abbr":"hr","wind_direction_compass":"WSW","created":"2019-07-07T19:48:05.401292Z","applicable_date":"2019-07-10","min_temp":21.035,"max_temp":27.365000000000002,"the_temp":27.705,"wind_speed":3.781840686075983,"wind_direction":241.54548912889092,"air_pressure":1015.31,"humidity":80,"visibility":10.573873578302711,"predictability":77},{"id":6642052576051200,"weather_state_name":"Heavy Rain","weather_state_abbr":"hr","wind_direction_compass":"SW","created":"2019-07-07T19:48:08.655943Z","applicable_date":"2019-07-11","min_temp":21.62,"max_temp":27.5,"the_temp":26.814999999999998,"wind_speed":4.792288565065352,"wind_direction":234.4745011044219,"air_pressure":1014.1800000000001,"humidity":82,"visibility":9.220837807205918,"predictability":77},{"id":4929632203702272,"weather_state_name":"Heavy Rain","weather_state_abbr":"hr","wind_direction_compass":"SSW","created":"2019-07-07T19:48:11.726287Z","applicable_date":"2019-07-12","min_temp":21.48,"max_temp":24.5,"the_temp":23.63,"wind_speed":3.744240237015828,"wind_direction":212.99999999999997,"air_pressure":1014.61,"humidity":91,"visibility":9.999726596675416,"predictability":77}],"time":"2019-07-07T23:06:40.340019+01:00","sun_rise":"2019-07-07T06:33:15.708050+01:00","sun_set":"2019-07-07T19:05:20.177453+01:00","timezone_name":"LMT","parent":{"title":"Nigeria","location_type":"Country","woeid":23424908,"latt_long":"9.084570,8.674250"},"sources":[{"title":"BBC","slug":"bbc","url":"http://www.bbc.co.uk/weather/","crawl_rate":360},{"title":"Forecast.io","slug":"forecast-io","url":"http://forecast.io/","crawl_rate":480},{"title":"Met Office","slug":"met-office","url":"http://www.metoffice.gov.uk/","crawl_rate":180},{"title":"OpenWeatherMap","slug":"openweathermap","url":"http://openweathermap.org/","crawl_rate":360},{"title":"World Weather Online","slug":"world-weather-online","url":"http://www.worldweatheronline.com/","crawl_rate":360}],"title":"Ibadan","location_type":"City","woeid":1393672,"latt_long":"7.378840,3.895270","timezone":"Africa/Lagos"}"""
    
    let getWeatherForPosition location = task {
        let! locations =
            (location.Latitude, location.Longitude)
            ||> sprintf "https://www.metaweather.com/api/location/search/?lattlong=%f,%f"
            |> MetaWeatherSearch.AsyncLoad
        let bestLocationId = locations |> Array.sortBy (fun t -> t.Distance) |> Array.map (fun o -> o.Woeid) |> Array.head
        return!
            bestLocationId
            |> sprintf "https://www.metaweather.com/api/location/%d"
            |> MetaWeatherLocation.AsyncLoad }