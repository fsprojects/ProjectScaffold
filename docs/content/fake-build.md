# FAKE build

The ProjectScaffold uses [FAKE](http://fsharp.github.io/FAKE/) to automate the complete build and release process.
The `build.fsx` file contains this logic and is written in FAKE's build DSL.

## Running targets 

You can run any target in the build script using:

    $ build.cmd [TARGETNAME] // on windows
    $ build.sh [TARGETNAME] // on mono
    
If you don't specify a target name it will run the default process, which includes running tests and [building docs](writing-docs.html).
    
More details can be found in the [FAKE docs](http://fsharp.github.io/FAKE/).