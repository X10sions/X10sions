namespace Bootstrap.Css.Enums {
  public enum TableResponsiveSize {
    auto,
    // < 576px
    sm, 
    // < 768px
    md,
    // < 992px
    lg,
    // < 1200px
    xl
  }

  public static class TableResponsiveSizeExtensions {

    public static string ToCssString(this TableResponsiveSize? size, string prefix = "", string suffix = "") => size == null ? string.Empty : $"{prefix}table-responsive-{size}{suffix}".ToLower();

  }

}