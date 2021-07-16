#r "nuget: Giraffe.ViewEngine"

open Giraffe.ViewEngine


let card attributes = 
    article [ yield! attributes; _class "card is-green"]

let cardFooter attributes =
    footer [ yield! attributes; _class "card-footer is-rounded"]

let cardHeader attributes =
    header [ yield! attributes; _class "card-header no-icons"]

let mySection = 
    div [] [
        card [] [
            cardHeader [] [
                h1 [] [ str "This is my custom card"]
                img [ _src "https://some-image.com"; _class "card-header-image" ]
            ]

            p [] [ str "this is the body of the card" ]

            cardFooter [ _data "my-attr" "extra attributes" ] [
                p [] [ str "This is my footer"]
            ]
        ]
    ]

let document = RenderView.AsString.htmlNode mySection

printfn "%s" document
