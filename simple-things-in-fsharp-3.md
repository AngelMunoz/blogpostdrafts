[Ply]: https://github.com/crowded/ply
[System.Net.Http]: https://docs.microsoft.com/en-us/dotnet/api/system.net.http?view=net-5.0
[Flurl]: https://flurl.dev/
[FsHttp]: https://github.com/ronaldschlenker/FsHttp
[HttpClient]: https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0
[JsonPlaceholder]: https://jsonplaceholder.typicode.com/


# Simple things in F#

This is the third post in Simple things in F#. Today we will talk about doing HTTP Requests

> ***DISCLAIMER***: I'll be using [Ply] in most of the samples due to it's efficient task CE and the easy interoperability between C# tasks as well as F#'s async. Http operations are by nature `async` operations, that means using `async/await` in C# using `async {}` in F# which can be a little ergonomic when you need to append `|> Async.AwaitTask` to every function/method that returns a task, that's why we'll be using [Ply] on the code samples.

When you come to the F# ecosystem you will find that there is a great amount of F# specific and *idiomatic* libraries meaning that the library was designed to be used from F# but there's also a even bigger amount of libraries that use C# as the code base.

What do they have in common?

- They are .NET libraries
- You can use them from any of the .NET languages (C#, F#, VB)

Sometimes these interop cases mean that the library is not *idiomatic* for the language you're using and there may surface interoperation issues between languages but don't let that stop you from trying libraries here and there and you're not wrong in trying to consume a C# library from F# or viceversa.

Why do I mention this? because today we'll see three ways to do Http Requests with different libraries, we'll first explore the BCL's (Base Class Library) [System.Net.Http] then we'll proceed to use [Flurl] and finally we'll check [FsHttp], and depending on your taste or needs you may want to use one or the other and all of them are completely fine to use .


## System.Net.Http

This is part of the BCL, so it's very likely that you'll see lot of code out there using [HttpClient].

Let's begin by downloading a web page

```fsharp
#r "nuget: Ply"

// the following open statement makes the `task {}` CE available
open FSharp.Control.Tasks
open System.Net.Http
open System.IO

task {
    /// note the ***use*** instead of ***let***
    use client = new HttpClient()
    let! response = 
        client.GetStringAsync("https://dev.to/tunaxor/doing-some-io-in-f-4agg")
    do! File.WriteAllTextAsync("./response.html", response)
    // after the client goes out of scope
    // it will get disposed automatically thanks to the ***use*** keyword
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
```

> To Run this, copy this content into a file named `script.fsx` (or whatever name you prefer) and type:
> - `dotnet fsi run script.fsx`

Keep in mind that by doing getting the contents as strings, you are naturally buffering that string in memory which might not be efficient if the website/content you're requesting is quite big and also if the content is a kind of binary file that can't be represented with strings like a PDF, an Excel File or similar. the way you can do this is by using the streams themselves

```fsharp
#r "nuget: Ply"

// the following open statement makes the `task {}` CE available
open FSharp.Control.Tasks
open System.Net.Http
open System.IO

task {
    /// open the file and note the ***use*** keyword in the file and the client
    use file = File.OpenWrite("./dummy.pdf")
    use client = new HttpClient()
    let! response = client.GetStreamAsync("https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf")
    /// copy the response contents to the file asynchronously
    do! response.CopyToAsync(file)
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously

```
> To Run this, copy this content into a file named `script.fsx` (or whatever name you prefer) and type:
> - `dotnet fsi run script.fsx`

this should have downloaded a PDF file and as you can see it's pretty small about 20 LoC including whitespace and comments now we know how to get strings and binary files with `HttpClient` what about posting Json?

for the following examples we'll be using [JsonPlaceholder]

```fsharp
#r "nuget: Ply"

// the following open statement makes the `task {}` CE available
open FSharp.Control.Tasks
open System.Net.Http
open System.Net.Http.Json
// model of a "Post" from the jsonplaceholder website
type Post =
    { userId: int
      id: int
      title: string
      body: string }

task {
    use client = new HttpClient()
    // use an anonymous record to create a partial post
    // (without id because it doesn't exist yet)
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
```
> To Run this, copy this content into a file named `script.fsx` (or whatever name you prefer) and type:
> - `dotnet fsi run script.fsx`

requesting JSON isn't hard either 
```fsharp
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
    let! posts = client.GetFromJsonAsync<Post[]>("https://jsonplaceholder.typicode.com/posts")
    printfn $"%A{posts}" // prints the 100 post array to the console
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
```

This should give you an idea how to do other kind of http verbs as well and shows basic usage of the HttpClient but let's move on to a relatively popular (2.5k gh stars at the moment of writing) library for HTTP requests


# Flurl