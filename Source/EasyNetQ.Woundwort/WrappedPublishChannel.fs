namespace EasyNetQ.Woundwort
open EasyNetQ
open System
open System.Collections.Generic
open System.Threading

type WrappedPublishChannel (wrapper, channel : IPublishChannel) =
    interface IPublishChannel with
        member this.Publish<'T> (T : 'T) =
            wrapper <| fun () -> channel.Publish(T)
        member this.Publish<'T> (T : 'T, publishConfiguration) =
            wrapper <| fun () -> channel.Publish(T, publishConfiguration)
        member this.Request<'TRequest, 'TResponse>(request, action) =
            wrapper <| fun () -> channel.Request<'TRequest, 'TResponse>(request, action)
        member this.Request<'TRequest, 'TResponse>(request, action, arguments) =
            wrapper <| fun () -> channel.Request<'TRequest, 'TResponse>(request, action, arguments)
        member this.RequestAsync request =
            channel.RequestAsync request
        member this.RequestAsync (request, arguments : IDictionary<string, obj>) =
            channel.RequestAsync(request, arguments)
        member this.RequestAsync (request, arguments, token) =
            channel.RequestAsync (request, arguments, token)
        member this.RequestAsync (request, token : CancellationToken) =
            channel.RequestAsync (request, token)
        member this.get_Bus () =
            channel.Bus
    interface IDisposable with
        member this.Dispose () =
            channel.Dispose()