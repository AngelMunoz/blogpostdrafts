#r "nuget: Ply"

// the following open statement makes the `task {}` CE available
open FSharp.Control.Tasks
open System.Net.Http
open System.IO

task {
    /// open the file and note the ***use*** keyword in the file and the client
    use file = File.OpenWrite("./dummy.pfd")
    use client = new HttpClient()
    let! response = client.GetStreamAsync("https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf")
    ///
    do! response.CopyToAsync(file)
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
