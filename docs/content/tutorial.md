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

## Build 

After initialization, you can 

- Open, edit, build and test using ``ProjectName.sln``
- Build and test release binaries using ``build.cmd`` or ``build.sh `` 
- Build and test release packages using ``build.cmd Release`` or ``build.sh Release`` 
- Build and publish release docs using ``build.cmd ReleaseDocs`` or ``build.sh ReleaseDocs``
- Add assets to the GitHub releases (Look into the Release target in ``build.fsx``) 
 
## Release

In order to get your first release process started you have to do a manual step. Please create a `build.cmd` with the following content:

    [lang=batchfile]
    @echo off
    cls
    
    .paket\paket.bootstrapper.exe prerelease
    if errorlevel 1 (
      exit /b %errorlevel%
    )
    
    .paket\paket.exe restore -v
    if errorlevel 1 (
      exit /b %errorlevel%
    )
    
    packages\FAKE\tools\FAKE.exe build.fsx "target=Release" "NugetKey=NUGETKEY" "github-user=GITHUBUSERNAME"  "github-pw=GITHUBPW"
    
Of course you gave to fill in the ``NUGETKEY``, ``GITHUBUSERNAME`` and ``GITHUBPW`` with your own credentials.
The `build.cmd` is listed in the `.gitignore` file. This prevents accidental commits of your login.

After that you can release your software by calling:
 
    $ release.cmd