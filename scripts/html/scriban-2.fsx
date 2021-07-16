#r "nuget: Scriban"

open System
open Scriban

type Product = 
    { name: string;
      price: float; 
      details : {| description: string |} }

let detailDiv = 
    """
    <details>
        <summary> {{ product.details.description | string.truncate 15 }} <summary>
        {{ product.details.description }}
    </details>
    """

let renderProducts products = 
    let html = 
        sprintf
            """
            <ul id='products'>
            {{ for product in products }}
              <li>
                <h2>{{ product.name }}</h2>
                     Price: {{ product.price }}
                     {{ "%s" | object.eval_template }}
              </li>
            {{ end }}
            </ul>
            """ detailDiv
    let result = Template.Parse(html)
    result.Render({| products = products |})

let result =
    renderProducts [
        { name = "Shoes"
          price = 20.50
          details = 
            {| description = "The most shoes you'll ever see" |} }
        { name = "Potatoes"
          price = 1.50
          details =
            {| description = "The most potato you'll ever see"  |} }
        { name = "Cars"
          price = 10.3
          details =
            {| description = "The most car you'll ever see"  |} }
    ]

printfn "%s" result