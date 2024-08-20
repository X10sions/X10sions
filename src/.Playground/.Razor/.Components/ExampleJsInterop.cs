using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace X10sions.Playground.Razor;
// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.
public class ExampleJsInterop : IAsyncDisposable {
  private readonly AsyncLazy<IJSObjectReference> moduleTask;
  //private readonly Lazy<Task<IJSObjectReference>> moduleTask;

  public ExampleJsInterop(IJSRuntime jsRuntime) {
    moduleTask = new(async () => await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/SharedRazor.RCL/exampleJsInterop.js"));
    //moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/SharedRazor.RCL/exampleJsInterop.js").AsTask());
  }

  public async ValueTask<string> Prompt(string message) {
    var module = await moduleTask.Value;
    return await module.InvokeAsync<string>("showPrompt", message);
  }

  public async ValueTask DisposeAsync() {
    if (moduleTask.IsValueCreated) {
      var module = await moduleTask.Value;
      await module.DisposeAsync();
    }
  }
}

/// <summary> https://devblogs.microsoft.com/pfxteam/asynclazyt/ </summary>
public class AsyncLazy<T> : Lazy<Task<T>> {
  public AsyncLazy(Func<T> valueFactory) : base(() => Task.Factory.StartNew(valueFactory)) { }
  public AsyncLazy(Func<Task<T>> taskFactory) : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }
  public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
}