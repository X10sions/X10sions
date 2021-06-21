using Bootstrap.Css.Enums;
namespace Bootstrap {
  public interface ITableOptions {
    public bool InitBootstrap { get; set; }
    public TableBorder? Border { get; set; }
    public ContextualClass? ContextualClass { get; set; }
    public bool Hover { get; set; }
    public TableResponsiveSize? ResponsiveSize { get; set; }
    public TableSize? Size { get; set; }
    public bool Striped { get; set; }
  }

  public static class ITableOptionsExtensions {

    public static string ToCssString(this ITableOptions options) => options.InitBootstrap ? ("table"
      + options.Border.ToCssString(" ")
      + options.ContextualClass.ToCssString(" ")
      + (options.Hover ? " table-hover" : string.Empty)
      + options.ResponsiveSize.ToCssString(" ")
      + options.Size.ToCssString(" ")
      + (options.Striped ? " table-striped" : string.Empty)
      ).ToLower() : string.Empty;
  }

}