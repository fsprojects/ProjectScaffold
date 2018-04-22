[<AutoOpen>]
module internal FSharp.ProjectTemplate.Prelude

open Microsoft.FSharp.Core.Printf
open System
open System.Collections.Generic
open System.Globalization
open System.Reflection
open System.Text

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

let inline toOption x = 
    match x with
    | true, v -> Some v
    | _       -> None

let inline tryWith f x = f x |> toOption

let argDefault x y =
    defaultArg y x

type Boolean with
    static member TryParse x =
        tryWith bool.TryParse x

type Byte with
    static member TryParseWithOptions style provider x =
        Byte.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Byte.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type SByte with
    static member TryParseWithOptions style provider x =
        SByte.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        SByte.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type UInt16 with
    static member TryParseWithOptions style provider x =
        UInt16.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        UInt16.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type Int16 with
    static member TryParseWithOptions style provider x =
        Int16.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Int16.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type UInt32 with
    static member TryParseWithOptions style provider x =
        UInt32.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        UInt32.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type Int32 with
    static member TryParseWithOptions style provider x =
        Int32.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Int32.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type UInt64 with
    static member TryParseWithOptions style provider x =
        UInt64.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        UInt64.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type Int64 with
    static member TryParseWithOptions style provider x =
        Int64.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Int64.TryParseWithOptions NumberStyles.Integer CultureInfo.InvariantCulture x

type Decimal with
    static member TryParseWithOptions style provider x =
        Decimal.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Decimal.TryParseWithOptions NumberStyles.Currency CultureInfo.InvariantCulture x

type Single with
    static member TryParseWithOptions style provider x =
        Single.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Single.TryParseWithOptions NumberStyles.Float CultureInfo.InvariantCulture x

type Double with
    static member TryParseWithOptions style provider x =
        Double.TryParse(x, style, provider) |> toOption

    static member TryParse x =
        Double.TryParseWithOptions NumberStyles.Float CultureInfo.InvariantCulture x

type DateTime with
    static member FromUnixToLocal (timestamp:int64) =
        let start = DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        start.AddSeconds(float (timestamp / 1000L)).ToLocalTime()

    static member FromUnixToUtc (timestamp:int64) =
        let start = DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        start.AddSeconds(float (timestamp / 1000L))

    static member TryParseWithOptions style provider x =
        DateTime.TryParse(x, provider, style) |> toOption

    static member TryParse x =
        DateTime.TryParseWithOptions DateTimeStyles.None CultureInfo.InvariantCulture x

    static member TryParseExactWithOptions style provider (formats: string[]) x =
        System.DateTime.TryParseExact(x, formats, provider, style) |> toOption

    static member tryParseExact formats x =
        DateTime.TryParseExactWithOptions DateTimeStyles.None CultureInfo.InvariantCulture formats x

type DateTimeOffset with
    static member TryParseWithOptions style provider x =
        DateTimeOffset.TryParse(x, provider, style) |> toOption

    static member TryParse x =
        DateTimeOffset.TryParseWithOptions DateTimeStyles.None CultureInfo.InvariantCulture x

    static member TryParseExactWithOptions style provider (formats: string[]) x =
        DateTimeOffset.TryParseExact(x, formats, provider, style) |> toOption

    static member tryParseExact formats x =
        DateTimeOffset.TryParseExactWithOptions DateTimeStyles.None CultureInfo.InvariantCulture formats x

type IDictionary<'Key, 'Value> with
    member inline this.TryFind(key) =
        tryWith this.TryGetValue key 

    member inline this.GetValueOrDefault(key, defaultValue) = 
        this.TryFind(key) |> defaultArg <| defaultValue

let formatExceptionDisplay (e:Exception) =
    let sb = StringBuilder()
    let delimeter = String.replicate 50 "*"
    let nl = Environment.NewLine

    let rec printException (e:Exception) count =
        if (e :? TargetException && (isNull e.InnerException |> not))
        then printException (e.InnerException) count
        else
            if (count = 1) then bprintf sb "%s%s%s" e.Message nl delimeter
            else bprintf sb "%s%s%d)%s%s%s" nl nl count e.Message nl delimeter
            bprintf sb "%sType: %s" nl (e.GetType().FullName)
            // Loop through the public properties of the exception object
            // and record their values.
            e.GetType().GetProperties()
            |> Array.iter (fun p ->
                // Do not log information for the InnerException or StackTrace.
                // This information is captured later in the process.
                if (p.Name <> "InnerException" && p.Name <> "StackTrace" &&
                    p.Name <> "Message" && p.Name <> "Data") then
                    try
                        let value = p.GetValue(e, null)
                        if (isNull value |> not)
                        then bprintf sb "%s%s: %s" nl p.Name (value.ToString())
                    with
                    | e2 -> bprintf sb "%s%s: %s" nl p.Name e2.Message
            )
            if (isNull e.StackTrace |> not) then
                bprintf sb "%s%sStackTrace%s%s%s" nl nl nl delimeter nl
                bprintf sb "%s%s" nl e.StackTrace
            if (isNull e.InnerException |> not)
            then printException e.InnerException (count + 1)
    printException e 1
    sb.ToString()