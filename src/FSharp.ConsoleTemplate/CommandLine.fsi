namespace FSharp.ProjectTemplate

open System

/// Command line parsing.
module CommandLine = 
    
    type PredicateOperator =
    | EQ
    | GT
    | LT
    | Between

    type Predicate =
        {
            Operator : PredicateOperator
            StartDate : DateTime
            EndDate : DateTime option
        }
        with
            static member Create : operator : PredicateOperator -> startDate : DateTime -> endDate : DateTime option -> Predicate

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

    val parse : programName : string -> argv : string [] -> ParsedCommand
