namespace Common.VisualStudio.ProjectVersion {
  public enum BumpType {
    SameValue,
    Bump,
    DateTime_YYMM,
    DateTime_ddHH,
    DateTime_mmss,
    //DateTime_UtcYYMM,
    //DateTime_UtcddHH,
    //DateTime_Utcmmss,
    Reset,
    CustomValue
  }
}