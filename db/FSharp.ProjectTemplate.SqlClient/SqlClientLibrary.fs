namespace FSharp.ProjectTemplate.SqlClient

open FSharp.Data
open FSharp.ProjectTemplate.Domain

module Impl =

    [<Literal>]
    let ConnectionString = @"Name=ProjectTemplate"

    type private GetPersonLastSeenQuery = SqlCommandProvider<"GetPersonLastSeen.sql", ConnectionString>
    type private SavePersonLastSeenQuery = SqlCommandProvider<"SavePersonLastSeen.sql", ConnectionString>
    type private CreateSchemaCommand = SqlCommandProvider<"CreateSchema.sql", ConnectionString>

    let LoadPersonLastSeen ( p : Person ) =
        async {
            let! result = (new GetPersonLastSeenQuery()).AsyncExecute( p.FirstName, p.LastName )
            return result |> Seq.tryHead
        }

    let SavePersonLastSeen ( p : Person ) =
        (new SavePersonLastSeenQuery()).AsyncExecute( p.FirstName, p.LastName ) |> Async.RunSynchronously |> ignore

    let internal createSchema () =
        (new CreateSchemaCommand()).AsyncExecute() |> Async.RunSynchronously |> ignore

type Database() = 

    interface FSharp.ProjectTemplate.IHelloPersistency with
        member this.Load(p:Person) = Impl.LoadPersonLastSeen p |> Async.RunSynchronously
        member this.Save(p:Person) = Impl.SavePersonLastSeen p

