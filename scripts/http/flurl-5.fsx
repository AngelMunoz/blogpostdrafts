#r "nuget: Ply"
#r "nuget: Flurl.Http"

open FSharp.Control.Tasks
open System.IO
open Flurl.Http

task {
    let path = Path.GetFullPath("./dummy.pdf")

    try
        let! response =
            "https://sampleurl.nox"
                .PostMultipartAsync(fun content ->
                    content
                        .AddString("firstName", "Jane")
                        .AddString("lastName", "Smith")
                        .AddString("email", "jane@smith.lol")
                        .AddFile("pdfresume", path, "application/pdf")
                    |> ignore)

        printfn "Status code: %i" response.StatusCode
    with ex -> printfn "%s" ex.Message
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
