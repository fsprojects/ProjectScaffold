namespace RestFul

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave.Http
open Suave.Http.Successful
open Suave.Http.Applicatives
open Suave.Types
open Suave.Http.RequestErrors


[<AutoOpen>]
module RestFul =    

    // 'a -> WebPart
    let JSON v =     
        let jsonSerializerSettings = new JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()
    
        JsonConvert.SerializeObject(v, jsonSerializerSettings)
        |> OK 
        >>= Writers.setMimeType "application/json; charset=utf-8"

    let fromJson<'a> json =
        JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a    

    let getResourceFromReq<'a> (req : HttpRequest) = 
        let getString rawForm = System.Text.Encoding.UTF8.GetString(rawForm)
        req.rawForm |> getString |> fromJson<'a>

    type RestResource<'a> = {
        GetAll : unit -> 'a seq
        GetById : int -> 'a option
        IsExists : int -> bool
        Create : 'a -> 'a
        Update : 'a -> 'a option
        UpdateById : int -> 'a -> 'a option
        Delete : int -> unit
    }

    let rest resourceName resource =
       
        let resourcePath = "/" + resourceName
        let resourceIdPath = new PrintfFormat<(int -> string),unit,string,string,int>(resourcePath + "/%d")
        
        let badRequest = BAD_REQUEST "Resource not found"

        let handleResource requestError = function
            | Some r -> r |> JSON
            | _ -> requestError

        let getAll= warbler (fun _ -> resource.GetAll () |> JSON)
        let getResourceById = 
            resource.GetById >> handleResource (NOT_FOUND "Resource not found")
        let updateResourceById id =
            request (getResourceFromReq >> (resource.UpdateById id) >> handleResource badRequest)

        let deleteResourceById id =
            resource.Delete id
            NO_CONTENT

        let isResourceExists id =
            if resource.IsExists id then OK "" else NOT_FOUND ""

        choose [
            path resourcePath >>= choose [
                GET >>= getAll
                POST >>= request (getResourceFromReq >> resource.Create >> JSON)
                PUT >>= request (getResourceFromReq >> resource.Update >> handleResource badRequest)
            ]
            DELETE >>= pathScan resourceIdPath deleteResourceById
            GET >>= pathScan resourceIdPath getResourceById
            PUT >>= pathScan resourceIdPath updateResourceById
            HEAD >>= pathScan resourceIdPath isResourceExists
        ]
