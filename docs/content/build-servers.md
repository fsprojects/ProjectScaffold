# Build servers

The ProjectScaffold is configured to run with a couple of build servers out of the box.


## Travis CI 

The `.travis.yml` specifies the build-and-test scenario on OSX using Travis CI. 
You need to enable your CI build at [travis-ci.org](http://travis-ci.org) by logging on there with your GitHub account and activating the project.
If you enable this, then every pull request, commit, branch and tag of your project on GitHub will be built automatically. 
Builds of open source projects are free. The default build is on Mac OSX you can also specify Linux by changing the default language. 

## AppVeyor

The `appveyor.yml` file specifies the build-and-test scenario on Windows using AppVeyor. You need to enable your CI build at [appveyor.com](http://appveyor.com).
CI builds for open source projects are free.

## TeamCity

If you want to set up your project in TeamCity then create a build configuration and point the console runner to the `build.cmd`.