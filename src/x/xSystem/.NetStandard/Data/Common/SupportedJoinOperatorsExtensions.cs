namespace System.Data.Common;
public static class SupportedJoinOperatorsExtensions {

  public static bool HasInner(this SupportedJoinOperators supportedJoinOperators) => supportedJoinOperators.HasFlag(SupportedJoinOperators.Inner);
  public static bool HasLeftOuter(this SupportedJoinOperators supportedJoinOperators) => supportedJoinOperators.HasFlag(SupportedJoinOperators.LeftOuter);
  public static bool HasRightOuter(this SupportedJoinOperators supportedJoinOperators) => supportedJoinOperators.HasFlag(SupportedJoinOperators.RightOuter);
  public static bool HasFullOuter(this SupportedJoinOperators supportedJoinOperators) => supportedJoinOperators.HasFlag(SupportedJoinOperators.FullOuter);

}