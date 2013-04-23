[<AutoOpen>]
module EasyNetQ.Woundwort.Tests.TestHelpers
open System
open EasyNetQ
open Foq

let mutable private mockBusO : IBus option = None
let mutable private mockChannelO : IPublishChannel option = None

mockChannelO <-
    Mock<IPublishChannel>()
        .Setup(fun x -> <@ x.Bus @>).Returns(fun () -> mockBusO.Value)
        .Create()
    |> Some

mockBusO <-
    Mock<IBus>()
        .Setup(fun x -> <@ x.OpenPublishChannel() @>).Returns(fun () -> mockChannelO.Value)
        .Setup(fun x -> <@ x.IsConnected @>).Returns(true)
        .Create()
    |> Some

let mockBus = mockBusO.Value
let mockChannel = mockChannelO.Value

let testTracker (collector : ResizeArray<Choice<Exception, string * obj[]>>) =
    { new IEasyNetQLogger with
        member this.DebugWrite(format, args) =
            ()
        member this.InfoWrite(format, args) =
            ()
        member this.ErrorWrite(format, args) =
            collector.Add(Choice2Of2 (format, args))
        member this.ErrorWrite(ex) =
            collector.Add(Choice1Of2 ex)
            }