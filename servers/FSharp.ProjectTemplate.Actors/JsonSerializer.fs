module JsonSerializer

open Orleans.CodeGeneration
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open Orleans.Serialization

[<RegisterSerializerAttribute>]
type JObjectSerialization() =
    static member DeepCopier (original:obj) =
        original
    static member Serializer(untypedInput:Object) (stream:BinaryTokenStreamWriter) (expected:Type) =
        let input : JObject = downcast untypedInput
        let str = input.ToString()
        SerializationManager.Serialize(str, stream);
    static member Deserializer(expected:Type) (stream:BinaryTokenStreamReader) : obj =
        let str : string = downcast SerializationManager.Deserialize(typeof<string>, stream)
        upcast JObject.Parse(str)
