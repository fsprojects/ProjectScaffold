# Getting started

### Initializing

In order to start the scaffolding process run 

    $ build.cmd // on windows    
    $ build.sh  // on mono

During the init process you will be prompted for the name of your project and some other details: 

![alt text](img/init-script.png "Init script asking for project details")

The project structure will then be generated from templates.

### Build and Release

After initialization, you can 

- Open, edit, build and test using ``ProjectName.sln``
- Build and test release binaries using ``build.cmd`` or ``build.sh `` 
- Build and test release packages using ``build.cmd Release`` or ``build.sh Release`` 
- Build and publish release docs using ``build.cmd ReleaseDocs`` or ``build.sh ReleaseDocs``
- Add assets to the GitHub releases (Look into the Release target in ``build.fsx``) 
- Publish packages using ``build.cmd Release`` or ``build.sh Release`` (and specify the NugetAccessKey) 