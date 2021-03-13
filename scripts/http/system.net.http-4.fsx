#r "nuget: Ply"

// the following open statement makes the `task {}` CE available
open FSharp.Control.Tasks
open System.Net.Http
open System.Net.Http.Json

type Post =
    { userId: int
      id: int
      title: string
      body: string }

task {
    use client = new HttpClient()

    let partialPost =
        {| userId = 1
           title = "Sample"
           body = "Content" |}

    let! response = client.PostAsJsonAsync("https://jsonplaceholder.typicode.com/posts", partialPost)
    let! createdPost = response.Content.ReadFromJsonAsync<Post>()
    printfn $"Id: {createdPost.id} - Title: {createdPost.title}"
// Id: 101 - Title: Sample
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
