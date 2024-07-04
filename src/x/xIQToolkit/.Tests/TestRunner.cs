using System.Reflection;

namespace Test;
public class TestRunner {
  private string[] commandLineArgs;
  private List<Assembly> testAssemblies;
  private List<string> testNames;
  private bool verbose;

  public TestRunner(string[] commandLineArgs, params Assembly[] testAssemblies) {
    this.commandLineArgs = commandLineArgs.Select(a => a.Trim()).ToArray();
    this.testAssemblies = new List<Assembly>(testAssemblies);
    testNames = new List<string>();
    ParseCommandLineArgs();
  }

  public void RunTests() {
    var testTypes = GetTestTypes();
    if (testNames.Count == 0) {
      RunAllTests(testTypes);
    } else {
      var testTypeMap = testTypes.ToDictionary(t => t.Name, t => t);
      var testMethodLookup = testTypes.SelectMany(t => GetTestMethods(t).Select(m => new { Type = t, Method = m }))
                                      .ToLookup(x => x.Method.Name, x => x);

      foreach (var name in testNames) {
        var testName = name.EndsWith("Tests") ? name : name + "Tests";
        Type testType;
        if (testTypeMap.TryGetValue(testName, out testType)) {
          RunAllTests(testType, GetTestMethods(testType));
          continue;
        }

        var methodName = name.StartsWith("Test") ? name : "Test" + name;
        foreach (var methods in testMethodLookup[methodName].GroupBy(x => x.Type, x => x.Method)) {
          RunAllTests(methods.Key, methods);
        }
      }
    }
  }

  private void ParseCommandLineArgs() {
    foreach (var arg in commandLineArgs) {
      if (arg.Length > 0 && arg[0] == '-') {
        var parts = arg.Substring(1).Split(':');
        if (parts.Length > 0) {
          switch (parts[0]) {
            case "v":
            case "verbose":
              if (parts.Length > 1) {
                if (parts[1] == "-" || parts[1] == "false") {
                  verbose = false;
                } else if (parts[1] == "+" || parts[1] == "true") {
                  verbose = true;
                }
              } else {
                verbose = true;
              }
              break;
          }
        }
      } else {
        testNames.Add(arg);
      }
    }
  }

  private Type[] GetTestTypes() => testAssemblies.SelectMany(
        a => a.GetTypes()
            .Where(t => t.Name.EndsWith("Tests") && (t.IsPublic || t.IsNestedPublic) && t.IsClass && !t.IsAbstract && HasDefaultConstructor(t))
        ).ToArray();

  private static bool HasDefaultConstructor(Type type) => type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
        .FirstOrDefault(c => c.GetParameters().Length == 0) != null;

  private static MethodInfo[] GetTestMethods(Type testType) => testType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        .Where(m => m.Name.StartsWith("Test") && m.ReturnType == typeof(void) && m.GetParameters().Length == 0)
        .ToArray();

  private void RunAllTests(IEnumerable<Type> testTypes) {
    foreach (var testType in testTypes) {
      RunAllTests(testType, GetTestMethods(testType));
    }
  }

  private void RunAllTests(Type testType, IEnumerable<MethodInfo> testMethods) => new TestTypeRunner(this, testType, testMethods).RunTests();

  private class TestTypeRunner {
    private TestRunner runner;
    private Type testType;
    private IEnumerable<MethodInfo> testMethods;
    private object testIntance;

    private MethodInfo[] allMethods;
    private Func<MethodInfo, bool> canRunTest;
    private Action<Action> runTest;
    private Action<string[]> setup;
    private Action teardown;

    private int passed;
    private int failed;
    private int skipped;

    public TestTypeRunner(TestRunner runner, Type testType, IEnumerable<MethodInfo> testMethods) {
      this.runner = runner;
      this.testType = testType;
      this.testMethods = testMethods;

      testIntance = Activator.CreateInstance(testType);
      allMethods = testType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

      canRunTest = GetTestFunc<MethodInfo, bool>("CanRunTest");
      runTest = GetTestAction<Action>("RunTest");
      setup = GetTestAction<string[]>("Setup");
      teardown = GetTestAction("Teardown");
    }

    public void RunTests() {
      Console.WriteLine(testType.Name);

      Setup();

      try {
        foreach (var testMethod in testMethods) {
          RunTest(testMethod);
        }
      } finally {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("{0} Passed", passed);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("  {0} Skipped", skipped);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  {0} Failed", failed);
        Console.ForegroundColor = color;
        Console.WriteLine();

        Teardown();
      }
    }

    private void Setup() => setup?.Invoke(runner.commandLineArgs);

    private void Teardown() => teardown?.Invoke();

    private bool CanRunTest(MethodInfo testMethod) {
      if (canRunTest != null) {
        return canRunTest(testMethod);
      } else {
        return true;
      }
    }

    private void RunTest(Action testAction) {
      if (runTest != null) {
        runTest(testAction);
      } else {
        testAction();
      }
    }

    private void RunTest(MethodInfo testMethod) {
      var color = Console.ForegroundColor;

      try {
        if (CanRunTest(testMethod)) {
          if (runner.verbose) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Running {0}", testMethod.Name);
            Console.ForegroundColor = color;
          }

          var testAction = GetTestAction(testMethod.Name);
          RunTest(testAction);

          passed++;

          if (runner.verbose) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  {0} Succeeded", testMethod.Name);
          }
        } else {
          skipped++;

          if (runner.verbose) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  {0} Skipped", testMethod.Name);
          }
        }
      } catch (Exception e) {
        failed++;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  {0} Failed", testMethod.Name);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(e.Message);
        Console.ForegroundColor = color;

        if (runner.verbose) {
          Console.WriteLine(e.StackTrace);
        }
      } finally {
        Console.ForegroundColor = color;

        if (runner.verbose) {
          Console.WriteLine();
        }
      }
    }

    private Func<TResult> GetTestFunc<TResult>(string name) => GetTestDelegate<Func<TResult>>(name, typeof(TResult));

    private Func<TParam, TResult> GetTestFunc<TParam, TResult>(string name) => GetTestDelegate<Func<TParam, TResult>>(name, typeof(TResult), typeof(TParam));

    private Action GetTestAction(string name) => GetTestDelegate<Action>(name, typeof(void));

    private Action<TParam> GetTestAction<TParam>(string name) => GetTestDelegate<Action<TParam>>(name, typeof(void), typeof(TParam));

    private TDelegate GetTestDelegate<TDelegate>(string name, Type returnType, params Type[] parameterTypes)
        where TDelegate : class {
      var method = allMethods.FirstOrDefault(m => IsMatchingMethod(m, name, returnType, parameterTypes));
      if (method != null) {
        return (TDelegate)(object)Delegate.CreateDelegate(typeof(TDelegate), testIntance, method);
      } else {
        return null;
      }
    }

    private static bool IsMatchingMethod(MethodInfo method, string name, Type returnType, Type[] parameterTypes) {
      if (method.Name != name || method.ReturnType != returnType) {
        return false;
      }
      var parameters = method.GetParameters();
      if (parameters.Length != parameterTypes.Length) {
        return false;
      }
      for (var i = 0; i < parameters.Length; i++) {
        if (parameters[i].ParameterType != parameterTypes[i]) {
          return false;
        }
      }
      return true;
    }
  }
}