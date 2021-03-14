#r "nuget: Ply"
#r "nuget: SchlenkR.FsHttp"

open FSharp.Control.Tasks
open FsHttp
open FsHttp.DslCE
open FsHttp.Response
open System.Text.Json

type Post =
    { userId: int
      id: int
      title: string
      body: string }

let content =
    JsonSerializer.Serialize(
        {| userId = 1
           title = "Sample"
           body = "Content" |}
    )

task {
    let! response =
        httpAsync {
            POST "https://dev.to/tunaxor/doing-some-io-in-f-4agg"
            body
            json content
        }

    let! responsStream = response |> toStreamAsync
    let! responseValue = JsonSerializer.DeserializeAsync<Post>(responsStream)
    printfn "%A" responseValue
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
