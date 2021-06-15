using System;

namespace BBaithwaite {
  public class DynamicQueryResult {
    public DynamicQueryResult(string sql, object param) {
      _result = new Tuple<string, object>(sql, param);
    }

    readonly Tuple<string, object> _result;

    public string Sql {
      get {
        return _result.Item1;
      }
    }

    public object Param {
      get {
        return _result.Item2;
      }
    }
  }
}
