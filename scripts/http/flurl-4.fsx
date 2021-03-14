#r "nuget: Ply"
#r "nuget: Flurl.Http"

open FSharp.Control.Tasks
open Flurl.Http

type Post =
    { userId: int
      id: int
      title: string
      body: string }

task {
    let! postResult =
        "https://jsonplaceholder.typicode.com/posts"
            .WithHeaders(
                {| Accept = "application/json"
                   X_MY_HEADER = "my-header-value" |},
                true
            )
            .PostJsonAsync(
                {| userId = 1
                   title = "Sample"
                   body = "Content" |}
            )
            .ReceiveJson<Post>()

    printfn "%A" postResult
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
