namespace Bootstrap.Css.Enums{
public enum TableSize {
    sm,
    lg
  }

  public static class TableSizeExtensions {

    public static string ToCssString(this TableSize? size, string prefix = "", string suffix = "") => size == null ? string.Empty : $"{prefix}table-{size}{suffix}".ToLower();

  }
}