# Running tests

By default ProjectScaffold uses [NUnit](http://www.nunit.org/) as test framework. You can run the test suite by calling:

    $ build RunTests

## Changing the test framework

[FAKE](http://fsharp.github.io/FAKE/) supports many different test frameworks including [xUnit](https://xunit.codeplex.com/), [Machine.Specifications](https://github.com/machine/machine.specifications) and many more.    
If you want to use a different test framework the look for the `RunTest` target in the `build.fsx` file:

    Target "RunTests" (fun _ ->
        !! testAssemblies
        |> NUnit (fun p ->
            { p with
                DisableShadowCopy = true
                TimeOut = TimeSpan.FromMinutes 20.
                OutputFile = "TestResults.xml" })
    )
    
You also need to change the testing framework in the `paket.dependencies` file and run the [update process](paket-package-management.html#Updating-packages).

More details can be found in the [FAKE](http://fsharp.github.io/FAKE/) and [Paket docs](http://fsprojects.github.io/Paket/).