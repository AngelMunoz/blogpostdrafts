#r "nuget: Feliz.ViewEngine"

open Feliz.ViewEngine
open type Html
open type prop

let view = 
    html [
        head [ title "Feliz" ]
        body [
            header [ text "Feliz" ]
        ]
    ]

let document = Render.htmlDocument view

printfn "%s" document