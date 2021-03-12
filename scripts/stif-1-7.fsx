open System.IO

let dir = DirectoryInfo(".")

let dirs = dir.EnumerateDirectories()
let files = dir.EnumerateFiles()

printfn "directories: %s, files: %s" (dirs.GetType().ToString()) (files.GetType().ToString())

type Hierarchy =
    { Name: string
      Files: string list
      Directories: Hierarchy list }


let getFiles (dir: DirectoryInfo) =
    dir.EnumerateFiles()
    |> Seq.map (fun file -> file.FullName)
    |> List.ofSeq

let getDirectories (dir: DirectoryInfo) =
    dir.EnumerateDirectories()
    |> Seq.filter (fun dir -> not (dir.Name.StartsWith(".")))
    |> List.ofSeq

let rec getDirHierarchy (directories: DirectoryInfo list) =
    directories
    |> List.map
        (fun dir ->
            { Name = dir.FullName
              Files = getFiles dir
              Directories = getDirHierarchy (getDirectories dir) })

let getHierarchy (path: string) =
    let dir = DirectoryInfo(path)

    { Name = dir.FullName
      Files = getFiles dir
      Directories = getDirHierarchy (getDirectories dir) }

let hierarchy = getHierarchy "."
printfn $"%A{hierarchy}"
