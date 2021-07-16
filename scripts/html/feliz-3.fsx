#r "nuget: Feliz.ViewEngine"

open Feliz.ViewEngine
open type Html
open type prop

let card (content: ReactElement seq) = 
    article [
        className "card is-green"
        children content
    ]
let cardFooter content =
    footer [
        className "card-footer is-rounded"
        yield! content
    ]

let slotedHeader (content: ReactElement seq) = 
    header [
        className "card-header"
        children content
    ]

let customizableHeader content = 
    header [
        className "card-header"
        yield! content
    ]

let card1 = 
    div [
        card [
            slotedHeader [
                h1 [ text "This is my custom card"]
                // className "" <- can't do this
            ]
            p [ text "this is the body of the card" ]
            cardFooter [
                custom("data-my-attr", "extra attributes")
                children (p [text "This is my footer"])
            ]
        ]
    ]

let card2 = 
    div [
        card [
            customizableHeader [
                children (h1 [ text "This is my custom card"])
                className "custom class" 
            ]
            p [ text "this is the body of the card" ]
            cardFooter [
                custom("data-my-attr", "extra attributes")
                children (p [text "This is my footer"])
            ]
        ]
    ]

let r1 = Render.htmlView card1
let r2 = Render.htmlView card2

printfn "%s\n\n%s" r1 r2
