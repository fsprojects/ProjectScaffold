namespace FSharp.ProjectTemplate.PowerShell
 open System  
 open System.Management.Automation  
 open System.ComponentModel  
 [<RunInstaller(true)>]  
 type Log4NetSnapIn() =  
   inherit PSSnapIn()  
   override this.Name with get() = "aa"  
   override this.Vendor with get() = "bb"  
   override this.Description with get() = "dd"  
 [<Cmdlet(VerbsCommunications.Write, "Hi")>]  
 type WriteHelp() =   
   inherit Cmdlet()  
   override this.ProcessRecord() =   
     base.WriteObject("help");  
