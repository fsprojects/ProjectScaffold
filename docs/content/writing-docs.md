# Writing docs

In order to edit the docs run:

    $ build KeepRunning
    
This starts the [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) process. 
 
![alt text](img/keep-running.png "Keep running in order to edit docs")

Now open `docs/output/index.html` and start to edit the files in `docs/content`. The build script will update the output automatically.
You just have to refresh the browser. Press any key in command line window when you are ready.  

## Releasing docs

In order to release the docs run:

    $ build ReleaseDocs

This will start the build process and [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) again. The output will then be pushed to your `gh-pages` branch where [GitHub] will pick it up and finishs the release process.

The release of the documentation is also done during the [Release process](release-process.html). 