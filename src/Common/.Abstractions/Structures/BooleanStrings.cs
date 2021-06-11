using System;
using System.Collections.Generic;
using System.Linq;
namespace Common.Structures {
  public class BooleanStrings : Dictionary<string, bool> {
    public string[] FalseStrings => new[] { "f", "false", "n", "no", "off", "0" };
    public string[] TrueStrings => new[] { "t", "true", "y", "yes", "on", "1" };
    public static BooleanStrings Instance = new BooleanStrings();
    public BooleanStrings() {
      foreach (var s in FalseStrings) { Add(s, false); }
      foreach (var s in TrueStrings) { Add(s, true); }
    }
    public string JoinedFalseStrings(string separator = ", ") => string.Join(separator, FalseStrings);
    public string JoinedTrueStrings(string separator = ", ") => string.Join(separator, TrueStrings);
    public string ToTrueString(string s, string[] trueStrings = null) => (trueStrings ?? TrueStrings).Contains(s, StringComparer.OrdinalIgnoreCase) ? bool.TrueString : null;
    public string ToFalseString(string s, string[] falseStrings = null) => (falseStrings ?? FalseStrings).Contains(s, StringComparer.OrdinalIgnoreCase) ? bool.FalseString : null;
    public bool? ToBool(string s, string[] trueStrings = null, string[] falseStrings = null) => ToTrueString(s, trueStrings) == null ? (ToFalseString(s, falseStrings) == null ? (bool?)null : false) : true;

  }
}