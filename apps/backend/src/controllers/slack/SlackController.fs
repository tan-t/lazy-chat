namespace Slack
open Saturn
open Giraffe
open Microsoft.AspNetCore.Http
open Shared.CustomResponse
open Handler

module SlackController =
  let slackRouters = fun (conn: System.Data.IDbConnection) ->
    router {
      post "/events" (fun (next: HttpFunc) (ctx:HttpContext) ->
        task {
          let! jsonStr = ctx.ReadBodyFromRequestAsync()
          let! response = jsonStr |> Slack.Event.SlackEventConstructor |> Slack.Handler.slackEventHandler { resolveForMessage = Chat.Message.resolveForMessage conn }

          match response with
          | Ok body ->
            match body with
            | SlackEventHandlerResult.Challenge challenge ->
              return! json challenge next ctx
            | _ ->
              return! json body next ctx
          | Error error ->
            printf "error: %s" (error)
            ctx.SetStatusCode(400)
            let errorRes = { error = error }
            let! _ = ctx.WriteJsonAsync(errorRes)
            return! next ctx
        }
      )
    }
