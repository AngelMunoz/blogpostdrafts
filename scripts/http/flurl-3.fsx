#r "nuget: Ply"
#r "nuget: Flurl.Http"

open FSharp.Control.Tasks
open System.IO
open Flurl.Http

task {
    let path = Path.GetFullPath(".")

    let! result =
        "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf"
            .DownloadFileAsync(path, "dummy.pdf")

    printfn "%s" result
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
