#r "nuget: Feliz.ViewEngine"

open Feliz.ViewEngine

let view = 
    Html.html [
        Html.head [ Html.title "Feliz" ]
        Html.body [
            Html.header [ prop.text "Feliz" ]
        ]
    ]

let document = Render.htmlDocument view

printfn "%s" document