# Release process

## Setup release script
 
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
    
    packages\FAKE\tools\FAKE.exe build.fsx "target=Release" "NugetKey=NUGETKEY" "github-user=USER"  "github-pw=PW"
    
Of course you gave to fill in the ``NUGETKEY``, ``USER`` and ``PW`` with your own credentials.
The `build.cmd` is listed in the `.gitignore` file. This prevents accidental commits of your login.

## GitHub releases

The automatic release process is creating a [GitHub release](https://github.com/blog/1547-release-your-software). If you want to upload files to GitHub during the release process then locate the `Release` target in ``build.fsx``. You will find the following:

    // release on github
    createClient (getBuildParamOrDefault "github-user" "") (getBuildParamOrDefault "github-pw" "")
    |> createDraft gitOwner gitName release.NugetVersion (release.SemVer.PreRelease <> None) release.Notes 
    // TODO: |> uploadFile "PATH_TO_FILE"    
    |> releaseDraft
    |> Async.RunSynchronously 

## Release your software

All your tests pass, the documentation is in good shape then it's time to release your software.

The first step is to edit the [RELEASE_NOTES.md](https://github.com/fsprojects/ProjectScaffold/blob/master/RELEASE_NOTES.md) file. Add an entry with the version no., date and describe the changes.

    [lang=batchfile]
    ### 1.0.0-rc001 - July 25 2014
    * More awesome stuff comming
    * Added SourceLink for Source Indexing PDB
             
    #### 0.5.0-beta002 - October 29 2013
    * Improved quality of solution-wide README.md files
    
    #### 0.5.0-beta001 - October 24 2013
    * Changed name from fsharp-project-scaffold to FSharp.ProjectScaffold
    * Initial release                                                                                                     

After that you can release your software by calling:
 
    $ release.cmd