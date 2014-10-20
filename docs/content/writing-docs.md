# Writing documentation

ProjectScaffold allows to you generate HTML documentation similar to this page using [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting).

The `docs/tools/generate.fsx` file controls the generation of narrative and API documentation.
For most projects, you'll simply need to edit some values located at the top of the file.

The relevant settings are as follows:

  - `referenceBinaries` - A list of the binaries for which documentation should be created. The files listed should each have a corresponding XMLDoc file emitted into the `bin` folder as part of the build process.
  - `website` - The root URL to which the generated documentation should be uploaded. In the included example, this points to the GitHub Pages root for this project.
  - `info` - A list of key/value pairs which further describe the details of your project. This list is exporteded to `docs/tools/templates/template.cshtml` file for data-binding purposes.
        
## Markdown files

The project is configured to convert all `*.md` and `*.fsx` files in `docs/content` to HTML pages and emit the results into `docs/output`.
The `docs/content/index.fsx` file acts as a starting point. Use this file to provide a narrative overview of your project.

You can include *Markdown* and **actual executable F# code** in the files. 

The process for editing documentation is to start a continuous documentation generation loop:

    $ build KeepRunning
    
This starts the [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) process. 
 
![alt text](img/keep-running.png "Keep running in order to edit docs")

Now open `docs/output/index.html` and start to edit the files in `docs/content`. The build script will continuously sync the output automatically; you just have to refresh the browser.

Press any key in command line window to end the loop.

## API docs

ProjectScaffold generates a build target that automatically generates nice looking API docs for your assemblies.

To configure this process look into `docs/tools/generate.fsx` and look for:

    let referenceBinaries = [ "##ProjectName##.dll" ]
    
This will be configured automatically during the init process, but you can add more libraries if you want. In order to trigger the generation process run:

    $ build GenerateReferenceDocs

The result will be nice looking docs like the following sample:

![alt text](img/api-docs.png "API docs with Github Links")  

As you can see it can even create links back to your source code on [GitHub.com](http://github.com).

## Changing the layout

The layout is determined by the `docs/tools/templates/template.cshtml` file. 
It uses the Razor templating engine, and leverages jQuery and Bootstrap. 
Change this file to alter the non-content aspects of your documentation.

## Release process

In order to release the docs run:

    $ build ReleaseDocs

This will start the build process and [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) again. The output will then be pushed to your `gh-pages` branch where [GitHub](http://www.github.com) will pick it up and complete the release process.

The release of the documentation is also done during the [Release process](release-process.html).
