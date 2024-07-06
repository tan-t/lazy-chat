open Saturn
open Slack
open System.Data.SQLite

let conn = new SQLiteConnection("Data Source=state.db;Version=3")

let routers = router {
  forward "/slack" (SlackController.slackRouters conn)
}

let app = application {
  use_router routers
  url "http://0.0.0.0:8080"
}

[<EntryPoint>]
let main _argv =
  run app
  0
