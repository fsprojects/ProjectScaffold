# FAKE build

The ProjectScaffold uses [FAKE](http://fsharp.github.io/FAKE/) to automate the complete build and release process.
The `build.fsx` file contains this logic and is written in FAKE's build DSL.
It uses the [FAKE - F# Make](http://fsharp.github.io/FAKE/) library to manage many aspects of maintaining a solution.
It contains a number of common tasks (i.e. build targets) such as directory cleaning, unit test execution, and NuGet package assembly.
You are encouraged to adapt existing build targets and/or add new ones as necessary. 
However, if you are leveraging the default conventions, as setup in this scaffold project, you can start by simply supplying some values at the top of this file.

They are as follows:

  - `project` - The name of your project, which is used in serveral places: in the generation of AssemblyInfo, as the name of a NuGet package, and for locating a directory under `src`.
  - `summary` - A short summary of your project, used as the description in AssemblyInfo. It also provides a short summary for your NuGet package.
  - `description` - A longer description of the project used as a description for your NuGet package.
  - `authors` - A list of author names, as should be displayed in the NuGet package metadata.
  - `tags` - A string containing space-separated tags, as should be included in the NuGet package metadata.
  - `solutionFile` - The name of your solution file (sans-extension). It is used as part of the build process.
  - `testAssemblies` - A list of [FAKE](http://fsharp.github.io/FAKE/) globbing patterns to be searched for unit-test assemblies.
  - `gitHome` - The URL of user profile hosting this project's GitHub repository. This is used for publishing documentation.
  - `gitName` - The name of this project's GitHub repository. This is used for publishing documentation.

## Running targets 

You can run any target in the build script using:

    $ build.cmd [TARGETNAME] // on windows
    $ build.sh [TARGETNAME] // on mono
    
If you don't specify a target name it will run the default process, which includes running tests and [building docs](writing-docs.html).
    
More details can be found in the [FAKE docs](http://fsharp.github.io/FAKE/).