using System.Text;

namespace Common.Html.Tags;

public class CssSelector {
  public const string Terminator = ";";
  //public const string xDelimeter = ";";

  CssSelector(string selector) {
    Selector = selector;
  }

  public CssSelector(string selector, HashSet<Declaration> declarations) : this(selector) {
    foreach (var d in declarations) {
      Declarations.Add(d);
    }
  }

  public CssSelector(string selector, string declaration) : this(selector) {
    foreach (var d in declaration.Split(';')) {
      var kvp = d.Split(':');
      Declarations.Add(new Declaration(kvp[0], kvp[1]));
    }
  }

  public string Selector { get; }
  public HashSet<Declaration> Declarations { get; } = new HashSet<Declaration>();

  public string ToHtml() {
    var sb = new StringBuilder();
    sb.Append($"{Selector}{{");
    foreach (var p in Declarations) {
      sb.Append(p.ToHtml());
    }
    sb.Append($"}}");
    return sb.ToString();
  }

  public class Declaration {
    public const string Separator = ":";

    public Declaration(string property, string value) {
      Property = property.Trim();
      Value = value.Trim();
    }

    public Declaration(string keyValue) {
      var a = keyValue.Split(Separator);
      Property = a[0].Trim();
      Value = a[1].Trim();
    }

    public string Property { get; set; }
    public string Value { get; set; }

    public string ToHtml() => Property + Separator + Value + CssSelector.Terminator;

    //public static string DeclarationsToHtml(IEnumerable<StyleDeclaration> declarations) {
    //  var sb = new StringBuilder();
    //  foreach (var p in declarations) {
    //    sb.Append(p.ToHtml());
    //  }
    //  return sb.ToString();
    //}

  }

}