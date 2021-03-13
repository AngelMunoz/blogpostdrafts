#r "nuget: Ply"

open FSharp.Control.Tasks
open System
open System.IO


task {
    let source = Path.Combine("./", "sample.txt")

    let destiny =
        Path.Combine("./", "samples", "moved.txt")

    File.Move(source, destiny)
}
|> Async.AwaitTask
// we need to run this synchronously
// so the fsi can finish executing the tasks
|> Async.RunSynchronously
