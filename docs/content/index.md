# Project Scaffold

A prototypical .NET solution (file system layout and tooling), recommended by the F# Foundation.

This scaffolding can be used to generate the suggested structure of a typical .NET library.

Read the [Getting started tutorial](tutorial.html) to learn how you can set up your own build process. 

### Technologies

 The ProjectScaffold project uses a lot of different open source technologies.
 This documentation gives an overview.

| Area                      |  Technologies                             |
|:--------------------------|:------------------------------------------|
| Platforms                 | Linux, Windows, OSX                       |
| Build Automation          | [FAKE](http://fsharp.github.io/FAKE/)     |
| Unit Testing              | [NUnit](http://www.nunit.org/)            |
| Package Formats           | Nuget packages                            |
| Dependency Manager        | [Paket](http://fsprojects.github.io/Paket/) |
| Documentation Authoring   | Markdown, HTML and F# Literate Scripts    |
| Source Code Linking       | SourceLink                                |
| Continuous Build and Test | [Travis](http://travis-ci.org) (Linux/OSX) and [AppVeyor](http://appveyor.com) (Windows) |
| Default Package Hosting   | [nuget.org](http://nuget.org)             |
| Default Documentation Hosting  | [GitHub Pages](https://help.github.com/articles/what-are-github-pages)   |

 Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork 
the project and submit pull requests.

The library is available under Public Domain license, which allows modification and 
redistribution for both commercial and non-commercial purposes. For more information see the 
[License file][license] in the GitHub repository. 

  [content]: https://github.com/fsprojects/FSharp.ProjectScaffold/tree/master/docs/content
  [gh]: https://github.com/fsprojects/FSharp.ProjectScaffold
  [issues]: https://github.com/fsprojects/FSharp.ProjectScaffold/issues
  [license]: https://github.com/fsprojects/FSharp.ProjectScaffold/blob/master/LICENSE.txt