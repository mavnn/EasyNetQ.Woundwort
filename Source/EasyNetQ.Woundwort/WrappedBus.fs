namespace EasyNetQ.Woundwort
open EasyNetQ
open System

type WrappedBus (publishWrapper : IPublishChannel -> IPublishChannel,
                 subscribeWrapper, bus : IBus) =
    interface IBus with
        member this.OpenPublishChannel () =
            publishWrapper <| bus.OpenPublishChannel ()
        member this.OpenPublishChannel configure =
            publishWrapper <| bus.OpenPublishChannel configure
        member this.Subscribe (subscriptionId, onMessage) =
            subscribeWrapper <| fun () -> bus.Subscribe(subscriptionId, onMessage)
        member this.Subscribe (subscriptionId, onMessage, configure) =
            subscribeWrapper <| fun () -> bus.Subscribe(subscriptionId, onMessage, configure)
        member this.SubscribeAsync (subscriptionId, onMessage) =
            bus.SubscribeAsync(subscriptionId, onMessage)
        member this.SubscribeAsync (subscriptionId, onMessage, configure) =
            bus.SubscribeAsync(subscriptionId, onMessage, configure)
        member this.Respond responder =
            bus.Respond responder
        member this.Respond (responder, arguments) =
            bus.Respond(responder, arguments)
        member this.RespondAsync responder =
            bus.RespondAsync(responder)
        member this.RespondAsync (responder, arguments) =
            bus.RespondAsync(responder, arguments)
        member this.add_Connected value = bus.add_Connected value
        member this.remove_Connected value = bus.remove_Connected value
        member this.add_Disconnected value = bus.add_Disconnected value
        member this.remove_Disconnected value = bus.remove_Disconnected value
        member this.get_IsConnected () = bus.IsConnected
        member this.get_Advanced () = bus.Advanced
    interface IDisposable with
        member this.Dispose () =
            bus.Dispose()