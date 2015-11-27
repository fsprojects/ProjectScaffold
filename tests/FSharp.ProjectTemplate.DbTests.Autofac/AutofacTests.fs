namespace FSharp.ProjectTemplate.DbTests.Autofac

open NUnit.Framework
open Serilog
open System

module Setup=
    let runningOnAppveyor =
      not <| String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))

[<SetUpFixture>]
type SetupTest() =
    [<SetUp>]
    let ``start logging`` =
        Log.Logger <- LoggerConfiguration()
            .Destructure.FSharpTypes()
            .MinimumLevel.Debug() //uncomment to see the full debug in the console
            .WriteTo.ColoredConsole()
            .CreateLogger()
        Log.Information( "Tests started" )

module Tests =

    open FSharp.ProjectTemplate
    open FSharp.ProjectTemplate.Domain
    open System
    open Support

    [<Test>]
    let HokusPokus () =
        let a = FSharp.ProjectTemplate.NMemory.Impl.Database()
        Assert.IsNotNull(a)

    [<Test>]
    let ``simple database crud is working`` () =
        if not Setup.runningOnAppveyor then
            Log.Information( "Test entered" )
            let db = DI.Load<IHelloPersistency> ()
            //let db = DI.Register<FSharp.ProjectTemplate.NMemory.Impl.Database, IHelloPersistency> ()
            let p = {FirstName="John";LastName="Rambo"}
            db.Save( p )
            let lastSeen = db.Load( p )
            Assert.AreEqual( true, lastSeen.IsSome )
            Assert.LessOrEqual( DateTime.Now - lastSeen.Value, TimeSpan.FromSeconds(float 1) )
