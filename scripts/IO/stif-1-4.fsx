#r "nuget: Ply"

open FSharp.Control.Tasks
open System
open System.IO


task {
    let path = Path.Combine("./", "sample.log")
    let filename = Path.ChangeExtension(path, "txt")
    File.Copy(path, filename)
}
|> Async.AwaitTask
// we need to run this synchronously
// so the fsi can finish executing the tasks
|> Async.RunSynchronously
