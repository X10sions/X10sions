using LinqToDB.Data;
using System;
using System.Diagnostics;

namespace LinqToDB.Tests.Base {
  public class DisableLogging : IDisposable {
    private TraceSwitch _traceSwitch;

    public DisableLogging() {
      _traceSwitch = DataConnection.TraceSwitch;
      DataConnection.TurnTraceSwitchOn(TraceLevel.Off);
    }

    public void Dispose() => DataConnection.TraceSwitch = _traceSwitch;
  }

  //  static class DataCache<T>
  //    where T : class {
  //    static readonly Dictionary<string, List<T>> _dic = new Dictionary<string, List<T>>();
  //    public static List<T> Get(string context) {
  //      lock (_dic) {
  //        context = context.Replace(".LinqService", "");

  //        if (!_dic.TryGetValue(context, out var list)) {
  //          using (new DisableLogging())
  //          using (var db = new DataConnection(context)) {
  //            list = db.GetTable<T>().ToList();
  //            _dic.Add(context, list);
  //          }
  //        }

  //        return list;
  //      }
  //    }

  //    public static void Clear() => _dic.Clear();
  //  }

  //  public static class Helpers {

  //    public static string ToInvariantString<T>(this T data) => string.Format(CultureInfo.InvariantCulture, "{0}", data).Replace(',', '.').Trim(' ', '.', '0');

  //  }

  //  public class AllowMultipleQuery : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.AllowMultipleQuery;

  //    public AllowMultipleQuery(bool value = true) {
  //      Common.Configuration.Linq.AllowMultipleQuery = value;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.AllowMultipleQuery = _oldValue;
  //  }

  //  public class GuardGrouping : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.GuardGrouping;

  //    public GuardGrouping(bool enable) {
  //      Common.Configuration.Linq.GuardGrouping = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.GuardGrouping = _oldValue;
  //  }

  //  public class ParameterizeTakeSkip : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.ParameterizeTakeSkip;

  //    public ParameterizeTakeSkip(bool enable) {
  //      Common.Configuration.Linq.ParameterizeTakeSkip = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.ParameterizeTakeSkip = _oldValue;
  //  }

  //  public class PreloadGroups : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.PreloadGroups;

  //    public PreloadGroups(bool enable) {
  //      Common.Configuration.Linq.PreloadGroups = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.PreloadGroups = _oldValue;
  //  }

  //  public class GenerateExpressionTest : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.GenerateExpressionTest;

  //    public GenerateExpressionTest(bool enable) {
  //      Common.Configuration.Linq.GenerateExpressionTest = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.GenerateExpressionTest = _oldValue;
  //  }

  //  public class DoNotClearOrderBys : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.DoNotClearOrderBys;

  //    public DoNotClearOrderBys(bool enable) {
  //      Common.Configuration.Linq.DoNotClearOrderBys = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.DoNotClearOrderBys = _oldValue;
  //  }

  //  public class UseBinaryAggregateExpression : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.UseBinaryAggregateExpression;

  //    public UseBinaryAggregateExpression(bool enable) {
  //      Common.Configuration.Linq.UseBinaryAggregateExpression = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.UseBinaryAggregateExpression = _oldValue;
  //  }

  //  public class GenerateFinalAliases : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Sql.GenerateFinalAliases;

  //    public GenerateFinalAliases(bool enable) {
  //      Common.Configuration.Sql.GenerateFinalAliases = enable;
  //    }

  //    public void Dispose() => Common.Configuration.Sql.GenerateFinalAliases = _oldValue;
  //  }

  //  public class SerializeAssemblyQualifiedName : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.LinqService.SerializeAssemblyQualifiedName;

  //    public SerializeAssemblyQualifiedName(bool enable) {
  //      Common.Configuration.LinqService.SerializeAssemblyQualifiedName = enable;
  //    }

  //    public void Dispose() => Common.Configuration.LinqService.SerializeAssemblyQualifiedName = _oldValue;
  //  }



  //  public class DisableQueryCache : IDisposable {
  //    private readonly bool _oldValue = Common.Configuration.Linq.DisableQueryCache;

  //    public DisableQueryCache(bool value = true) {
  //      Common.Configuration.Linq.DisableQueryCache = value;
  //    }

  //    public void Dispose() => Common.Configuration.Linq.DisableQueryCache = _oldValue;
  //  }

  //  public class WithoutJoinOptimization : IDisposable {
  //    public WithoutJoinOptimization(bool opimizerSwitch = false) {
  //      Common.Configuration.Linq.OptimizeJoins = opimizerSwitch;
  //      Query.ClearCaches();
  //    }

  //    public void Dispose() => Common.Configuration.Linq.OptimizeJoins = true;
  //  }

  //  public class DeletePerson : IDisposable {
  //    readonly IDataContext _db;

  //    public DeletePerson(IDataContext db) {
  //      _db = db;
  //      Delete(_db);
  //    }

  //    public void Dispose() => Delete(_db);

  //    readonly Func<IDataContext, int> Delete =
  //      CompiledQuery.Compile<IDataContext, int>(db => db.GetTable<Person>().Delete(_ => _.ID > TestBase.MaxPersonID));

  //  }

  //  public class WithoutComparisonNullCheck : IDisposable {
  //    public WithoutComparisonNullCheck() {

  //      LinqToDB.Common.Configuration.Linq.CompareNullsAsValues = false;
  //    }

  //    public void Dispose() {
  //      Common.Configuration.Linq.CompareNullsAsValues = true;
  //      Query.ClearCaches();
  //    }
  //  }
}