namespace FSharp.ProjectScaffold.MUIApp.Views

open FsXaml

type MainView = XAML<"MainView.xaml", true>

// This is to demonstrate being able to add in "code behind"
// Note, in this case, this only displays a message when double clicking on 
// the full name text box
// see https://github.com/fsprojects/FsXaml/blob/master/demos/WpfSimpleMvvmApplication/MainView.xaml.fs
type MainViewController() =
    inherit UserControlViewController<MainView>()

    let showMessage _ =
        System.Windows.MessageBox.Show "You double clicked on Full Name!"
        |> ignore

    override this.OnLoaded view =                                
        // Subscribe to the double click event, but also unsubscribe when we unload
        view.tbFullName.MouseDoubleClick.Subscribe showMessage
        |> this.DisposeOnUnload
        

