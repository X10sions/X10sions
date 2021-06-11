namespace Common.VisualStudio.ProjectVersion {
  public class AutoVersionSettings {
    public BumpType BumpTypeVersionPart1 { get; set; }
    public BumpType BumpTypeVersionPart2 { get; set; }
    public BumpType BumpTypeVersionPart3 { get; set; }
    public BumpType BumpTypeVersionPart4 { get; set; }
    public BumpType BumpTypeVersionSuffix { get; set; }

    public int NewVersionPart1 { get; set; }
    public int NewVersionPart2 { get; set; }
    public int NewVersionPart3 { get; set; }
    public int? NewVersionPart4 { get; set; }

    //public Func<int> FuncNewVersionPart3 = () => newVersionDate.YYMM();
    //public Func<int> FuncNewVersionPart4 = () => newVersionDate.ddHH();
    //public Func<int> FuncNewVersionSuffix = () => newVersionDate.mmss();

    //public string NewVersionSuffix { get; set; }
  }
}