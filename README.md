AutoFac and Primitive obsession: how I learned to love injecting configurations
===============================================================================

Registering components in AutoFac is straightforward, as long as no primitive dependencies are involved. This post describes a technique for dealing with primitive dependencies, such as connection strings, URLs and configuration parameters in general.

## The ordinary case

Say we have a class `Foo` depending on `Bar`: 

```csharp
class Foo
{
    public Foo(Bar bar) { }
}

class Bar {}
```

Registering both of them them is just simple as writing:

```csharp
var builder = new ContainerBuilder();
builder.RegisterType<Foo>();
builder.RegisterType<Bar>();
```

This is enough for AutoFac, since it knows how to create instances of `Bar` (it's just a matter of invoking its default constructor), and consequently it knows how to build instances of `Foo`.

It works with no additional configurations even if dependencies are reversed (e.g `Bar` depends on `Foo`) or if relationships are implicit, for example when `Foo` depends on `Func<Bar>`, or on `List<Bar>` and the like: AutoFac is smart enough to build an instance of the right class and inject it into the right component.

## Here come the primitives

Troubles start when one of the dependencies is a primitive. Suppose that `Foo` also depends on a connection string, which we decided to represent as a `string`:

```csharp
class Foo
{
    public Foo(Bar bar, string connectionString) { }
}
```

We are offered a bunch of native Autofac facilities for registering this class. Either we can use a lambda:

```csharp
builder.Register(c =>
{
    string connectionString = "someConnectionString";
    var bar = c.Resolve<Bar>();
    return new Foo(bar, connectionString);
});
```

or we can continue using the ordinary `RegisterType<>()`, enhanced with the `withParameter()` facility:

```csharp
builder.RegisterType<Foo>()
    .WithParameter("connectionString", "someConnectionString");
```

This might be tollerable as long as there there are just a very little number of primitive dependencies. It starts stinking when it ends up with code like the following


```csharp
builder.Register(c =>
{
    string connectionString = "someConnectionString";
    string url = "http://some.url";
    string maxUsers = 19;
    
    var bar = c.Resolve<Bar>();
    var baz = c.Resolve<Baz>()
    return new Foo(bar, baz, connectionString, url, maxUsers);
});
```

or

```csharp
builder.RegisterType<Foo>()
	.WithParameter("connectionString", "someConnectionString")
	.WithParameter("url", "http://some.url")
	.WithParameter("maxUsers", 19);
```

Not only is it verbose and repetitive, but the resulting code is also affected by a couple problems:

* it is error prone: both the URL and the connectionString are represented with the same class (a string), so it is very possible to switch their value without any chances the receiving class detects the issue but at runtime; would you spot the problem in the following code?

```csharp
class Foo
{
    public Foo(Bar bar, Baz baz, string connectionString, string url) { }
}

builder.Register(c =>
{
    var bar = c.Resolve<Bar>();
    var baz = c.Resolve<Baz>()
    return new Foo(bar, baz, "http://some.url", "someConnectionString", 19);
});
```

The compiler wouldn't, and that's a pity;

* `withParameter` references parameters by names, as strings, so simple refactoring tasks such as renaming variables become very fragile;
The following code compiles, but throws an exception at runtime the moment we will try to resolve `Foo`:


```csharp
class Foo
{
    public Foo(Bar bar, Baz baz, string connectionString, string url) { }
}

builder.RegisterType<Foo>()
	.WithParameter("connectionstring", "someConnectionString")
	.WithParameter("url", "http://some.url")
	.WithParameter("maxUsers", 19);
```

Yes, it's just a matter of a capitalized `S`. Hard to spot, isn't it?



==========================
Appunti



* Le configurazioni sono dipendenze: vantaggi nell'avere la configurazione nel punto di ingresso dell'applicazione.
* Le classi non si rompono a runtime, la dipendenza dalla configurazione diventa esplicita, non è possibile istanziare una classe senza fornire la configurazione.
* La classe diventa indipendente dalla sorgente della configurazione (file? nei test è programmatica)
* Quando capita di dover registrare dei primitivi: principalmente per delle configurazioni

    public class Foo { }

* In quei casi la registrazione diventa prolissa. Portare degli esempi
* Trucco con Autofac withParameter. Svantaggio: la registrazione diventa dipendente dal nome del parametro.
* Soluzione non buona: iniettare una classe che incapsula la configurazione.
* Tuttavia, questo è un ServiceLocator in disguise. Citare l'articolo di Seeman.
* Alternativa: raggruppare le configurazioni (esempio: username e password).
* Soluzione più generale: Evitare i primitivi. Link a primitive obsession. URL non è una stringa. URL, database connection string.
* Trucco in C# per trattare una classe come primitiva, vedi https://lostechies.com/jimmybogard/2007/12/03/dealing-with-primitive-obsession/
