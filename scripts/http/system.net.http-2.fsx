#r "nuget: Ply"

// the following open statement makes the `task {}` CE available
open FSharp.Control.Tasks
open System.Net.Http
open System.IO

task {
    use client = new HttpClient()

    let! response = client.GetStringAsync("https://dev.to/tunaxor/doing-some-io-in-f-4agg")
    do! File.WriteAllTextAsync("./response.html", response)
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
