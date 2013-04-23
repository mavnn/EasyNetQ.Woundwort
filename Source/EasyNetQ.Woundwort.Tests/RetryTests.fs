module EasyNetQ.Woundwort.Tests.RetryTests
open System
open EasyNetQ
open EasyNetQ.Woundwort
open DoubleTap
open NUnit.Framework
open FsUnit

[<Test>]
let ``Double Tap should succeed with RetryBus`` () =
    let sut = doubleTap () |> RetryBus.CreateBus :> IBus
    use channel = sut.OpenPublishChannel ()
    channel.Publish("One!")
    |> should be (sameAs ())
    