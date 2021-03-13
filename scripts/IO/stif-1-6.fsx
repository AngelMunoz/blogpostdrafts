open System.IO

let path = Path.Combine(".", "my-dir")
let dirinfo = Directory.CreateDirectory(path)
printfn $"%s{dirinfo.Name} - %s{dirinfo.FullName}"
// use either dirinfo.Delete()
// dirinfo.Delete()
// or Directory.Delete
Directory.Delete(dirinfo.FullName)


let files =
    Directory.EnumerateFiles(".")
    |> Seq.fold (fun prev next -> $"{prev}\n\t{next}") ""

let directories =
    Directory.EnumerateDirectories(".")
    |> Seq.fold (fun prev next -> $"{prev}\n\t{next}") ""

printfn $"files:\t{files}\n directories:\t{directories}"
