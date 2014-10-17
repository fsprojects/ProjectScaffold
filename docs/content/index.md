# ProjectScaffold

This project can be used to scaffold a prototypical .NET solution including file system layout and tooling.

This includes [Paket](paket-package-management.html) dependency management and a [FAKE](fake-build.html) build process which: 

* updates all AssemblyInfo files
* compiles the application and [runs all test projects](running-tests.html)
* generates [SourceLinks](https://github.com/ctaggart/SourceLink)
* generates [API docs based on XML document tags](writing-docs.html#API-docs)
* generates [documentation based on Markdown files](writing-docs.html#Markdown-files)
* generates [NuGet](http://www.nuget.org) packages
* and allows a simple [one step release process](release-process.html). 

## Getting started

This scaffolding process can be used to generate the suggested structure of a typical .NET library.

### Cloning the project

This first thing to do is to clone or copy the ProjectScaffold repository to your own workspace.

### Initializing

In order to start the scaffolding process run 

    $ build.cmd // on windows
    $ build.sh  // on mono

During the init process you will be prompted for the name of your project and some other details: 

![alt text](img/init-script.png "Init script asking for project details")

The project structure will then be generated from templates.

After the initialisation you can open, edit, build and test using ``ProjectName.sln``.
 
### Further topics

* [Running builds](fake-build.html)
* [Running tests](running-tests.html)
* [Paket dependency management](paket-package-management.html)
* [Writing docs](writing-docs.html)
* [Release process](release-process.html)

## Contributing and copyright

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork  the project and submit pull requests.

If you want to contribute to the documentation the please checkout the [docs](https://github.com/fsprojects/ProjectScaffold/tree/docs) branch.  

The library is available under Public Domain license, which allows modification and 
redistribution for both commercial and non-commercial purposes. For more information see the 
[License file][license] in the GitHub repository. 

  [content]: https://github.com/fsprojects/FSharp.ProjectScaffold/tree/master/docs/content
  [gh]: https://github.com/fsprojects/FSharp.ProjectScaffold
  [issues]: https://github.com/fsprojects/FSharp.ProjectScaffold/issues
  [license]: https://github.com/fsprojects/FSharp.ProjectScaffold/blob/master/LICENSE.txt