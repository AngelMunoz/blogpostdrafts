#r "nuget: Giraffe.ViewEngine"

open Giraffe.ViewEngine


let view =
    html [] [
        head [] [ title [] [ str "Giraffe" ] ]
        body [] [ h1 [] [ str "Giraffe" ] ]
    ]

let document = RenderView.AsString.htmlDocument view

printfn "%s" document