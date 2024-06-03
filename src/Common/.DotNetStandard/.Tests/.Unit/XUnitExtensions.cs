using System.Collections.Generic;

namespace Xunit.Extensions {
  public static class TheoryDataItem {

    public static TheoryData<T> ToTheoryData<T>(this IEnumerable<T> src) {
      // https://github.com/zspitz/ANTLR4ParseTreeVisualizer/blob/fe5f475f91cd922d944563ca844b79c84967e654/Tests.Shared/Extensions.cs
      var ret = new TheoryData<T>();
      foreach (var a in src) {
        ret.Add(a);
      }
      return ret;
    }

    public static TheoryData<T1, T2> ToTheoryData<T1, T2>(this IEnumerable<(T1, T2)> src) {
      // https://github.com/zspitz/ANTLR4ParseTreeVisualizer/blob/fe5f475f91cd922d944563ca844b79c84967e654/Tests.Shared/Extensions.cs
      var ret = new TheoryData<T1, T2>();
      foreach (var (a, b) in src) {
        ret.Add(a, b);
      }
      return ret;
    }

    public static TheoryData<T1, T2, T3> ToTheoryData<T1, T2, T3>(this IEnumerable<(T1, T2, T3)> src) {
      // https://github.com/zspitz/ANTLR4ParseTreeVisualizer/blob/fe5f475f91cd922d944563ca844b79c84967e654/Tests.Shared/Extensions.cs
      var ret = new TheoryData<T1, T2, T3>();
      foreach (var (a, b, c) in src) {
        ret.Add(a, b, c);
      }
      return ret;
    }

    public static TheoryData<T1, T2, T3, T4> ToTheoryData<T1, T2, T3, T4>(this IEnumerable<(T1, T2, T3, T4)> src) {
      // https://github.com/zspitz/ANTLR4ParseTreeVisualizer/blob/fe5f475f91cd922d944563ca844b79c84967e654/Tests.Shared/Extensions.cs
      var ret = new TheoryData<T1, T2, T3, T4>();
      foreach (var (a, b, c, d) in src) {
        ret.Add(a, b, c, d);
      }
      return ret;
    }

  }
}