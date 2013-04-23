namespace EasyNetQ.Woundwort
open EasyNetQ

type RetryBus =
    inherit WrappedBus
    new (bus) = 
        { inherit WrappedBus(
            (fun chan -> new RetryPublishChannel(chan) :> IPublishChannel),
            (fun f -> f()), 
            bus) }
    static member CreateBus (bus : IBus) =
        new RetryBus(bus)
