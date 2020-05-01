[![Patterns.PipesAndFilters](https://badge.fury.io/nu/Patterns.PipesAndFilters.svg)](https://badge.fury.io/nu/Patterns.PipesAndFilters)
[![Github Actions](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Ftoddmeinershagen%2Fpipes-and-filters%2Fbadge&label=build&logo=none)](https://actions-badge.atrox.dev/toddmeinershagen/pipes-and-filters/goto?ref=master)

# pipes-and-filters
This is a base library to implement an in-process, asynchronous implementation of the pipes and filters pattern in .NET Core.  

The pipes and filters pattern is normally a distributed messaging pattern, but it can also be used to split up a lengthy in-process method into smaller, single responsibility pieces of logic that mutate a given input and return it as output.  Each filter added to the pipeline further mutates the output of one filter and passes it on to the next. 

Below is an example of an asynchronous filter.  Note - you can either implement the ```IAsyncFilter<T>``` or use the ```AsyncFilterBase<T>``` as shown below.

```csharp
public class Pause : AsyncFilterBase<string>
{
    private readonly TimeSpan _timeToPause;

    public Pause(TimeSpan timeToPause)
    {
        _timeToPause = timeToPause;
    }

    protected override async Task<string> OnExecuteAsync(string input, CancellationToken cancellationToken)
    {
        await Task.Delay(_timeToPause, cancellationToken);
        return input;
    }
}
```

Below is an example of a synchronous filter.  Note - you should derive from the ```SynchronousFilterBase<T>``` as shown below which wraps your logic in an asynchronous execution.

```csharp
public class Append : SyncFilterBase<string>
{
    private readonly string _text;

    public Append(string text)
    {
        _text = text;
    }

    protected override string OnExecute(string input)
    {
        return input += _text;
    }
}
```

To create a pipeline you can call the ```Add(IAsyncFilter filter)``` method with an instance that you create.

```csharp
[Test]
public async Task given_filters_when_executing_then_should_execute_in_order()
{
    var input = "one";
    var pipeline = new Pipeline<string>();

    pipeline
        .Add(new Append(", two"))
        .Add(new Append(", three"));
    var result = await pipeline.ExecuteAsync(input);

    result.Should().Be("one, two, three");
}
```

Or, you can add filters by using the generic version of ```Add<TFilter>``` where ```TFilter``` is to be a type of ```IAsyncFilter<T>```.  This version uses either the default instance creator, ```Activator.CreateInstance(Type type)``` or you can provide your own ```IServiceProvider``` to define how that type should be created.

```csharp
[Test]
public async Task given_no_service_provider_and_filter_added_as_type_when_executing_then_should_use_default_service_provider()
{
    var input = "one";
    var pipeline = new Pipeline<string>();
    pipeline.Add<Reverse>();

    var result = await pipeline.ExecuteAsync(input);

    result.Should().Be("eno");
}
```

And lastly, Pipelines are also an asynchronous Filter, so you can add them like any other Filter to another Pipeline.

```csharp
[Test]
public async Task given_pipeline_as_filter_and_additional_filters_when_executing_then_should_return_results()
{
    var input = "";

    var numbers = new Pipeline<string>()
        .Add(new Append("1"))
        .Add(new Pause(TimeSpan.FromMilliseconds(25)))
        .Add(new Append("2"))
        .Add(new Append("3"));

    var result = await new Pipeline<string>()
        .Add(numbers)
        .Add<Reverse>()
        .ExecuteAsync(input);

    result.Should().Be("321");
}
```