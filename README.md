<h1 align="center">
  <img alt="DISC Logo" src="https://raw.githubusercontent.com/iamscottcab/disc/main/.github/images/logo.png" width="250px"/><br/>
  Dependency Injection <i>(by)</i> Scott Cab
</h1>
<p align="center"><b>DISC</b> is a zero dependency runtime IoC / DI container written in C# for .NET 5.0.</p>

## Introduction
Dependency Injection *(by)* Scott Cab **DISC** is a lightweight DI / IoC container that resolves dependency graphs at runtime through standard constructor based injection patterns. It currently supports the following features:

- Multiple service lifetimes including Singleton, Scoped and Transient.
- Common registration patterns via Generic, Type and factory declarations.
- Support for open generics, similar to ILogger in other frameworks.
- Lifetime validation to try and identify captured dependencies.
- Manual scope creation & service resolution.

## Why Use DISC?
You may want to use the DISC container in the following circumstances:

- You are making a small application or side-project and have requirements for a lightweight DI solution without the features of a larger framework.
- You are new to DI / IoC concepts and want a container with a small footprint and a small suite of options to learn the basics on.
- You want strong out of the box lifetime checking to help validate your injections to better understand your dependency graph.

*OR...*

- You are me, and you wrote it, and you think it's cool... 🤷‍♀️

## ⚙️ Installation
Get the latest release from the [Releases Page](https://github.com/iamscottcab/disc/releases), or clone and compile from source. If you'd like a NuGet Package please open up an issue on the repo and I'll see if I can make it happen.

## ⚡️ Quick Start

1. Add a project reference in your project / solution to point to `DISC.DependencyInjection.dll`.
2. Add the relevant using to the program entry point.
```c#
using DISC;
```
3. In the program entry point create a new root DI scope.
```c#
var rootScope = DI.CreateRootScope();
```
4. Register the entry point specific to your application *(i.e. not `Main(string[] args)`)*.
```c#
rootScope.RegisterEntryPoint<MyApp>();
```
5. Register your dependencies
```c#
rootScope.RegisterSingleton<A>();
rootScope.RegisterSingleton(typeof(ILogger<>), typeof(MyCustomLogger<>));
rootScope.RegisterScoped<B>();
rootScope.RegisterTransient<ISomeInterface, SomeClass>();
rootScope.RegisterTransient(typeof(AnImportantClass), () => new AnImportantClass());
```
6. Resolve your entry point to start the DI chain and bootstrap your application...
```c#
rootScope.ResolveEntryPoint<MyApp>();
```
...or alternatively for an async implementation await on the resolved entry point.
```c#
await rootScope.ResolveEntryPoint<MyApp>().MyEntryMethodAsync();
```

Add the classes as required into your relevant constructors as you would with a traditional DI pattern and the container will resolve the graph as it resolves your entry point.
```c#
public class MyApp {

    private A _a;

    public MyApp(A a) {
        _a = a;
    }
}
```

⚠️ A note on both registering and resolving the entry point class. These are purely to be verbose, especially for beginners, about the intention of adding the main class to your DI tree *(to act as the entry point for DI resolution)*, and to give you some lifetime safety and other error checking. It is however semantically equivalent to adding a Singleton to your DI scope and then getting it if you prefer that syntax.

## 🔭 Scopes
Outside the context of an API, where scopes are fairly intrinsic, there is no automatic tool to handle creating a scope for you, as the context of that scope is largely abritrary. To create your own scopes you can inject an `IScopeProvider` into your class and request a new scope.

Imagine an application that is listening to requests made over a socket that needs to process messages, perhaps from a chat application.

```c#
public class MyApp {

    private IScopeProvider _scopeProvider;

    public MyApp(IScopeProvider scopeProvider) {
        _scopeProvider = scopeProvider;

        SomeOtherLib.onMessageReceieved += ProcessMessage;
    }

    private void ProcessMessage(string message) {
        var scope = _scopeProvider.CreateScope();
        var processor = scope.GetService<MessageProcessor>(); // This is a scoped service, getting a new version only for the lifetime of the container.
        processor.Process(message);
    }
}
```

## 📝 Settings
Strict defaults are great when you're learning or you wish to validate lifetimes at development time but they can get in the way.

You can pass an optional `DISettings` object to `DI.CreateRootScope()` to turn off different forms of lifetime validation if required.

## 💖 Thanks
If you've gotten this far, or you've enjoyed this tool and want to say thanks you can do that in the following ways:
- Add a [GitHub Star](https://github.com/iamscottcab/disc) to the project.
- Say hi on [Twitter](https://twitter.com/iamscottcab).
