#r "paket: groupref FakeBuild //"

#load "./.fake/init.fsx/intellisense.fsx"

open System.Collections.Generic
open System.IO
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Tools

// --------------------------------
// init.fsx
// This file is run the first time that you run build.sh/build.cmd
// It generates the build.fsx and generate.fsx files
// --------------------------------

let dirsWithProjects = ["src";"tests";"docsrc/content"]
                       |> List.map (fun d -> DirectoryInfo (__SOURCE_DIRECTORY__ @@ d))

// special funtions
// many whom might be replaceable with FAKE functions

let failfUnlessExists f msg p = if not <| File.Exists f then failwithf msg p
let combine p1 p2 = Path.Combine(p2, p1)
let move p1 p2 =
  if File.Exists p1 then
    printfn "moving %s to %s" p1 p2
    File.Move(p1, p2)
  elif Directory.Exists p1 then
    printfn "moving directory %s to %s" p1 p2
    Directory.Move(p1, p2)
  else
    failwithf "Could not move %s to %s" p1 p2
let localFile f = combine f __SOURCE_DIRECTORY__
let buildTemplatePath = localFile "build.template"
let outputPath = localFile "build.fsx"

let prompt (msg:string) =
  System.Console.Write(msg)
  System.Console.ReadLine().Trim()
  |> function | "" -> None | s -> Some s
  |> Option.map (fun s -> s.Replace ("\"","\\\""))
let runningOnAppveyor =
  not <| System.String.IsNullOrEmpty(Environment.environVar("CI"))
let runningOnTravis =
  not <| System.String.IsNullOrEmpty(Environment.environVar("TRAVIS"))
let inCI = runningOnAppveyor || runningOnTravis
let promptFor friendlyName =
  if inCI then Some "CONTINUOUSINTEGRATION"
  else prompt (sprintf "%s: " friendlyName)
let rec promptForNoSpaces friendlyName =
  match promptFor friendlyName with
  | None -> None
  | Some s when not <| String.exists (fun c -> c = ' ') s -> Some s
  | _ -> System.Console.WriteLine("Sorry, spaces are disallowed"); promptForNoSpaces friendlyName
let rec promptYesNo msg =
  match prompt (sprintf "%s [Yn]: " msg) with
  | None
  | Some "Y" | Some "y" -> true
  | Some "N" | Some "n" -> false
  | _ -> System.Console.WriteLine("Sorry, invalid answer"); promptYesNo msg

failfUnlessExists buildTemplatePath "Cannot find build template file %s"
  (Path.GetFullPath buildTemplatePath)

// User input
let border = "#####################################################"
let print msg =
  printfn """
  %s
  %s
  %s
  """ border msg border

print """
# Project Scaffold Init Script
# Please answer a few questions and we will generate
# two files:
#
# build.fsx               This will be your build script
# docsrc/tools/generate.fsx This script will generate your
#                         documentation
#
# NOTE: Aside from the Project Name, you may leave any
# of these blank, but you will need to change the defaults
# in the generated scripts.
#
"""

let [<Literal>] defaultGitUrl = "https://github.com"
let [<Literal>] defaultGitRawUrl = "https://raw.githubusercontent.com"

let normalizeUrl (url : string) = url.TrimEnd('/')
let promptAndNormalizeFor (normalize : string -> string) = promptFor >> (function | Some e -> Some(normalize e) | e -> e)
let promptAndNormalizeUrlFor = promptAndNormalizeFor normalizeUrl

let vars = Dictionary<string,string option>()
vars.["##ProjectName##"] <- promptForNoSpaces "Project Name (used for solution/project files)"
vars.["##Summary##"]     <- promptFor "Summary (a short description)"
vars.["##Description##"] <- promptFor "Description (longer description used by NuGet)"
vars.["##Author##"]      <- promptFor "Author"
vars.["##Tags##"]        <- promptFor "Tags (separated by spaces)"
vars.["##GitUrl##"]      <- promptAndNormalizeUrlFor (sprintf "Github url (leave blank to use \"%s\")" defaultGitUrl)
vars.["##GitRawUrl##"]   <- promptAndNormalizeUrlFor (sprintf "Github raw url (leave blank to use \"%s\")" defaultGitRawUrl)
vars.["##GitHome##"]     <- promptFor "Github User or Organization"
vars.["##GitName##"]     <- promptFor "Github Project Name (leave blank to use Project Name)"

let wantGit     = if inCI 
                    then false
                    else promptYesNo "Initialize git repo"
let givenOrigin = if wantGit
                    then promptForNoSpaces "Origin (url of git remote; blank to skip)"
                    else None

//Basic settings
let solutionTemplateName = "FSharp.ProjectScaffold"
let projectTemplateName = "FSharp.ProjectTemplate"
let consoleTemplateName = "FSharp.ConsoleTemplate"
let oldProjectGuid = "7E90D6CE-A10B-4858-A5BC-41DF7250CBCA"
let projectGuid = System.Guid.NewGuid().ToString()
let oldTestProjectGuid = "E789C72A-5CFD-436B-8EF1-61AA2852A89F"
let testProjectGuid = System.Guid.NewGuid().ToString()

//Rename solution file
let templateSolutionFile = localFile (sprintf "%s.sln" solutionTemplateName)
failfUnlessExists templateSolutionFile "Cannot find solution file template %s"
            (templateSolutionFile |> Path.GetFullPath)

let projectName =
  match vars.["##ProjectName##"] with
  | Some p -> p.Replace(" ", "")
  | None -> "ProjectScaffold"
  
let consoleName = sprintf "%sConsole" projectName
let solutionFile = localFile (projectName + ".sln")
move templateSolutionFile solutionFile

//Rename project files and directories
dirsWithProjects
|> List.iter (fun pd ->
    // project files
    pd
    |> DirectoryInfo.getSubDirectories
    |> Array.collect (fun d -> DirectoryInfo.getMatchingFiles "*.?sproj" d)
    |> Array.iter (fun f -> f.MoveTo(f.Directory.FullName @@ (f.Name.Replace(projectTemplateName, projectName).Replace(consoleTemplateName, consoleName))))
    // project directories
    pd
    |> DirectoryInfo.getSubDirectories
    |> Array.iter (fun d -> d.MoveTo(pd.FullName @@ (d.Name.Replace(projectTemplateName, projectName).Replace(consoleTemplateName, consoleName))))
    )

//Now that everything is renamed, we need to update the content of some files
let replace t r (lines:seq<string>) =
  seq {
    for s in lines do
      if s.Contains(t) then yield s.Replace(t, r)
      else yield s }

let replaceWithVarOrMsg t n lines =
    replace t (vars.[t] |> function | None -> n | Some s -> s) lines

let overwrite file content = File.WriteAllLines(file, content |> Seq.toArray); file

let replaceContent file =
  File.ReadAllLines(file) |> Array.toSeq
  |> replace projectTemplateName projectName
  |> replace consoleTemplateName consoleName
  |> replace (oldProjectGuid.ToLowerInvariant()) (projectGuid.ToLowerInvariant())
  |> replace (oldTestProjectGuid.ToLowerInvariant()) (testProjectGuid.ToLowerInvariant())
  |> replace (oldProjectGuid.ToUpperInvariant()) (projectGuid.ToUpperInvariant())
  |> replace (oldTestProjectGuid.ToUpperInvariant()) (testProjectGuid.ToUpperInvariant())
  |> replace solutionTemplateName projectName
  |> replaceWithVarOrMsg "##Author##" "Author not set"
  |> replaceWithVarOrMsg "##Description##" "Description not set"
  |> replaceWithVarOrMsg "##Summary##" "Summary not set"
  |> replaceWithVarOrMsg "##ProjectName##" "FSharpSolution"
  |> replaceWithVarOrMsg "##Tags##" "fsharp"
  |> replaceWithVarOrMsg "##GitUrl##" defaultGitUrl
  |> replaceWithVarOrMsg "##GitRawUrl##" defaultGitRawUrl
  |> replaceWithVarOrMsg "##GitHome##" "[github-user]"
  |> replaceWithVarOrMsg "##GitName##" projectName
  |> overwrite file
  |> sprintf "%s updated"

let rec filesToReplace dir = seq {
  yield! Directory.GetFiles(dir, "*.?sproj")
  yield! Directory.GetFiles(dir, "*.fs")
  yield! Directory.GetFiles(dir, "*.fsi")
  yield! Directory.GetFiles(dir, "*.cs")
  yield! Directory.GetFiles(dir, "*.xaml")
  yield! Directory.GetFiles(dir, "*.fsx")
  yield! Directory.GetFiles(dir, "paket.template")
  yield! Directory.EnumerateDirectories(dir) |> Seq.collect filesToReplace
}

[solutionFile] @ (dirsWithProjects
    |> List.collect (fun d -> d.FullName |> filesToReplace |> List.ofSeq))
|> List.map replaceContent
|> List.iter print

//Replace tokens in build template
let generate templatePath generatedFilePath =
  failfUnlessExists templatePath "Cannot find template %s" (templatePath |> Path.GetFullPath)

  let newContent =
    File.ReadAllLines(templatePath) |> Array.toSeq
    |> replace "##ProjectName##" projectName
    |> replace "##ConsoleName##" consoleName
    |> replaceWithVarOrMsg "##Summary##" "Project has no summmary; update build.fsx"
    |> replaceWithVarOrMsg "##Description##" "Project has no description; update build.fsx"
    |> replaceWithVarOrMsg "##Author##" "Update Author in build.fsx"
    |> replaceWithVarOrMsg "##Tags##" ""
    |> replaceWithVarOrMsg "##GitUrl##" defaultGitUrl
    |> replaceWithVarOrMsg "##GitRawUrl##" defaultGitRawUrl
    |> replaceWithVarOrMsg "##GitHome##" "Update GitHome in build.fsx"
    |> replaceWithVarOrMsg "##GitName##" projectName

  File.WriteAllLines(generatedFilePath, newContent)
  File.Delete(templatePath)
  print (sprintf "# Generated %s" generatedFilePath)

generate (localFile "build.template") (localFile "build.fsx")

//Handle source control
let isGitRepo () =
  try
    let info = Git.CommandHelper.findGitDir __SOURCE_DIRECTORY__
    info.Exists
  with
    | _ -> false

let setRemote (name,url) _ =
  try
    let cmd = sprintf "remote add %s %s" name url
    match Git.CommandHelper.runGitCommand __SOURCE_DIRECTORY__ cmd with
    | true ,_,_ -> Trace.tracefn "Successfully add remote '%s' = '%s'" name url
    | false,_,x -> Trace.traceError <| sprintf "Couldn't add remote: %s" x
  with
    | x -> Trace.traceException x

let isRemote (name,url) value =
  let remote = String.getRegEx <| sprintf @"^%s\s+(https?:\/\/|git@)github.com(\/|:)%s(\.git)?\s+\(push\)$" name url
  remote.IsMatch value

let isScaffoldRemote = isRemote ("origin","fsprojects/ProjectScaffold")

let hasScaffoldOrigin () =
  try
    match Git.CommandHelper.runGitCommand __SOURCE_DIRECTORY__ "remote -v" with
    | true ,remotes,_ ->  remotes |> Seq.exists isScaffoldRemote
    | false,_      ,_ ->  false
  with
    | _ -> false

if isGitRepo () && hasScaffoldOrigin () then
  Shell.deleteDir (Git.CommandHelper.findGitDir __SOURCE_DIRECTORY__).FullName

if wantGit then
  Git.Repository.init __SOURCE_DIRECTORY__ false false
  givenOrigin |> Option.iter (fun url -> setRemote ("origin",url) __SOURCE_DIRECTORY__)

//overwrite release notes
let releaseNotesContent = [sprintf "#### 0.0.1 - %s" <| System.DateTime.Now.ToLongDateString(); "* Initial release"]
overwrite "RELEASE_NOTES.md" releaseNotesContent

//overwrite readme
let readmeContent = [sprintf "# %s" projectName]
overwrite "README.md" readmeContent

//Clean up
File.Delete "init.fsx"
