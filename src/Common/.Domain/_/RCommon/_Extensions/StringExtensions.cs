using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RCommon;

public static class StringExtensions {
  private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
  private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

  [DebuggerStepThrough]
  public static string FormatWith(this string target, params object[] args) {
    Guard.IsNotEmpty(target, "target");
    return string.Format(Constants.CurrentCulture, target, args);
  }

  [DebuggerStepThrough]
  public static bool IsEmail(this string target) => !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);

  [DebuggerStepThrough]
  public static bool IsWebUrl(this string target) => !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);


}
