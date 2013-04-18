# EasyNetQ.Woundwort

![General Woundwort](http://images.wikia.com/watershipdown/images/6/6a/GeneralWoundwort.jpg)

EasyNetQ with added strategies...

[EasyNetQ](https://github.com/mikehadlow/EasyNetQ) is an opinionated RabbitMQ client for .net. Sometimes it's nice to be able to extend those opinions with some of your own; for example, maybe you want to retry once rather than throw immediately if a connection is closed when you try and publish (EasyNetQ attempts to re-open connections it knows have failed, so this isn't quite as stupid as it sounds).

Woundwort strategies are wrappers around the base EasyNetQ interface that are designed to be composable (probably via IoC container) to give the messaging strategy you need for your app. And if it's not provided here, the worst case is that you've just found the `WrappedBus` and `WrappedPublishChannel` classes to help you easily write your own composable expansions to EasyNetQ. Hope you enjoy!

## A word on design

Woundwort deliberately only changes the behaviour of the 'simple' EasyNetQ methods. If you are planning to use either the Advanced interfaces or the Async methods it's assumed that your application will be handling it's own messaging strategies.


## Current status

Not even alpha. This is more of a request for comment than an actual implementation - let me know what you think in the issues or via email at michael+woundwort@mavnn.co.uk. 

The only strategy currently implemented is the Retry strategy. It's not implemented very well - it's just a proof of concept.

```fsharp
use bus = new RetryBus(RabbitHutch.CreateBus()) :> IBus
```

or

```csharp
IBus bus = new RetryBus(RabbitHutch.CreateBus());

```

will give you a bus that will retry a failed publish or request response after 200 mseconds the first time it throws an EasyNetQException. If it throws anyway else, or fails a second time, you're on your own.

## Licence

EasyNetQ.Woundwort is of course licenced with the same permissive MIT licence as EasyNetQ itself.
