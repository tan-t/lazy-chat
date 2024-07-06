namespace Slack
open Slack.Event
open System.Threading.Tasks

module Handler =

  type SlackEventHandlerDependencyInjection = {
    resolveForMessage: unit -> Slack.Message.HandleMessageWorkflowCommand
  }

  type SlackEventHandlerResult =
    | Challenge of Challenge.SlackChallengeResponse
    | Message of Message.HandleMessageResult

  type SlackEventHander = SlackEventHandlerDependencyInjection -> Slack.Event.SlackEvent -> Task<Result<SlackEventHandlerResult, string>>

  let slackEventHandler:SlackEventHander = fun (di: SlackEventHandlerDependencyInjection) (ev: Slack.Event.SlackEvent) ->
    match ev with
    | Slack.Event.Challenge challenge ->
      Slack.Challenge.handle challenge.challenge |> SlackEventHandlerResult.Challenge |> Result.Ok |> Task.FromResult
    | Slack.Event.Callback body ->
      match body with
      | Slack.Event.MessageEventBody message ->
        task {
          let resolved = di.resolveForMessage()
          let! result = Slack.Message.handleMessage resolved message
          return result |> Result.map(fun x -> SlackEventHandlerResult.Message x)
        }
      | Slack.Event.UnknownEvetnBody _ -> Result.Error "unknow event" |> Task.FromResult
    | Slack.Event.UnknownEvent ev -> Result.Error ev |> Task.FromResult

