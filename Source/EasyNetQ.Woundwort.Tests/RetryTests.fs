module EasyNetQ.Woundwort.Tests.RetryTests
open System
open EasyNetQ
open EasyNetQ.Woundwort
open DoubleTap
open NUnit.Framework
open FsUnit

[<Test>]
// [<Explicit("Requires local rabbit bus")>]
let ``Double Tap should succeed with RetryBus`` () =
    let sut = doubleTap (new ResizeArray<Choice<Exception, string * obj[]>>()) |> RetryBus.CreateBus :> IBus
    use channel = sut.OpenPublishChannel ()
    (fun () -> channel.Publish("One!")) |> should not' (throw typeof<EasyNetQException>)
