namespace Common.Html.Tags {
  public interface ISelect {
    bool? AutoFocus { get; set; }
    Form Form { get; set; }
    bool? Required { get; set; }

    bool? Disabled { get; set; }
    bool? Multiple { get; set; }
    string Name { get; set; }
    int Size { get; set; }
  }
}
