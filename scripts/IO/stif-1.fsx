#r "nuget: Ply"

open FSharp.Control.Tasks
open System
open System.IO

printfn "%s" (Path.GetDirectoryName("./scripts/stif/stif-1.fsx"))

let ext =
    Path.GetExtension("./scripts/script.fsx")

let filename = Path.GetFileName("./scripts/script.fsx")

let filenamenoext =
    Path.GetFileNameWithoutExtension("./scripts/script.fsx")

let fullpath = Path.GetFullPath("./scripts/script.fsx")

printfn $"{ext}, {filename}, {filenamenoext}\n{fullpath}"

let pathlike =
    Path.Combine(@"..\", $"{Guid.NewGuid()}", $"{Guid.NewGuid()}", "finalfile.fsx")

printfn "%s" pathlike

// let's build a path from the system's temp path and a "Sample.txt" filename
let path =
    Path.Combine(Path.GetTempPath(), "Sample.txt")

printfn $"Does the file exists yet? {File.Exists(path)}"
let file = File.Create(path)
// let's close the file and check the name
file.Close()
printfn $"{file.Name}"
// and delete it afterwards
File.Delete(path)
// we can check if we did delete it
printfn $"we deleted the file right? {not (File.Exists(path))}"
