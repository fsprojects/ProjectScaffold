namespace FSharp.ProjectTemplate

open CommandLine
open Microsoft.FSharp.Core.Printf
open System
open System.Reflection
open System.Text

module console1 =

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

    [<EntryPoint>]
    let main argv = 
        printfn "%A" argv

        let parsedCommand = parse (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name) argv

        match parsedCommand.Error with
            | Some e -> 
                printfn "%s" <| formatExceptionDisplay e
                printfn "%s" parsedCommand.Usage
            | None -> 
                printfn "%A" parsedCommand

        printfn "Hit any key to exit."
        System.Console.ReadKey() |> ignore
        0