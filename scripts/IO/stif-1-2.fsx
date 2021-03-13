#r "nuget: Ply"

open FSharp.Control.Tasks
open System
open System.Collections.Generic
open System.IO
open System.Text

let queue = new Queue<string>()

let log (logValue: string) = queue.Enqueue logValue

/// naive approach to logging
let flushToLog () =
    task {
        let path = Path.Combine("./", "sample.log")
        // do! File.WriteAllLinesAsync(path, queue)
        do! File.AppendAllLinesAsync(path, queue)
        queue.Clear()
    }

task {
    for i in 1 .. 10 do
        log $"Logging: %i{i}"

    do! flushToLog ()

    for i in 11 .. 20 do
        log $"Logging: %i{i}"

    do! flushToLog ()

    for i in 21 .. 30 do
        log $"Logging: %i{i}"

    do! flushToLog ()
}
|> Async.AwaitTask
// we need to run this synchronously
// so the fsi can finish executing the tasks
|> Async.RunSynchronously
