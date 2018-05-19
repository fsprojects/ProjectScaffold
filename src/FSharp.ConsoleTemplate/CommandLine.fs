namespace FSharp.ProjectTemplate

open Argu
open System

module CommandLine = 
    let private bind f = 
        function
        | Ok x -> f x
        | Error x -> Error x

    let private returnM = Ok

    type EitherBuilder() =
        member __.Return a = returnM a
        member __.Bind (m, f) = bind f m
        member __.ReturnFrom m = m

    let choose = EitherBuilder()

    type PredicateOperator =
    | EQ
    | GT
    | LT
    | Between
        override __.ToString() =
            match __ with
            | EQ -> "eq"
            | GT -> "gt"
            | LT -> "lt"
            | Between -> "between"

    type Predicate =
        {
        Operator : PredicateOperator
        StartDate : DateTime
        EndDate : DateTime option
        }
        static member Create operator startDate endDate =
            match endDate with
            | Some _x ->
                match operator with
                | EQ
                | GT
                | LT -> 
                    sprintf "operator is %O and end date is requested" operator
                    |> invalidArg "Log Predicate Create"
                | Between ->
                    {
                    Operator = operator
                    StartDate = startDate
                    EndDate = endDate
                    }

            | None -> 
                match operator with
                | EQ
                | GT
                | LT -> 
                    {
                    Operator = operator
                    StartDate = startDate
                    EndDate = endDate
                    }
                | Between ->
                    sprintf "operator is %O and no end date is requested" operator
                    |> invalidArg "Log Predicate Create"

    type Source =
        | File of string list
        | Console of string
        | NoSource

    type Target =
        | File of string
        | Console
        
    type ParsedCommand =
        {
        Usage : string
        Predicate : Predicate option
        Source : Source
        Target : Target
        Error: Exception option
        }

    type CLIArguments =
        | [<AltCommandLine("-eq"); Unique>] EQ of string
        | [<AltCommandLine("-gt"); Unique>] GT of string
        | [<AltCommandLine("-lt"); Unique>] LT of string
        | [<AltCommandLine("-btwn"); Unique>] Between of string * string

        | [<AltCommandLine("-i")>] InputFiles of string list

        | [<AltCommandLine("-f")>] InputFolder of string list

        | [<AltCommandLine("-o")>] Output of string
        | [<AltCommandLine("-c")>] ConsoleInput of string 
        | [<AltCommandLine("-p")>] Print
  
         with
            interface IArgParserTemplate with
                member s.Usage =
                    match s with
                    | EQ _ -> "equals datetime, e.g. \"08/23/2017 14:57:32\" or 08/23/2017"
                    | GT _ -> "greater than datetime"
                    | LT _ -> "less than datetime"
                    | Between _ -> "between datetimes, comma separated"
                    | InputFiles _ -> "(optional) file path to process"
                    | InputFolder _ -> "(optional) folder path to process"
                    | Output _ -> "(optional, not implemented) output path"
                    | ConsoleInput _ -> "input from console"
                    | Print -> "print target"

    let parseCommandLine programName (argv : string []) = 

        try
            match argv, argv.Length with
            | _, 0 -> 
                Error (invalidArg "no arguments" "")
            | help, 1  when help.[0].ToLower() = "--help" ->
                Error (invalidArg "" "")
            | _, _ ->
                let parser = 
                    ArgumentParser.Create<CLIArguments>(programName = programName)

                let commandLine = parser.Parse argv
                let usage = parser.PrintUsage()

                Ok (commandLine, usage)
        with e ->
            Error e       

    let parseDate msg (date : string) =
        let d = date.Replace("\'", "").Replace("\"", "")
        match DateTime.TryParse d with
        | true, dt -> dt 
        | _ -> 
            match DateTime.TryParse (sprintf "%s 0:0:0" d) with
            | true, dt -> dt 
            | _ -> 
                invalidArg msg date

    let getEq (commandLine : ParseResults<CLIArguments>) =
        try
            match commandLine.TryGetResult <@ EQ @> with
            | Some dateTime ->
                {
                Operator = PredicateOperator.EQ
                StartDate = parseDate "error parsing EQ date" dateTime
                EndDate = None
                } |> Some |> Ok
            | None -> Ok None
            
        with e ->
            Error e

    let getGt (commandLine : ParseResults<CLIArguments>) =
        try
            match commandLine.TryGetResult <@ GT @> with
            | Some dateTime ->
                {
                Operator = PredicateOperator.GT
                StartDate = parseDate "error parsing GT date" dateTime
                EndDate = None
                } |> Some |> Ok
            | None -> Ok None
            
        with e ->
            Error e

    let getLt (commandLine : ParseResults<CLIArguments>) =
        try
            match commandLine.TryGetResult <@ LT @> with
            | Some dateTime ->
                {
                Operator = PredicateOperator.LT
                StartDate = parseDate "error parsing LT date" dateTime
                EndDate = None
                } |> Some |> Ok
            | None -> Ok None
            
        with e ->
            Error e

    let getBetween (commandLine : ParseResults<CLIArguments>) =
        try
            match commandLine.TryGetResult <@ Between @> with
            | Some (dateTime1, dateTime2) ->
                let dt1 = parseDate "error parsing Between first date" dateTime1
                let dt2 = parseDate "error parsing Between 2nd date" dateTime2

                if dt2 > dt1 then
                    {
                    Operator = PredicateOperator.Between
                    StartDate = dt1
                    EndDate = Some dt2
                    } |> Some |> Ok
                else invalidArg "start date not less than end date" ""
            | None -> Ok None
            
        with e ->
            Error e

    let mergePredicate eq gt lt between=
        try
            match ([eq; gt; lt; between] |> List.choose id) with
            | [predicate] ->
                predicate
                |> Ok
            | hd::tl ->
                sprintf "%s, %s" (hd.Operator.ToString()) (tl.Head.Operator.ToString())
                |> invalidArg "multiple predicates selected" 
            | _ -> invalidArg "no predicate selected" "EQ, GT, LT, Between"
        with e ->
            Error e

    let parseTarget (commandLine : ParseResults<CLIArguments>) = 

        let targetList = 
            []
            |> (fun x -> 
                    if commandLine.Contains <@ Output @> then 
                        match commandLine.TryGetResult <@ Output @> with
                        | Some path -> (Target.File path)::x
                        | None -> x
                    else x)

        match targetList with
        | [] -> Ok Target.Console
        | [x] -> Ok x
        | hd::tl -> 
            ArgumentException(sprintf "more than one output target specified: %s, %s" (hd.ToString()) (tl.Head.ToString())) :> Exception
            |> Error 

    let parseSource (commandLine : ParseResults<CLIArguments>) = 

        let sourceList = 
            []
            |> (fun x -> 
                    if commandLine.Contains <@ InputFiles @> then 
                        match commandLine.TryGetResult <@ InputFiles @> with
                        | Some path -> (Source.File path)::x
                        | None -> x
                        
                    else x)
            |> (fun x -> 
                    if commandLine.Contains <@ ConsoleInput @> then 
                        match commandLine.TryGetResult <@ ConsoleInput @> with
                        | Some consoleInput -> (Source.Console consoleInput)::x
                        | None -> x
                    else x)

        match sourceList with
        | [] -> Ok Source.NoSource
        | [x] -> Ok x
        | hd::tl ->
            ArgumentException(sprintf "more than one input source specified: %s, %s" (hd.ToString()) (tl.Head.ToString())) :> Exception
            |> Error

    let parse programName argv = 

        match choose { 
                        let! commandLine, usage = parseCommandLine programName argv

                        let! eq = getEq commandLine
                        let! gt = getGt commandLine
                        let! lt = getLt commandLine
                        let! between = getBetween commandLine
                        let! mergedPredicate = mergePredicate eq gt lt between

                        let! target = parseTarget commandLine
                        let! source = parseSource commandLine
                        
                        return 
                            {
                            Usage = usage
                            Predicate = Some mergedPredicate
                            Source = source
                            Target = target
                            Error = None
                            } 
                        } with
        | Ok x -> x
        | Error (e : Exception) -> 
            let usage = ArgumentParser.Create<CLIArguments>(programName = programName).PrintUsage()
            {
            Usage = usage
            Predicate = None
            Source = Source.NoSource
            Target = Target.Console 
            ParsedCommand.Error = Some e
            } 

