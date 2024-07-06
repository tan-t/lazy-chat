namespace Chat
open System.Data
open Slack.Message
open Slack.Event

module Message =
  let resolveForMessage = fun (con:IDbConnection) () ->
    {
      InsertMessage = fun(body: MessageEventBody) ->
        task {
          let res = Result.Error("not implemented")
          return res
        }
    }


