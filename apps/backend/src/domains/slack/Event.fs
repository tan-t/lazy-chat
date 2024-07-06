namespace Slack
open System.Text.Json
open System.Collections.Generic

module Event =
  type MessageEventBody = {
    text: string
    channel: string
    user: string
    ts: string
  }

  type EventBody =
    | MessageEventBody of MessageEventBody
    | UnknownEvetnBody of string

  type ChallengeEvent = {
    challenge: string
  }

  type SlackEvent =
    | Challenge of ChallengeEvent
    | Callback of EventBody
    | UnknownEvent of string

  let EventBodyConstructor = fun (ev: string) ->
    let json = JsonSerializer.Deserialize<Dictionary<string, string>>(ev)
    match json.["type"].ToString() with
    | "message" -> MessageEventBody ({
      text = json.["text"].ToString();
      channel = json.["channel"].ToString();
      user = json.["user"].ToString();
      ts = json.["ts"].ToString();
    })
    | _ -> UnknownEvetnBody ev

  let SlackEventConstructor = fun (ev: string) ->
    let json = JsonSerializer.Deserialize<Dictionary<string, string>>(ev)
    match json.["type"].ToString() with
    | "challenge" -> Challenge ({challenge = (json.["challenge"].ToString())})
    | "event_callback" -> Callback (EventBodyConstructor (json.["event"].ToString()))
    | _ -> UnknownEvent ev


