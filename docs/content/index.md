# ProjectScaffold

This project can be used to scaffold a prototypical .NET solution including file system layout and tooling.

This includes [Paket](paket-package-management.html) dependency management and a [FAKE](fake-build.html) build process which: 

* updates all AssemblyInfo files
* compiles the application and runs all test projects with [NUnit](http://www.nunit.org/)
* generates [SourceLinks](https://github.com/ctaggart/SourceLink)
* generates [API docs based on XML document tags](writing-docs.html#API-docs)
* generates [documentation based on Markdown files](writing-docs.html#Markdown-files)
* generates [NuGet](http://www.nuget.org) packages
* and allows a simple [one step release process](release-process.html). 

Read the [Getting started tutorial](tutorial.html) to learn how you can set up your own project.

 Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork  the project and submit pull requests.

If you want to contribute to the documentation the please checkout the [docs](https://github.com/fsprojects/ProjectScaffold/tree/docs) branch.  

The library is available under Public Domain license, which allows modification and 
redistribution for both commercial and non-commercial purposes. For more information see the 
[License file][license] in the GitHub repository. 

  [content]: https://github.com/fsprojects/FSharp.ProjectScaffold/tree/master/docs/content
  [gh]: https://github.com/fsprojects/FSharp.ProjectScaffold
  [issues]: https://github.com/fsprojects/FSharp.ProjectScaffold/issues
  [license]: https://github.com/fsprojects/FSharp.ProjectScaffold/blob/master/LICENSE.txt