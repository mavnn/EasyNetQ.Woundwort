module EasyNetQ.Woundwort.Tests.DoubleTap
open System
open EasyNetQ
open EasyNetQ.Woundwort
open NUnit.Framework
open FsUnit

type DoubleTapChannel =
    inherit WrappedPublishChannel
    new (channel) =
        let lastFailed = ref false
        { inherit WrappedPublishChannel(
            (fun channel f -> 
                match !lastFailed with
                | false -> 
                    lastFailed := true
                    raise <| EasyNetQException("Single tap...")
                | true -> 
                    f ()
                    lastFailed := false),
            channel) }

type DoubleTapBus =
    inherit WrappedBus
    new (bus) = 
        { inherit WrappedBus(
            (fun chan -> new DoubleTapChannel(chan) :> IPublishChannel),
            (fun f -> f()), 
            bus) }
    static member CreateBus(bus : IBus) =
        new DoubleTapBus(bus)

let doubleTap () =
    mockBus
    |> DoubleTapBus.CreateBus
    :> IBus

[<Test>]
let ``Double Tap should throw on first publish`` () =
    let sut = doubleTap ()
    use channel = sut.OpenPublishChannel ()
    (fun () -> channel.Publish("Single attempt.")) |> should throw typeof<EasyNetQException>

[<Test>]
let ``Double Tap should succeed on second attempt`` () =
    let sut = doubleTap ()
    use channel = sut.OpenPublishChannel ()
    try
        channel.Publish("One!")
    with
    | :? EasyNetQException -> channel.Publish("Two!") |> should be (sameAs ())
