AutoFac and Primitive obsession: how I learned to love the injection of configuration parameters
================================================================================================

Registering components in AutoFac is straightforward, as long as no primitive dependencies are involved. This post describes a technique for dealing with primitive dependencies, such as connection strings, URLs and configuration parameters in general.

## The ordinary case

Say you have a class `Foo` depending on `Bar`: 

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

Troubles start when one of the dependencies is a primitive. Suppose that `Foo` also depends on a connection string, which you decided to represent as a `string`:

```csharp
class Foo
{
    public Foo(Bar bar, string connectionString) { }
}
```

You are offered a bunch of native Autofac facilities for registering this class. Either you can use a lambda:

```csharp
builder.Register(c =>
{
    string connectionString = "someConnectionString";
    var bar = c.Resolve<Bar>();
    return new Foo(bar, connectionString);
});
```

or you can continue using the ordinary `RegisterType<>()`, enhanced with the `withParameter()` facility:

```csharp
builder.RegisterType<Foo>()
    .WithParameter("connectionString", "someConnectionString");
```

### Pain points

This might be tollerable as long as there there are just a very little number of primitive dependencies. It starts stinking when it ends up with code like the following:


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

Not only is it verbose and repetitive, but the resulting code is also affected by a couple of problems:

* both the URL and the connection string are represented with the same class (a string), giving no chances to the compiler to know which is which; consequently, it is very possible to switch their value without any chances the receiving class detects the issue but at runtime. Would you spot the problem in the following code?

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

* `withParameter` references parameters by name, as a string, so simple refactoring tasks such as renaming variables become very fragile. The following code compiles, but it will throw an exception at runtime the moment you will try to resolve `Foo`:


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

## The illusory solution
Why do you need to have configuration parameters, in the first place? Of course because you want the freedom to change them at runtime, presumably reading them from a configuration file:

```csharp
var connectionString = ConfigurationManager.AppSetting["myConnection"];

builder.Register(c =>
{
    var bar = c.Resolve<Bar>();
    return new Foo(bar, connectionString);
});

```

You suggests you a trick you could be tempted to use: to directly inject `ConfigurationManager.AppSetting` and solve so the dependency from primitive parameters.

```csharp
class Foo
{
    public Foo(Bar bar, NameValueCollection config)
    {
        var connectionString = config["myConnection"];
    }
}

builder.RegisterType<Foo>();
builder.RegisterInstance(ConfigurationManager.AppSetting).As<NameValueCollection>();
```

Or may be you could be tempted to define a custom service for collecting all of your configuration parameters:

```csharp
class MyConfigs
{
    public T Get<T>(string key)
    {
        ...
    }

    public void LoadFromConfigfile()
    {
        ...
    }
}

builder.RegisterType<Foo>();
builder.RegisterInstance(new MyConfig().LoadFromConfigfile());
```

so that you can easily inject it into the classes that need one or more configuration parameters:

```csharp
class Foo
{
    Foo(Bar bar, MyConfigs conf)
    {
        var maxUsers = conf.Get<int>("maxUsers");
    }
}
```

This seems to solve some of the problems related to Primitive Obsession, right?

Well, it does. But, in fact, it also introduces some additional problems, possibly worse than the ones it is supposed to solve.

The problem is, it's a Service Locator. I stronly suggest you to read the seminal Mark Seeman's post [Service Locator Is An Anti-Pattern](http://http://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/) which collects a lot of reasons why you should avoid using of the Service Locator pattern. Basically, the root of Service Locator's evil is that id hides class dependencies, causing run-time errros and unneeded maintenance additional burden.

Now, Service Locator is much more about services, while here we are dealing with configurazion parameters, values with no behaviour, simple strings and integers. Yet Mark Seeman's argument apply.

Injecting a configuration-parameters locator is an anti-pattern. Just don't do it.

## A hint
So what to do? An idea may come from some special cases. There are times when configuration parameters come bundled together.

Take a class `Foo` that requires the access to an external service `Bar` and therefore needs to receive the url, the username and the access token via injection. It may come natural to group the authentication parameters in a single class:

```csharp
class BarServiceAuthParameters
{
    string Url { get; }
    string Username { get; }
    string AccessToken { get; }

    public BarServiceAuthParameters(string url, string username, string accessToken)
    {
        Url = url;
        Username = username;
        AccessToken = accessToken;
    }
}

class Foo
{
    public Foo(BarServiceAuthParameters authParams) { }
}
```

Now, registering `Foo` is just:

```csharp
builder.RegisterType<Foo>();
```

since it doesn't depend on any primitives. All the burden is actually left to `BarServiceAuthParameters`, but this is also pretty straightforward, because it's just a matter of registering an instance, with no other dependencies:

```csharp
builder.RegisterInstance(new BarServiceAuthParameters("http://some.url", "john", "token-123"));
```

It's nice that it isn't anymore a business of `Foo` how to register parameters: for its point of view, parameters are just an ordinary dependency.

This should give us a suggestion: if you only wrap any primitive parameter with a DTO class, you could fix the issue we described so far.

## Winning the Primitive Obsession
So, if you have a composite configuation (such as `BarServiceAuthParameters`), you already have a simple solution at hand. And there's the possibility to do the same for simpler, primitive configuration parameters too.

There is something wrong in injecting a primitive.
Say you have 2 configuration parameters: `maxDownloadableFiles` and `numerOfItemsPerPage`. They are defined in 2 completely different contexts, yet you can represent both of them with an `int`.


The root error is to share the same class (in this case, an `int`) for the 2 completely different purposes. It would be just like having `CustomerController` and `NHibernateSession` represented by the very same class. Doing this, you would give no chance neither to Autofac nor to the compiler itself to distinguish the first from the second.

You surely see the benefit in providing the customer controller and the NHibernate session with 2 dedicated classes: there wouldn't be hard to see the same benefit to using 2 different classes to the 2 configuration parameters. 

In fact, you may be averse to creating small objects only for holding simple values such as connection strings, user names, integer or floating point levels or values  that you might simply represent with a prmitive type,

Anyway, that's the first step to avoid convoluted registration expression with AutoFac.


We can enhance the solution in a couple of ways, which will be shown in few lines, but before delving into details, let's elaborate on the Primitive Obsession: what is it, and in which way is it detrimental to the quality of code.


The common solution is just to wrap the primitives in DTO class: DDD calls them Value Object. In the the very short post [Primitive Obsession](http://wiki.c2.com/?PrimitiveObsession) Jb Rainsberger claims those kind of Vlaue Object


> [...] become "attractive code", meaning literally a class/module that attracts behavior towards it as new methods/functions. For example, instead of scattering all the parsing/formatting behavior for dates stored as text, introduce a DateFormat class which attracts that behavior as methods called parse() and format(), almost like magic.

So, it's likely that just by introducing the class `URL` you will end up enhancing it with some formatting or validation logic, which you could not do with a plain, primitive `string`.


## Enhancing the solution

There are drawbacks with Value Object.

Say you replace `string ConnectionString` with a wrapper class

```csharp
class ConnectionString
{
    public string Value { get; }

    public ConnectionString(string value)
    {
        Value = value;
    }
}
```

Now it's just more difficult to consume it. You need to write

```csharp
connectionString.Value
```

where it used to be just

```csharp
connectionString
```

since it's not a string anymore. It's also more difficult to assign it a value. Instead of

```csharp
var connectionString = "foobar";
```

you need

```csharp
var connectionString = new ConnectionString("foobar"); 
```

Yawn...


### Extending Primitives and Primary Constructors


If only primitive types weren't not sealed, a solution could even be:

```csharp
class ConnectionString : string { }
class Url : string { }
class MaxUsers : int { }

```

Unfortunately, this isn't supported by C#. You must rely on a  more verbose syntax and write something like:

```csharp
class ConnectionString
{
    public string Value { get; }

    public ConnectionString(string value)
    {
        Value = value;
    }
}

class Foo
{
    public Foo(Bar bar, ConnectionString connectionString) { }
}

builder.RegisterInstance(new ConnectionString("some value"));
builder.RegisterType<Foo>();
builder.RegisterType<Bar>();
```


Hopefully, C# 6 Primary Constructor shall reduce the glue code, allowing to replace definitions such as

```csharp
class BarServiceAuthParameters
{
    string Url { get; }
    string Username { get; }
    string AccessToken { get; }

    public BarServiceAuthParameters(string url, string username, string accessToken)
    {
        Url = url;
        Username = username;
        AccessToken = accessToken;
    }
}

```

with

```csharp

class BarServiceAuthParameters(string url, string username, string accessToken) { }
```

which is much more terse and elegant.


### `implicit` and `explicit` to the resque
There's another path to explore: it would be nice if it were possible to implicitly cast it from and to strings.

Actually, that's not too hard to achieve. There is a technique Jimmy Bogard brillantly exposed in his post [Dealing with primitive obsession](https://lostechies.com/jimmybogard/2007/12/03/dealing-with-primitive-obsession) that  makes a smart use of the cast operators `implicit` and `explicit` and allows to make you consume and create your Value Objects as they are primitives.

Go and read the post. You will learn that by defining your Value Object as

```csharp
public class ConnectionString
{
    public string Value { get; }

    public ConnectionString(string value)
    {
        Value = value;
    }

    public static implicit operator string(ConnectionString connectionString)
    {
        return connectionString.Value;
    }

    public static explicit operator ConnectionString(string value)
    {
        return new ConnectionString(value);
    }
}
```

you will be able to consume and create it as a primitive, as in the following example:

```csharp

class Foo
{
    public ConnectionString Conn;
}


var foo = new Foo();
foo.Conn = (ConnectionString) "barbaz";

string s = foo.Conn;
```