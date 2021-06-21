namespace Bootstrap.Css.Enums {
  public enum TableBorder {
    bordered,
    borderless
  }

  public static class TableBorderExtensions {

    public static string ToCssString(this TableBorder? border, string prefix = "" ,string suffix = "")
      => border == null ? string.Empty : $"{prefix}table-{border}{suffix}".ToLower();

  }
}