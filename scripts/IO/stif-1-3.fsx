#r "nuget: Ply"

open FSharp.Control.Tasks
open System
open System.IO


task {
    let path = Path.Combine("./", "sample.log")
    let! lines = File.ReadAllLinesAsync path
    let! content = File.ReadAllTextAsync path
    printfn $"Content:\n\n{content}"
    printfn $"Lines in file: %i{lines.Length}"
}
|> Async.AwaitTask
// we need to run this synchronously
// so the fsi can finish executing the tasks
|> Async.RunSynchronously
