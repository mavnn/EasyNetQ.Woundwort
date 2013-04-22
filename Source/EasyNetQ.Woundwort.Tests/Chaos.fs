module EasyNetQ.Woundwort.Tests.Chaos
open System
open EasyNetQ
open EasyNetQ.Woundwort

let rand = Random(1)

type ChaosChannel =
    inherit WrappedPublishChannel
    new (channel) =
        { inherit WrappedPublishChannel(
            (fun channel f -> 
                match Random().Next 2 with
                | 0 -> raise <| EasyNetQException("Chaos Attack!")
                | _ -> f ()),
            channel) }

type ChaosBus =
    inherit WrappedBus
    new (bus) = 
        { inherit WrappedBus(
            (fun chan -> new ChaosChannel(chan) :> IPublishChannel),
            (fun f -> f()), 
            bus) }
    static member CreateBus(bus : IBus) =
        new ChaosBus(bus)

let chaos tracker =
    RabbitHutch.CreateBus("localhost", fun (x : IServiceRegister) -> ignore <| x.Register<IEasyNetQLogger>(fun _ -> tracker))
    |> ChaosBus.CreateBus
    |> RetryBus.CreateBus