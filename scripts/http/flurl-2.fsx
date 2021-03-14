#r "nuget: Ply"
#r "nuget: Flurl.Http"

open FSharp.Control.Tasks
open System.IO
open Flurl.Http

task {
    use file = File.OpenWrite("./response.html")

    let! content =
        "https://dev.to/tunaxor/doing-some-io-in-f-4agg"
            .GetStreamAsync()

    do! content.CopyToAsync(file)
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
