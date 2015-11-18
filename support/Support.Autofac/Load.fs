namespace Support

open Autofac
open Serilog

type public LogRequestsModule() =
    inherit Module()

    override this.AttachToComponentRegistration(componentRegistry,registration) =
        //base.AttachToComponentRegistration(componentRegistry,registration)
        registration.Preparing.Add( fun p -> 
            Log.Debug( "Resolving concrete type {@ConcreteType}", p.Component.Activator.LimitType ) )

module DI =
    open Microsoft.Framework.Configuration
    open Microsoft.Framework.Configuration.Json
    open Autofac.Configuration

    let Load<'T> () = 
        // Add the configuration to the ConfigurationBuilder.
        let config = new ConfigurationBuilder()
        config.AddJsonFile("autofac.json") |> ignore

        // Register the ConfigurationModule with Autofac.
        let module1 = new ConfigurationModule(config.Build())
        let builder = new ContainerBuilder()
        builder.RegisterModule<LogRequestsModule>()|> ignore
        builder.RegisterModule(module1) |> ignore
        let container = builder.Build()
        container.Resolve<'T>()

    let Register<'T, 'U> () = 
        let builder = new ContainerBuilder()
        builder.RegisterModule<LogRequestsModule>()|> ignore
        builder.RegisterType<'T>().As<'U>() |> ignore
        let container = builder.Build()
        container.Resolve<'U>()

