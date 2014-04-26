// -------------------------------------------------------------------------------
// Initial rename of library and project script
// -------------------------------------------------------------------------------

// Bind operator
let (>>=) m f = Option.bind f m

// Args function that parses command line arguments
let getArg argv key =
  let arg = Array.tryFind(fun (a:string) -> a.StartsWith(key)) argv
  match arg with
  | Some x -> x.Replace(key, "") |> Some
  | None -> None

// Thread-safe console logger
let ts () = System.DateTime.Now.ToString("o")           // ISO-8601
let cw (s:string) = System.Console.WriteLine(s)         // Threadsafe console writer
let cew (s:string) = System.Console.Error.WriteLine(s)  // Threadsafe console error writer
type LogLevel = Info | Warning | Error
let log level x y =
  let msg = sprintf "%s - %A: %A (%A)" (ts()) level x y
  match level with
  | LogLevel.Error -> cew msg
  | LogLevel.Info | LogLevel.Warning -> cw msg

// Generic process executer (needed for "git mv source target")
let executeProcess exe args dir =
  try
    let psi = new System.Diagnostics.ProcessStartInfo(exe,args) 
    psi.CreateNoWindow <- true        
    psi.UseShellExecute <- false
    psi.RedirectStandardOutput <- true
    psi.RedirectStandardError <- true
    psi.WorkingDirectory <- dir
    let p = System.Diagnostics.Process.Start(psi)
    let o = new System.Text.StringBuilder()
    let e = new System.Text.StringBuilder()
    p.OutputDataReceived.Add(fun x -> o.AppendLine(x.Data) |> ignore)
    p.ErrorDataReceived.Add(fun x -> e.AppendLine(x.Data) |> ignore)
    p.BeginErrorReadLine()
    p.BeginOutputReadLine()
    p.WaitForExit()
    (p.ExitCode, o.ToString(), e.ToString()) |> Some
  with ex -> log LogLevel.Error (exe,args,dir) ex; None

// Scaffold & Template
let scaffold = "FSharp.ProjectScaffold"
let template = "FSharp.ProjectTemplate"

// The name of the library (will replace "FSharp.ProjectScaffold")
let lib = 
  ((fsi.CommandLineArgs,"lib=") ||> getArg, "FSharp.Foo")
  ||> defaultArg

// The name of the project (will replace "FSharp.ProjectTemplate")
let proj =
  ((fsi.CommandLineArgs,"proj=") ||> getArg, "FSharp.Bar")
  ||> defaultArg

// Folder & file helper functions
let root = __SOURCE_DIRECTORY__
let recursively = System.IO.SearchOption.AllDirectories
let pattern filter = "*" + filter + "*"
let pattern' filter = "*" + filter
let dirs path filter =
  System.IO.Directory.EnumerateDirectories(path,filter,recursively)
let files path filter =
  System.IO.Directory.EnumerateFiles(path,filter,recursively)
let rev (s:string) =
  s |> Seq.toArray |> Array.fold(fun a x -> (x |> string) + a) ""
let replaceFirst input from to' =
  let r = new System.Text.RegularExpressions.Regex(from)
  r.Replace(input = input,replacement = to', count = 1)
let rename' path path' =
  let exe  = "git"
  let args = sprintf "mv \"%s\" \"%s\"" path path'
  (exe,args,root) |||> executeProcess, path, path'
let rename (path:string) from to' =
  let from' = from  |> rev
  let to''  = to'   |> rev
  let path' = (path |> rev, from', to'') |||> replaceFirst |> rev
  (path,path') ||> rename'
let rollback xs = xs |> List.iter(fun (x,y) -> (y,x) ||> rename' |> ignore)

// File content helper functions
let utf8 = System.Text.UTF8Encoding.UTF8
let readLines path = System.IO.File.ReadLines(path,utf8)
let writeLines path (contents:string seq)  =
  System.IO.File.WriteAllLines(path,contents,utf8)
let copy from to' =
  System.IO.File.Copy(from,to',true)
let delete path = System.IO.File.Delete(path)
let extensions = [ ".sln"; ".fs"; ".fsx"; ".fsproj"; ".nuspec"; ".md" ]

// Rename files or directories
let renameIO from to' fn atomic' =
  try
      (root,from |> pattern) ||> fn
      |> Seq.map(fun x -> (x,from,to') |||> rename)
      |> Seq.fold(fun (i,acc) (x,y,z) ->
                  let i' =
                    match x with
                    | Some (a,b,c) -> a
                    | None -> 1
                  (i+i',(y,z)::acc)) (0,[])
      |> fun (x,y) ->
        match x with
        | 0 -> (y,atomic') ||> List.append |> Some
        | _ -> atomic' |> rollback; None
  with ex -> log LogLevel.Error (atomic',from,to') ex; None

// Update files content
let updateContent exts atomic' =
  try
    exts
    |> Seq.map(fun x -> (root,x |> pattern') ||> files)
    |> Seq.fold(fun a x -> (x,a) ||> Seq.append) Seq.empty
    |> Seq.filter(fun x -> not (x.Contains "rename.fsx"))
    |> Seq.fold(fun a x ->
                let x' = x + "_"
                x |> readLines
                  |> Seq.map(fun y -> y.Replace(scaffold,lib)
                                       .Replace(template,proj))
                  |> writeLines x'
                (x,x')::a) []
    |> Seq.iter(fun (x,y) -> (y,x) ||> copy; y |> delete)
    |> Some
  with ex ->
    let exe  = "git"
    let args = sprintf "checkout -- *"
    let git  = (exe,args,root) |||> executeProcess
    atomic' |> rollback
    log LogLevel.Error (exts,git) ex; None

// Rename with atomicity "git mv file2 file1"
[] |> Some >>= (renameIO scaffold lib dirs)
           >>= (renameIO template proj dirs)
           >>= (renameIO scaffold lib files)
           >>= (renameIO template proj files)

// Update content
           >>= (updateContent extensions)
