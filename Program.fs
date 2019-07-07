module Build.Program

open BlackFox.Fake

[<EntryPoint>]
let main argv =
    BuildTask.setupContextFromArgv argv

    let defaultTask = Tasks.createAndGetDefault()

    if argv.Length > 0 
        && (argv.[0].ToLower().StartsWith("-h") || argv.[0].ToLower().StartsWith("--h" ) || argv.[0].ToLower()= "help" || argv.[0].ToLower()= "list" ) then
        Tasks.listAvailable()
        0
    else
        BuildTask.runOrDefaultApp defaultTask