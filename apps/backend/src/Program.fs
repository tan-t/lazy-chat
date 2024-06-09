open Giraffe
open Saturn
open Microsoft.AspNetCore.Http


type Person = {
  name: string
  id: int
}

let person = { name = "test";  id = 1; }

let getPersonById = fun personId ->
  { name = "test"; id = personId  }

type SlackChallenge = {
  token: string
  challenge: string
}

let handleSlackEvent = fun challenge ->
  challenge.challenge

let handle = fun (next: HttpFunc) (ctx: HttpContext) ->
  task {
    let! challenge = ctx.BindJsonAsync<SlackChallenge>()
    let! _ = handleSlackEvent challenge |> ctx.WriteTextAsync
    return! next ctx
  }

let routers = router {
  post "/api/events/slack" (handle)
  getf "/api/persons/%i" (fun personId -> getPersonById personId |> json)
}

let app = application {
  use_router routers
  url "http://localhost:8080"
}

[<EntryPoint>]
let main _argv =
  run app
  0
