open System.IO

let dir = DirectoryInfo(".")

let directories =
    dir.GetDirectories("*.*", SearchOption.AllDirectories)

printfn "%A" directories
