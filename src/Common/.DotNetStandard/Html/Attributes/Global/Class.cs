namespace Common.Html.Attributes {
  public interface IClassHtmlAttribute : IHtmlAttribute<HashSet<string>> { };

  public static class IClassHtmlAttributeExtensions {

    public static IClassHtmlAttribute Add(this IClassHtmlAttribute attr, params string[] cssClasses) {
      attr ??= new ClassHtmlAttribute(new HashSet<string>());
      if (cssClasses is not null) {
        foreach (var cssClass in cssClasses.Where(x => !string.IsNullOrWhiteSpace(x))) {
          //var split = cssClass.Split(new[] { ClassHtmlAttribute.CssClassDelimeter }, System.StringSplitOptions.RemoveEmptyEntries);
          //if (split.Length > 1) {
          //  attr.Add(split);
          //} else if(split.Length == 1) {
          attr.Value.Add(cssClass);
          //}
        }
      }
      return attr;
    }

    public static IClassHtmlAttribute Remove(this IClassHtmlAttribute attr, params string[] cssClasses) {
      attr ??= new ClassHtmlAttribute(new HashSet<string>());
      if (cssClasses is not null) {
        foreach (var cssClass in cssClasses.Where(x => !string.IsNullOrWhiteSpace(x))) {
          //var split = cssClass.Split(new[] { ClassHtmlAttribute.CssClassDelimeter }, System.StringSplitOptions.RemoveEmptyEntries);
          //if (split.Length > 1) {
          //  attr.Add(split);
          //} else if(split.Length == 1) {
          attr.Value.Remove(cssClass);
          //}
        }
      }
      return attr;
    }

    //public void ClassRemove(string cssClass) => classList.Remove(cssClass);
    //public void ClassSet(string cssClass) => classList.Add(cssClass);
  }

  /// <summary>Specifies one Or more classnames For an element (refers To a Class In a style sheet)</summary>
  public readonly record struct ClassHtmlAttribute : IClassHtmlAttribute {
    public ClassHtmlAttribute(HashSet<string> value) {
      Value = value ?? new HashSet<string>();
    }
    public HashSet<string> Value { get; }
    public const string CssClassDelimeter = " ";
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.@class;
  }

}