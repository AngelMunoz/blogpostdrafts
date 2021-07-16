#r "nuget: Scriban"

open Scriban

type Product = { name: string; price: float; description: string }

let renderProducts products = 
    let html = 
        """
        <ul id='products'>
        {{ for product in products }}
          <li>
            <h2>{{ product.name }}</h2>
                 Price: {{ product.price }}
                 {{ product.description | string.truncate 15 }}
          </li>
        {{ end }}
        </ul>
        """
    let result = Template.Parse(html)
    result.Render({| products = products |})

let result =
    renderProducts [
        { name = "Shoes"; price = 20.50; description = "The most shoes you'll ever see"}
        { name = "Potatoes"; price = 1.50; description = "The most potato you'll ever see" }
        { name = "Cars"; price = 10.3; description = "The most car you'll ever see" }
    ]

printfn "%s" result