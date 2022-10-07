namespace X10sions.Wsus.Web {
  public class AppSettings {

    public _ConnectionStrings ConnectionStrings { get; set; } = new _ConnectionStrings();

    public class _ConnectionStrings : Dictionary<string, string> {
      public string SUSDB { get => this[nameof(SUSDB)]; set => this[nameof(SUSDB)] = value; }
    }

  }
}
