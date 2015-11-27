namespace FSharp.ProjectTemplate.MUIApp.ViewModels

open System
open System.Windows
open System.Windows.Input

open FsXaml

open FSharp.ViewModule
open FSharp.ViewModule.Validation

type MainViewModel() as self = 
    inherit ViewModelBase()
    
    // Using validation with default naming
    let firstName = self.Factory.Backing(<@ self.FirstName @>, "", notNullOrWhitespace >> noSpaces >> notEqual "Reed")
    
    // Using validation with custom name
    let validateName = 
        validate "Last Name" 
        >> notNullOrWhitespace 
        >> fixErrors
        >> hasLengthAtLeast 3 
        >> noSpaces 
        >> result
    let lastName = self.Factory.Backing(<@ self.LastName @>, "", validateName)

    let hasValue str = not(System.String.IsNullOrWhiteSpace(str))
    let okCommand = 
        self.Factory.CommandSyncParamChecked(
            (fun param -> MessageBox.Show(sprintf "Hello, %s" param) |> ignore), 
            (fun param -> self.IsValid && hasValue self.FirstName && hasValue self.LastName), 
            [ <@ self.FirstName @> ; <@ self.LastName @> ; <@ self.IsValid @> ])   // Or could be: [ <@ self.FullName @> ])

    do
        // Add in property dependencies
        self.DependencyTracker.AddPropertyDependencies(<@@ self.FullName @@>, [ <@@ self.FirstName @@> ; <@@ self.LastName @@> ])

    member x.FirstName with get() = firstName.Value and set value = firstName.Value <- value
    member x.LastName with get() = lastName.Value and set value = lastName.Value <- value
    member x.FullName with get() = x.FirstName + " " + x.LastName 
    
    member x.OkCommand = okCommand

    // Note that you can filter the validations based on the propertyName parameter,
    // which allows for more efficient processing since you can only check relevant info for that property
    override x.Validate propertyName =
        seq {
            if String.IsNullOrWhiteSpace(x.FullName) then
                yield EntityValidation(["You must provide a name"])
            else if propertyName = "FullName" then
                let err = x.FullName |> (validate propertyName >> notEqual "Reed Copsey" >> resultWithError "That is a poor choice of names")                    
                // Alternatively, this can be done manually:
//                  let err = 
//                        match x.FullName with
//                        | "Reed Copsey" -> ["That is a poor choice of names"]
//                        | _ -> []
                yield PropertyValidation("FullName", err)
                yield EntityValidation(err)
        }
