# EasyNetQ.Woundwort

![General Woundwort](http://images.wikia.com/watershipdown/images/6/6a/GeneralWoundwort.jpg)

EasyNetQ with added strategies...

[EasyNetQ](https://github.com/mikehadlow/EasyNetQ) is an opinionated RabbitMQ client for .net. Sometimes it's nice to be able to extend those opinions with some of your own; for example, maybe you want to retry once rather than throw immediately if a connection is closed when you try and publish (EasyNetQ attempts to re-open connections it knows have failed, so this isn't quite as stupid as it sounds).

Woundwort strategies are wrappers around the base EasyNetQ interface that are designed to be composable (probably via IoC container) to give the messaging strategy you need for your app. And if it's not provided here, the worst case is that you've just found the `WrappedBus` and `WrappedPublishChannel` classes to help you easily write your own composable expansions to EasyNetQ. Hope you enjoy!

## A word on design

Woundwort deliberately only changes the behaviour of the 'simple' EasyNetQ methods. If you are planning to use either the Advanced interfaces or the Async methods it's assumed that your application will be handling it's own messaging strategies.


## Current status

Not even alpha. This is more of a request for comment than an actual implementation - let me know what you think in the issues or via email at michael+woundwort@mavnn.co.uk. 

The only strategy currently implemented is the Retry strategy.

You can create a retry bus with the following F# or C# code:

```fsharp
use bus = RabbitHutch.CreateBus() |> RetryBus.CreateBus :> IBus
```

or

```csharp
IBus bus = RetryBus.CreateBus(RabbitHutch.CreateBus());
```

A retry bus will catch the first failed attempt to publish that throws an `EasyNetQException`. If the bus is connected, it will retry immediately - otherwise it will wait up to 5 seconds for the bus to re-connect and attempt to re-send.

If the re-connection times out it will re-throw the original exception, and if the a second publish attempt fails it will throw.

## DoubleTap and Chaos

To help with your testing needs, two WrappedBuses have been provided with... unusual characteristics. The DoubleTapBus will alternately throw `EasyNetQException`s and publish normally, while the `ChaosBus` pseudo-randomly fails 50% of the time. To make testing with the Chaos bus slightly saner, it's always seeded with the same value.

Tests have been written using the wonderful [FsUnit](http://code.google.com/p/fsunit/), and I have some plans to incorporate some [FsCheck](http://fscheck.codeplex.com/) tests when I get a chance.

## Licence

EasyNetQ.Woundwort is of course licenced with the same permissive MIT licence as EasyNetQ itself.
