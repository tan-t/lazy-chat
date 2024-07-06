namespace Slack
module Challenge =

  type SlackChallengeResponse = {
    challenge: string
  }

  let handle = fun challenge ->
    { challenge = challenge }
