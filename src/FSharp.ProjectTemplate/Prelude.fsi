namespace FSharp.ProjectTemplate

open System
open System.Globalization

module Prelude =

    [<Class>]
    type EitherBuilder =
        new : unit -> EitherBuilder
        member Bind : m:Result<'b,'c> * f:('b -> Result<'d,'c>) -> Result<'d,'c>
        member Return : a:'e -> Result<'e,'f>
        member ReturnFrom : m:'a -> 'a

    type Boolean with
      static member TryParse : x:string -> bool option

    type Byte with
      static member TryParse : x:string -> byte option

    type SByte with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> sbyte option
    type SByte with
      static member TryParse : x:string -> sbyte option

    type UInt16 with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> uint16 option
    type UInt16 with
      static member TryParse : x:string -> uint16 option

    type Int16 with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> int16 option
    type Int16 with
      static member TryParse : x:string -> int16 option

    type UInt32 with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> uint32 option
    type UInt32 with
      static member TryParse : x:string -> uint32 option

    type Int32 with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider -> x:string -> int option
    type Int32 with
      static member TryParse : x:string -> int option

    type UInt64 with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> uint64 option
    type UInt64 with
      static member TryParse : x:string -> uint64 option

    type Int64 with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> int64 option
    type Int64 with
      static member TryParse : x:string -> int64 option

    type Decimal with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> decimal option
    type Decimal with
      static member TryParse : x:string -> decimal option

    type Single with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> float32 option
    type Single with
      static member TryParse : x:string -> float32 option

    type Double with
      static member
        TryParseWithOptions : style:NumberStyles ->
                                provider:IFormatProvider ->
                                  x:string -> float option
    type Double with
      static member TryParse : x:string -> float option

    type DateTime with
      static member FromUnixToLocal : timestamp:int64 -> DateTime
      static member FromUnixToUtc : timestamp:int64 -> DateTime
      static member
        TryParseWithOptions : style:DateTimeStyles ->
                                provider:IFormatProvider ->
                                  x:string -> DateTime option
      static member TryParse : x:string -> DateTime option
      static member
        TryParseExactWithOptions : style:DateTimeStyles ->
                                     provider:IFormatProvider ->
                                       formats:string [] ->
                                         x:string -> DateTime option
      static member
        tryParseExact : formats:string [] -> x:string -> DateTime option

    type DateTimeOffset with
      static member
        TryParseWithOptions : style:DateTimeStyles ->
                                provider:IFormatProvider ->
                                  x:string -> DateTimeOffset option
      static member TryParse : x:string -> DateTimeOffset option
      static member
        TryParseExactWithOptions : style:DateTimeStyles ->
                                     provider:IFormatProvider ->
                                       formats:string [] ->
                                         x:string -> DateTimeOffset option
      static member
        tryParseExact : formats: string array -> x:string -> DateTimeOffset option

    /// bool * 'a -> 'a option
    val inline toOption : bool * 'a -> 'a option
    
    /// defaultArg parameters reversed
    val argDefault : x:'a -> y:'a option -> 'a

    /// EitherBuilder() on Result type
    val choose : EitherBuilder

    /// format exception to string for display
    val formatExceptionDisplay : e:Exception -> string