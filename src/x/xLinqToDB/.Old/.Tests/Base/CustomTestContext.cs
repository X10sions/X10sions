using System.Collections.Generic;
using System.Threading;

namespace LinqToDB.Tests.Base {
  class CustomTestContext {
    public static string TRACE = "key-trace";
    public static string LIMITED = "key-limited";

    private static readonly AsyncLocal<CustomTestContext?> _context = new AsyncLocal<CustomTestContext?>();

    private readonly IDictionary<string, object?> _state = new Dictionary<string, object?>();

    public static CustomTestContext Create() => _context.Value = new CustomTestContext();
    public static CustomTestContext Get() => _context.Value ?? Create();
    public static void Release() => _context.Value = null;
    public TValue Get<TValue>(string name) => (_state.ContainsKey(name)) ? (TValue)_state[name]! : default!;
    public void Set<TValue>(string name, TValue value) => _state[name] = value;

  }

}