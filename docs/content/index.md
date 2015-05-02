## What is ProjectScaffold?

ProjectScaffold helps you get started with a new .NET/Mono project solution with everything needed for successful organising of code, tools and publishing. 

We recommend you use ProjectScaffold for all new projects.

Tools include [Paket](paket-package-management.html) for dependency management and [FAKE](fake-build.html) for automating the build process.

Paket:

* helps manage NuGet packages
* can reference files [directly with a URL](http://fsprojects.github.io/Paket/http-dependencies.html) from GitHub or from any where on the web
* gives precise and predictable control over referenced packages

FAKE: 

* allows a simple [one step release process](release-process.html)
* works with most [build servers](build-servers.html) 
* compiles the application and [runs all test projects](running-tests.html)
* synchronizes all ``AssemblyInfo`` files prior to compilation
* generates [SourceLinks](https://github.com/ctaggart/SourceLink)
* generates [API docs based on XML documentation tags](writing-docs.html#API-docs)
* generates [documentation based on Markdown files](writing-docs.html#Markdown-files)
* generates and/or pushes [NuGet](http://www.nuget.org) packages

## Getting Started

The first thing to do is to [clone](https://github.com/fsprojects/ProjectScaffold.git) or [copy](https://github.com/fsprojects/ProjectScaffold/archive/master.zip) the ProjectScaffold repository to your developer workspace. This will eventually be your solution folder. Feel free to rename ProjectScaffold folder to your liking.

### Initializing

In order to generate your project first run:

    $ build.cmd // on windows
    $ build.sh  // on mono

This would prompt you to enter a name for your project solution, which is required, and then some more details which are optional:

* Project summary
* Project description
* Author's name for NuGet package
* Tags for NuGet package (separated by spaces)
* GitHub username
* GitHub project name (if different than project name from above)

![alt text](img/init-script.png "Init script asking for project details")

During this initialization process project structure is generated and necessary packages and tools would be downloaded.

After the initialization has finished you can open, edit, build and test using ``ProjectName.sln``.
 
### Further topics

* [Running builds](fake-build.html)
* [Running tests](running-tests.html)
* [Paket dependency management](paket-package-management.html)
* [Writing docs](writing-docs.html)
* [Using build servers](build-servers.html)
* [Release process](release-process.html)

## Contributing and copyright

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork the project and submit pull requests.

If you want to contribute to the documentation, please do so by doing a ``checkout`` of the [``docs`` branch of the repo](https://github.com/fsprojects/ProjectScaffold/tree/docs).

The library is available under Public Domain license, which allows modification and
redistribution for both commercial and non-commercial purposes. For more information see the
[License file][license] in the GitHub repository.

  [content]: https://github.com/fsprojects/FSharp.ProjectScaffold/tree/master/docs/content
  [gh]: https://github.com/fsprojects/FSharp.ProjectScaffold
  [issues]: https://github.com/fsprojects/FSharp.ProjectScaffold/issues
  [license]: https://github.com/fsprojects/FSharp.ProjectScaffold/blob/master/LICENSE.txt
