module EasyNetQ.Woundwort.Retry
open EasyNetQ
open FSharp.Control
open System
open System.Threading

let RetryPublish (channel : IPublishChannel) f =
    try
        f ()
    with
    | :? EasyNetQException as ex ->
        let bq = BlockingQueueAgent(0)
        channel.Bus.add_Connected(fun () -> bq.Add())
        if channel.Bus.IsConnected then
            f ()
        else
            try
                bq.Get(5000)
                f ()
            with
            | :? TimeoutException ->
                raise ex