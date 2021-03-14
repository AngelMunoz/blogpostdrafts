#r "nuget: Ply"
#r "nuget: SchlenkR.FsHttp"

open FSharp.Control.Tasks
open FsHttp
open FsHttp.DslCE
open System.IO

task {
    let path = Path.GetFullPath("./dummy.pdf")

    try
        let! response =
            httpAsync {
                POST "https://sampleurl.nox"
                multipart
                valuePart "firstName" "Jane"
                valuePart "lastName" "Smith"
                valuePart "email" "jane@smith.lol"
                filePartWithName "pdfresume" path
            }

        printfn "StatusCode %A" response.statusCode
    with ex -> printfn "%s" ex.Message
}
|> Async.AwaitTask
// we run synchronously
// to allow the fsi to finish the pending tasks
|> Async.RunSynchronously
