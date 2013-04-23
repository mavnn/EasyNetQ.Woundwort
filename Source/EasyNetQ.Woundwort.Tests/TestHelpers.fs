[<AutoOpen>]
module EasyNetQ.Woundwort.Tests.TestHelpers
open System
open EasyNetQ
open Foq

let mockChannel = 
    Mock<IPublishChannel>()
        .Create()
let mockBus =
    Mock<IBus>()
        .Setup(fun x -> <@ x.OpenPublishChannel() @>).Returns(mockChannel)
        .Create()

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