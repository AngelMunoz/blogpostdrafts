[Ply]: https://github.com/crowded/ply/
[Saturn]: https://saturnframework.org/
[Yii Framework]: https://www.yiiframework.com/doc/
[Sails.js]: https://sailsjs.com/
[.NET SDK]: https://dotnet.microsoft.com/download

# Simple things in F#

Hello there, this is the 4th entry in Simple Things F#.

> If you come from PHP, Javascript this might help you understand a little bit more the F# backends or confuse you even more 😆 I'm sorry if it happens the later.

Today we'll try to keep it as simple as possible but no promises, since this entry is about creating web servers in F# using [Saturn] and not only that, we'll also try to go for a more traditional'ish MVC.

When I started my programming career about 6~7 years ago my first job was about doing PHP applications for a Customs Agency I used to use [Yii Framework] which is an MVC Framework for PHP. Later on in my journey I ended up using [Sails.js] which is another MVC framework (this time for node though). In F# we could try to go with `dotnet new mvc -lang F#` or `dotnet new webapp` but that would be the least idiomatic F# solution since ASP.NET assumes (at least on net5.0 and lower) that you will use OOP and while that's certainly something that can be done in F# it's not the ideal solution.

# Saturn
Before checking out the project let's talk about Saturn a bit

> Saturn
>
> A modern web framework that focuses on developer productivity, performance, and maintainability

Saturn is a functional first MVC framework that provides an idiomatic F# way to do backend development. Built on top of ASP.NET and Giraffe so feel free to enjoy performance, specially if you come from Javascript/Python

> Saturn has an opinionated and stablished way on how to do things, please check the Saturn documentation for a more *normal* way to do things within Saturn realm or check the SAFE stack documentation which has a more complete solution if you're looking for a production ready stack.

Having that said... let's start

# The Project
The main point of this is to explain the parts of a Saturn application, not necessarily to tell you how to do MVC with Saturn, as mentioned before, there are existing docs (both Saturn and SAFE) that focus on that. Please keep that in mind as we continue 

You can follow me on your machine if you have the [.NET SDK] installed then you can type

> `dotnet new --install AngelMunoz.Saturn.Templates::1.0.1`
>
> `dotnet new saturn-mvc -o ProjectName`

Once you have your project created on your machine with the following structure

```
Properties/
    launchSettings.json
Todos/
    Controllers.fs
    Models.fs
    Views.fs
    README.md
Views/
    Home.fs
    README.md
wwwroot/
    css/
        styles.css
    js/
        index.js
BaseViews.fs
Program.fs
ProjectName.fsproj
README.md
```
 Let's start by inspecting the `Program.fs` file.

```fsharp
module ProjectName.Program

open Giraffe
open Saturn
open Saturn.Endpoint
open ProjectName.Todos.Controller
open ProjectName.Views


let browser =
    /// pipelines are meant to configure
    /// the request's headers, authorization, challenges
    /// and related settings, they can be done via plugs
    /// or Saturn's specific methods like `set_header`
    pipeline {
        plug acceptHtml
        plug putSecureBrowserHeaders
        plug fetchSession
        set_header "x-pipeline-type" "Browser"
    }

let defaultView =
    /// routers are a collection of functions and handlers
    /// you can use other verbs like post/put/patch/delete
    /// and even accept parameters in the url like
    /// getf "/user/%i/categories/%s" (fun (id: int) (category: string) _ (ctx: HttpContext)-> ...)
    /// putf "/user/%i" (fun (id: int)  _ (ctx: HttpContext)-> ...)
    router {
        get "/" (htmlView (Home.Index()))
        get "/index.html" (redirectTo false "/")
        get "/default.html" (redirectTo false "/")
    }

let browserRouter =
    router {
        /// assigns the pipeline to configure this router's requests
        pipe_through browser

        /// you can either define the routes indiually like the defaultView
        // or forward routes to a particular router
        forward "" defaultView
        /// you can forward calls to controllers as well
        forward "/todos" TodoController
    }
/// you can compose multiple routers in a single one
/// for example a router in charge of JSON requests or XML requests can also be defined
/// and forwarded to a particular router
let appRouter = router { forward "" browserRouter }

[<EntryPoint>]
let main args =
    let app =
        // this is an ASP.NET ApplicationBuilder
        // here you'll find anything that you need to configure
        // your server
        application {
            use_developer_exceptions
            use_endpoint_router appRouter
            use_static "wwwroot"
        }
    // once the app is built, just run it
    run app
    0
```

As it can be seen in the `Program.fs` file we take care of the configuration, if we wanted to use CSRF, Cookies, JWT, Dependency Injection (which I don't think is needed unless your library really really needs it) authentication or authorization it would be here in the application builder.

Let's continue with the router contents. Our first router is `defaultView` which handles `/index.html` and similar calls with  `Home.Index()` which is defined inside `ProjectName.Views`

```fsharp
namespace ProjectName.Views

open Giraffe.ViewEngine
open ProjectName.BaseViews

[<RequireQualifiedAccess>]
module Home =
    let Index () =
        let content =
            [ Partials.Navbar(
                leftLinks =
                    [ a [ _href "/todos"; _class "navbar-item" ] [
                          str "Check the Todos!"
                      ] ]
              )
              article [ _class "page" ] [
                  header [] [
                      h1 [] [ str "Welcome to Saturn!" ]
                  ]
                  p [] [
                      str
                          """
                          Saturn is an F# web framework for asp.net
                          """
                  ]
              ] ]

        Layout.Default(content, "Home")
```
Woah ok, ok, that doesn't look simple! let's go bit by bit

To begin with, this is an XmlNode list which is basically a an HTML page but written in F#

The HTML tas are defined as 

> `tag [(* attributes *)] [(* contents (XmlNodes) *)]`


So our `Index()` function is basically calling a `Default(content, Title)` static method on the `Layout` class if you come from MVC applications from other places it is not uncommon to use a master layout and just put your content in there.
if we take out the partial call and our wrapper list the code looks like this

```fsharp
article (* attributes *) [ _class "page" ]
    // content
    [ header (* attributes *) []
        // content
        [ h1 (* attributes *) []
            // content
            [ str "Welcome to Saturn!" ] ]
          p [] [ str
                    """
                    Saturn is an F# web framework for asp.net
                    """ ]
    ]
```

Hopefully uou can see the pattern in there each tag has a list for attributes and a list for content.

There are other DSL Flavors out there like https://github.com/dbrattli/Feliz.ViewEngine (a react based DSL) or https://github.com/giraffe-fsharp/Giraffe.Razor (if you like razor pages) so if you feel this is too much, don't worry you can still write plain HTML (cshtml) or React jsx like code.

> We're also using a couple of helpers defined in `BaseViews.fs` we will not review those for the sake of keeping this *simple* what we need to know is that there's a layout that we're filling with content and that there's a `Navbar` partial we're using at the begining of our content.

We're essentially defining an HTML view that we will render with `htmlView` from `Giraffe.Core`.

The next Stop is finally our `TodoController` which is defined in `Todos/Controllers.fs`.

> In this section I will omit code for brevity but you will be able to see it locally if you used the dotnet template from the beginning of the post.

```fsharp
namespace ProjectName.Todos
(* ... open namespaces/modules ... *)

module Controller =
    // Models and Views are part of the ProjectName.Todos namespace
    open ProjectName.Todos

    // All of our controller functions
    // are isolated from the application using private

    let private todos = (* ... code ... *)

    let private addTodo = (* ... code ... *)

    let private createTodo = (* ... code ... *)

    let private showTodo = (* ... code ... *)

    let private editTodo = (* ... code ... *)

    let private updateTodo = (* ... code ... *)

    let private deleteTodo = (* ... code ... *)

    /// only TodoController is exposed which is used 
    /// by `forward "/todos" TodoController` in the browserRouter

    let TodoController =
        /// the controllers define a bunch of useful functions
        /// that we can use as a convention to handle http requests
        /// for a particular resource. In this case
        /// we're focusing on To-do's
        controller {
            // GET /todos
            index todos
            // GET /todos/add
            add addTodo
            // POST /todos/add
            create createTodo
            // GET /todos/1
            show showTodo
            // GET /todos/1/edit
            edit editTodo
            // PUT /todos/1
            update updateTodo
            // DELETE /todos/1
            delete deleteTodo
        }
```

Controllers are a good convention for the MVC background where you act directly on a particular resource, you are in no obligation to implement every method for every controller if your controller only needs an index and a add view, only implement those, no more no less.

Let's check the first three functions

```fsharp
// make the Model and View modules available
open ProjectName.Todos 

let private todos =
    fun ctx ->
        task {
            // this is the most ideal MVC situation
            // call a service or the model and get the information
            let! todos = Model.Find()
            // build the html view with the model
            let view = View.Index todos
            // return the rendered HTML view to the client
            return! Controller.renderHtml ctx view
        }

let private addTodo =
    fun ctx ->
        task {
            // the add function is the GET request where we
            // render the HTML where we will send the form
            // I'm not doing CSRF to prevent XSS but here we can also pass
            // the CSRF token if we had enabled it
            let view = View.AddTodo()
            return! Controller.renderHtml ctx view
        }

let private createTodo =
    fun (ctx: HttpContext) ->
        task {
            // since we're not using JS on the frontend we're just doing plain 'ol
            // HTML views, we get the values of our todo from the POST'ed form
            let title = ctx.GetFormValue("title")
            let isDone = ctx.GetFormValue("isDone")
            // we ensable the partial to create our To-do
            let partial =
                { title = title |> Option.defaultValue ""
                    isDone = isDone |> Option.defaultValue "off" }

            // Create the Todo
            let! todo = Model.Create partial
            // build the view
            let view = View.TodoDetail todo
            // return the HTML to the client
            return! Controller.renderHtml ctx view
        }
```
The rest of the functions perform more-less the same functionality given their name either they update or find a particular todo. Lastly let's just check for a bit the `delete` function.

In this case we used [HTMX] in the frontend to do an `ajax` call, there will be cases where we don't want to render completely a new HTML page so we use some kind of javascript code in the frontend (be it a library or hand written code) that will make a request to the server we can chose to act accodringly in those cases as well
```fsharp
let private deleteTodo =
    fun (ctx: HttpContext) id ->
        task {
            let! _ = Model.Delete id
            // set  the HTMX redirect header so once we delete
            // the resource we're redirected to the "/todos" page
            ctx.SetHttpHeader("HX-Redirect", "/todos")
            // set the status code
            ctx.SetStatusCode(204)
            // return an empty response
            return! Controller.text ctx ""
        }
```

Following our MVC code, we saw already the `Controller` let's check the `Model` now.

The Model module simply contains the types we're using in our controller, either from the request as the parameter or as the return value inside the HTML responses.
```fsharp


type Todo =
    { id: int
      title: string
      isDone: bool }

// when you bind forms from application/x-www-form-urlencoded
// the records must be marked as CLI mutable so they can be used correctly
// please bear in mind that if we were using JSON (using System.Text.JSON or Thoth.JSON)
// this attribute would not be needed
[<CLIMutable>]
type PartialTodo = { title: string; isDone: string }

/// we use require qualified access to prevent the polution of the namespace
[<RequireQualifiedAccess>]
/// Model could also be named Services as well, it's just a word to be honest, find
/// the word that fits best for your mental model in the end this is just an API/Interface to
/// interact with your database
module Model =
    open System.Threading.Tasks
    open System.Linq

    // fake database
    let private _todos = lazy (ResizeArray())

    (* Fake async services *)

    let Find () =
        Task.FromResult(_todos.Value |> List.ofSeq)

    let FindOne (id: int) = ... Task ...

    let Create (todo: PartialTodo) = ... Task ...

    let Update (todo: Todo) = ... Task ...

    let Delete (id: int) = ... Task ...
```

In this case the Model module it's just the means on how we acces our database in a real project you can use different conventions or ways to interact with it, the topic can become quite complex really fast hence why I left this part for you to decide how to use it best.

Lastly the views, in reallity we already covered those when we saw `Home.fs` but for sake of completeness let's check the `Index` view

```fsharp
/// let's define a helper function
/// that will define a general skeleton for our page
/// think of it as a `page` partial
let private page attrs content =
    [ Partials.Navbar() // prefill with the navbar
      // and an article with a particular case,
      // but override it if it comes inside attributes.
      article [ yield! attrs; _class "page" ] content ]

/// in F# everything is very likely a function that takes a parameter
/// Our controllers, our Models and our views are no exception
/// to render our index we take a list of To-do's
let Index (todos: Todo list) : XmlNode =
    let content =
        page [ _class "page f-row" ] [
            aside [ _class "menu" ] [
                ul [ _class "menu-list" ] [
                    li [] [
                        // offer a link to add a new todo
                        a [ _href "/todos/add" ] [
                            str "Add Todo"
                        ]
                    ]
                ]
            ]
            /// use a table (much emterprise, such demvelomper, much wow)
            table [ _class "table is-bordered is-striped is-narrow is-hoverable is-fullwidth" ] [
                thead [] [
                    th [] [ str "Id" ]
                    th [] [ str "Title" ]
                    th [] [ str "Is Done" ]
                ]
                tbody [] [
                    // render our todos inside the table
                    for todo in todos do
                        tr [] [
                            td [] [
                                a [ _href $"/todos/{todo.id}" ] [
                                    str $"{todo.id}"
                                ]
                            ]
                            td [] [ str todo.title ]
                            td [] [
                                str (sprintf "%s" (if todo.isDone then "Yes" else "No"))
                            ]
                        ]
                ]
            ]
        ]

    Layout.Default(content, "Todos")
```