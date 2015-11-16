namespace Support

module DI =
    open Microsoft.Framework.Configuration
    open Microsoft.Framework.Configuration.Json
    open Autofac
    open Autofac.Configuration

    let Load<'T> () = 
        // Add the configuration to the ConfigurationBuilder.
        let config = new ConfigurationBuilder()
        config.AddJsonFile("autofac.json") |> ignore

        // Register the ConfigurationModule with Autofac.
        let module1 = new ConfigurationModule(config.Build());
        let builder = new ContainerBuilder();
        builder.RegisterModule(module1) |> ignore
        let container = builder.Build()
        container.Resolve<'T>()
