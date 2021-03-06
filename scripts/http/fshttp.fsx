#r "nuget: Ply"
#r "nuget: SchlenkR.FsHttp"

open FSharp.Control.Tasks
open System.IO
open FsHttp
open FsHttp.DslCE
open FsHttp.Response

task {
    let! response = httpAsync { GET "https://dev.to/tunaxor/doing-some-io-in-f-4agg" }
    let! content = response |> toTextAsync
    do! File.WriteAllTextAsync("./response.html", content)
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
