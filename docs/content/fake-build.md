# FAKE build

The ProjectScaffold uses [FAKE](http://fsharp.github.io/FAKE/) to automate the complete build and release process.
The `build.fsx` file contains this logic and is written in FAKE's build DSL.


After initialization, you can 

- Open, edit, build and test using ``ProjectName.sln``
- Build and test release binaries using ``build.cmd`` or ``build.sh `` 
- Build and test release packages using ``build.cmd Release`` or ``build.sh Release`` 

More details can be found in the [FAKE docs](http://fsharp.github.io/FAKE/). 