open System.IO

Directory.Move("./samples", "./samples2")

let dir = DirectoryInfo("./samples2")

dir.MoveTo("./samples")
