# Getting started

This scaffolding process can be used to generate the suggested structure of a typical .NET library.

## Cloning the project

This first thing to do is to clone or copy the ProjectScaffold repository to your own workspace.

## Initializing

In order to start the scaffolding process run 

    $ build.cmd // on windows
    $ build.sh  // on mono

During the init process you will be prompted for the name of your project and some other details: 

![alt text](img/init-script.png "Init script asking for project details")

The project structure will then be generated from templates.
 
## Further topics

* [Running builds](fake-build.html)
* [Paket dependency management](paket-package-management.html)
* [Writing docs](writing-docs.html)
* [Release process](release-process.html)