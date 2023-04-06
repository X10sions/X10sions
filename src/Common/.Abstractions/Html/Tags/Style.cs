using Common.Attributes;
using System.Text;

namespace Common.Html.Tags {

  public class Style : HtmlTag5Base<Style> {
    //public const string StartTag = "<style>";
    //public const string EndTag = "</style>";

    public override string ToHtml() {

      var sb = new StringBuilder();
      sb.AppendLine($"<{TagName}");
      sb.Append($" {NotNullAttr(nameof(Type), Type)}");
      sb.Append(">");
      foreach (var s in Selctors) {
        sb.AppendLine(s.ToHtml());
      }
      sb.AppendLine($"</{TagName}>");

      //sb.AppendLine(StartTag);
      //sb.AppendLine($" type='{Type}'");
      //sb.AppendLine(EndTag);
      return sb.ToString();
    }

    [ToDo] public bool? Media { get; set; }



    /// <summary>
    /// Media type 
    /// </summary>
    [ToDo] public string Type { get; set; } = "text/css";

    public HashSet<StyleSelctor> Selctors { get; set; } = new HashSet<StyleSelctor>();
    //public Dictionary<string, string> Selectors { get; } = new Dictionary<string, string>();

    public class StyleSelctor {
      StyleSelctor(string selector) {
        Selector = selector;
      }
      public StyleSelctor(string selector, HashSet<StyleDeclaration> declarations) : this(selector) {
        foreach (var d in declarations) {
          Declarations.Add(d);
        }
      }

      public StyleSelctor(string selector, string declaration) : this(selector) {
        foreach (var d in declaration.Split(';')) {
          var kvp = d.Split(':');
          Declarations.Add(new StyleDeclaration(kvp[0], kvp[1]));
        }
      }

      public string Selector { get; }
      public HashSet<StyleDeclaration> Declarations { get; } = new HashSet<StyleDeclaration>();

      public string ToHtml() {
        var sb = new StringBuilder();
        sb.Append($"{Selector}{{");
        foreach (var p in Declarations) {
          sb.Append(p.ToHtml());
        }
        sb.Append($"}}");
        return sb.ToString();
      }

      public class StyleDeclaration {
        public StyleDeclaration(string property, string value) {
          Property = property;
          Value = value;
        }
        public string Property { get; set; }
        public string Value { get; set; }

        public string ToHtml() => $"{Property}:{Value};";

        //public static string DeclarationsToHtml(IEnumerable<StyleDeclaration> declarations) {
        //  var sb = new StringBuilder();
        //  foreach (var p in declarations) {
        //    sb.Append(p.ToHtml());
        //  }
        //  return sb.ToString();
        //}


      }

    }

  }

}
