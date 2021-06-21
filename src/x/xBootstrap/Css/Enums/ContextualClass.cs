namespace Bootstrap.Css.Enums {
  public enum ContextualClass {
    Default,
    Active,
    Primary,
    Seconndary,

    Success,
    Danger,
    Info,
    Warning,

    Light,
    Dark
  }

  public static class ContextualClassExtensions {

    public static string ToCssString(this ContextualClass? context, string prefix = "",  string suffix= "") => context == null ? string.Empty : $"{ prefix}{context}{suffix}".ToLower();
    public static string ToCssStringTable(this ContextualClass? context) => context.ToCssString(" table-");
  }

}