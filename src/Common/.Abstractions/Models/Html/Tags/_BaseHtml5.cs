using System.Collections.Generic;

namespace Common.Models.Html.Tags {
  public abstract class _BaseHtml5 : _BaseHtml401 {
    const string DataPrefix = "data-";

    public bool? ContentEditable { get; set; } // Specifies whether the content Of an element Is editable Or Not
    public Menu ContextMenu { get; set; } // Specifies a context menu For an element. The context menu appears When a user right-clicks On the element
    public Dictionary<string, object> Data { get; set; }  // Used To store custom data Private To the page Or application
    public TrueFalseAuto? Draggable { get; set; } // Specifies whether an element Is draggable Or Not
    public DropZone_Code? DropZone { get; set; } // Specifies whether the dragged data Is copied, moved, Or linked, When dropped
    public bool? Hidden { get; set; }  // Specifies that an element Is Not yet, Or Is no longer, relevant
    public bool? SpellCheck { get; set; } // Specifies whether the element Is To have its spelling And grammar checked Or Not
    public YesNo? Translate { get; set; } // Specifies whether the content Of an element should be translated Or Not

    public Dictionary<string, string> Attributes() {
      var dic = new Dictionary<string, string> {
        [nameof(ContentEditable)] = ContentEditable.Value.ToString(),
        [nameof(ContextMenu)] = ContextMenu?.Id
      };
      foreach (var k in Data.Keys)
        dic[nameof(DataPrefix) + k] = Data[k].ToString();
      dic[nameof(Draggable)] = Draggable.Value.ToString();
      dic[nameof(DropZone)] = DropZone.Value.ToString();
      dic[nameof(Hidden)] = Hidden.Value.ToString();
      dic[nameof(SpellCheck)] = SpellCheck.Value.ToString();
      dic[nameof(Translate)] = Translate.Value.ToString();
      return dic;
    }

    public enum TrueFalseAuto {
      True,
      False,
      Auto
    }

    public enum DropZone_Code {
      copy,  // Dropping the data will result In a copy Of the dragged data
      move,  // Dropping the data will result In that the dragged data Is moved To the New location
      link  // Dropping the data will result In a link To the original data
    }

    public enum YesNo {
      Yes,
      No
    }

  }
}
