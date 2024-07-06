namespace Infrastructure
open System.Data

module Context =
  type LazyChatContext = {
    connection: IDbConnection
  }
