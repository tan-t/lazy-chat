namespace Slack
open System.Threading.Tasks
open Slack.Event

module Message =
  type Message = {
    text: string
  }

  type InsertMessageSuccessResult =
    | Id of int

  type HandleMessageResult = {
    id: InsertMessageSuccessResult
  }

  type InsertMessage = MessageEventBody -> Task<Result<InsertMessageSuccessResult, string>>

  type HandleMessageWorkflowCommand = {
    InsertMessage: InsertMessage
  }

  type HandleMessage = HandleMessageWorkflowCommand -> MessageEventBody -> Task<Result<HandleMessageResult, string>>

  let handleMessage = fun (command: HandleMessageWorkflowCommand) (messageEvent: MessageEventBody) ->
    task {
      let! insertRes = command.InsertMessage messageEvent
      return insertRes |> Result.map( fun x -> ({ id = x }) )
    }

