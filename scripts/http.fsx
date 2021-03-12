#r "nuget: FsHttp"

open FsHttp

http {
    GET "https://jsonplaceholder.typicode.com/users"
    Accept "application/json"
}
|> toText
|> printfn "%s"
