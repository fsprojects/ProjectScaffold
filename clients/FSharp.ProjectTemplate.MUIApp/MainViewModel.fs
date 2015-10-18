namespace FSharp.ProjectScaffold.MUIApp.ViewModels

open System
open System.Windows
open System.Windows.Input

open FsXaml
open FSharp.ViewModule
open FSharp.ViewModule.Validation

type MainViewModel() as self = 
    inherit ViewModelBase()

    let firstName = self.Factory.Backing(<@ self.FirstName @>, "")
    let lastName = self.Factory.Backing(<@ self.LastName @>, "")
    let hasValue str = not(System.String.IsNullOrWhiteSpace(str))
    let okCommand = 
        self.Factory.CommandSyncParamChecked(
            (fun param -> MessageBox.Show(sprintf "Hello, %s" param) |> ignore), 
            (fun param -> hasValue self.FirstName && hasValue self.LastName), 
            [ <@ self.FirstName @> ; <@ self.LastName @> ])   // Or could be: [ <@ self.FullName @> ])

(* 
    do
        // Add in property dependencies
        self.Factory.SetPropertyDependencies(<@ self.FullName @>, [ <@ self.FirstName @> ; <@ self.LastName @> ])
*)
    
    member x.FirstName with get() = firstName.Value and set value = firstName.Value <- value
    member x.LastName with get() = lastName.Value and set value = lastName.Value <- value
    member x.FullName with get() = x.FirstName + " " + x.LastName 

    member x.OkCommand = okCommand
