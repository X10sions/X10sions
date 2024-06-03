using IQToolkit.Data;
using IQToolkit.Data.Ado;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Test {

  public abstract class QueryTestBase {
    private bool executeQueries;
    private Dictionary<string, string> baselines;
    private XmlTextWriter baselineWriter;
    private string baselineKey;
    private string queryText;
    private DbEntityProvider provider;

    public DbEntityProvider GetProvider() => provider;

    protected abstract DbEntityProvider CreateProvider();

    public virtual void Setup(string[] args) {
      provider = CreateProvider();
      provider.Connection.Open();

      if (args.Any(a => a == "-log")) {
        provider.Log = Console.Out;
      }

      executeQueries = ExecuteQueries();

      var baseLineFilePath = GetBaseLineFilePath();
      var newBaseLineFilePath = baseLineFilePath != null ? baseLineFilePath + ".new" : null;

      if (!string.IsNullOrEmpty(baseLineFilePath)) {
        ReadBaselines(baseLineFilePath);
      }

      if (!string.IsNullOrEmpty(newBaseLineFilePath)) {
        baselineWriter = new XmlTextWriter(newBaseLineFilePath, Encoding.UTF8) {
          Formatting = Formatting.Indented,
          Indentation = 2
        };
        baselineWriter.WriteStartDocument();
        baselineWriter.WriteStartElement("baselines");
      }
    }

    public virtual string GetBaseLineFilePath() => null;

    public virtual bool ExecuteQueries() => false;

    private void ReadBaselines(string filename) {
      if (!string.IsNullOrEmpty(filename) && File.Exists(filename)) {
        var doc = XDocument.Load(filename);
        baselines = doc.Root.Elements("baseline").ToDictionary(e => (string)e.Attribute("key"), e => e.Value);
      }
    }

    public virtual void Teardown() {
      if (provider != null) {
        provider.Connection.Close();
      }

      if (baselineWriter != null) {
        baselineWriter.Flush();
        baselineWriter.Close();
      }
    }

    public virtual bool CanRunTest(MethodInfo testMethod) {
      var exclusions = (ExcludeProvider[])testMethod.GetCustomAttributes(typeof(ExcludeProvider), true);
      foreach (var exclude in exclusions) {
        if (
            // actual name of the provider type
            string.Compare(provider.GetType().Name, exclude.Provider, StringComparison.OrdinalIgnoreCase) == 0
            // prefix of the provider type xxxQueryProvider
            || string.Compare(provider.GetType().Name, exclude.Provider + "QueryProvider", StringComparison.OrdinalIgnoreCase) == 0
            // last name of the namespace
            || string.Compare(provider.GetType().Namespace.Split(new[] { '.' }).Last(), exclude.Provider, StringComparison.OrdinalIgnoreCase) == 0
            ) {
          return false;
        }
      }

      return true;
    }

    public virtual void RunTest(Action testAction) {
      baselineKey = testAction.Method.Name;

      try {
        testAction();
      } catch (Exception e) {
        if (queryText != null) {
          throw new TestFailureException(e.Message + "\r\n\r\n" + queryText);
        } else {
          throw;
        }
      }
    }

    protected void TestQuery(IQueryable query) => TestQuery((EntityProvider)query.Provider, query.Expression, false);

    protected void TestQuery(Expression<Func<object>> query) => TestQuery(provider, query.Body, false);

    protected void TestQueryFails(IQueryable query) => TestQuery((EntityProvider)query.Provider, query.Expression, true);

    protected void TestQueryFails(Expression<Func<object>> query) => TestQuery(provider, query.Body, true);

    protected void TestQuery(EntityProvider pro, Expression query, bool expectedToFail) {
      if (query.NodeType == ExpressionType.Convert && query.Type == typeof(object)) {
        query = ((UnaryExpression)query).Operand; // remove box
      }

      queryText = null;
      queryText = pro.GetQueryText(query);
      WriteBaseline(baselineKey, queryText);

      if (executeQueries) {
        Exception caught = null;
        try {
          var result = pro.Execute(query);
          var seq = result as IEnumerable;
          if (seq != null) {
            // iterate results
            foreach (var item in seq) {
            }
          } else {
            var disposable = result as IDisposable;
            if (disposable != null)
              disposable.Dispose();
          }
        } catch (Exception e) {
          caught = e;

          if (!expectedToFail) {
            throw new TestFailureException(e.Message + "\r\n\r\n" + queryText);
          }
        }

        if (caught == null && expectedToFail) {
          throw new InvalidOperationException("Query succeeded when expected to fail");
        }
      }

      string baseline = null;
      if (baselines != null && baselines.TryGetValue(baselineKey, out baseline)) {
        var trimAct = TrimExtraWhiteSpace(queryText).Trim();
        var trimBase = TrimExtraWhiteSpace(baseline).Trim();
        if (trimAct != trimBase) {
          throw new InvalidOperationException(string.Format("Query translation does not match baseline:\r\n    Expected: {0}\r\n    Actual  : {1}", trimBase, trimAct));
        }
      }

      if (baseline == null && baselines != null) {
        throw new InvalidOperationException("No baseline");
      }
    }

    private void WriteBaseline(string key, string text) {
      if (baselineWriter != null) {
        baselineWriter.WriteStartElement("baseline");
        baselineWriter.WriteAttributeString("key", key);
        baselineWriter.WriteWhitespace("\r\n");
        baselineWriter.WriteString(text);
        baselineWriter.WriteEndElement();
      }
    }

    private string TrimExtraWhiteSpace(string s) {
      var sb = new StringBuilder();
      var lastWasWhiteSpace = false;
      foreach (var c in s) {
        var isWS = char.IsWhiteSpace(c);
        if (!isWS || !lastWasWhiteSpace) {
          if (isWS)
            sb.Append(' ');
          else
            sb.Append(c);
          lastWasWhiteSpace = isWS;
        }
      }
      return sb.ToString();
    }

    private void WriteDifferences(string s1, string s2) {
      var start = 0;
      var same = true;
      for (int i = 0, n = Math.Min(s1.Length, s2.Length); i < n; i++) {
        var matches = s1[i] == s2[i];
        if (matches != same) {
          if (i > start) {
            Console.ForegroundColor = same ? ConsoleColor.Gray : ConsoleColor.White;
            Console.Write(s1.Substring(start, i - start));
          }
          start = i;
          same = matches;
        }
      }
      if (start < s1.Length) {
        Console.ForegroundColor = same ? ConsoleColor.Gray : ConsoleColor.White;
        Console.Write(s1.Substring(start));
      }
      Console.WriteLine();
    }

    protected bool ExecSilent(string commandText) {
      try {
        provider.ExecuteCommand(commandText);
        return true;
      } catch (Exception e) {
        var msg = e.Message;
        return false;
      }
    }
  }
}
