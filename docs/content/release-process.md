# Release process

During the release process the build script will generate NuGet package and push it to [NuGet.org](http://www.nuget.org). 

It will also create a git tag and a github release entry.
 
## Setup release script
 
Prior to the first release from a given working area, you need to do a manual step. Please create a `release.cmd` with the following content:

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
    
    packages\build\FAKE\tools\FAKE.exe build.fsx "target=Release" "NugetKey=NUGETKEY" "github-user=USER"  "github-pw=PW"
    
(Substitute your specific credentials for ``NUGETKEY``, ``USER`` and ``PW``).

The `release.cmd` is listed in the `.gitignore` file. This prevents accidental commits of your credentials.

### Setup documentation release

Make sure you have a `gh-pages` branch. If it doesn't exist, then just create and push it.

## GitHub releases

The automatic release process creates a [GitHub release](https://github.com/blog/1547-release-your-software). If you want to upload files, you'll need to amend the `Release` target in ``build.fsx`` file. You will find the following:

    // release on github
    createClient (getBuildParamOrDefault "github-user" "") (getBuildParamOrDefault "github-pw" "")
    |> createDraft gitOwner gitName release.NugetVersion (release.SemVer.PreRelease <> None) release.Notes 
    // TODO: |> uploadFile "PATH_TO_FILE"    
    |> releaseDraft
    |> Async.RunSynchronously 

## Release your software

When all your tests are passing and the documentation is in good shape it's time to release your software...

The first step is to edit the [RELEASE_NOTES.md](https://github.com/fsprojects/ProjectScaffold/blob/master/RELEASE_NOTES.md) file. Add an entry with the version no., date and describe the changes.

    [lang=batchfile]
    ### 1.0.0-rc001 - July 25 2014
    * More awesome stuff coming
    * Added SourceLink for Source Indexing PDB
             
    #### 0.5.0-beta002 - October 29 2013
    * Improved quality of solution-wide README.md files
    
    #### 0.5.0-beta001 - October 24 2013
    * Changed name from fsharp-project-scaffold to FSharp.ProjectScaffold
    * Initial release                                                                                                     

After that you can release your software by simply running:
     
    $ release.cmd // on Windows
    $ release.sh  // on mono
