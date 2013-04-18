module EasyNetQ.Woundwort.Retry
open EasyNetQ
open System.Threading

let Retry f =
    try
        f ()
    with
    | :? EasyNetQException ->
        Thread.Sleep 200
        f ()