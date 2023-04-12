using System.Collections;

namespace Common.Html.Tags {
  public abstract class HtmlTag5Base<T> : HtmlTag401Base<T> where T : IHtmlTag {
    const string DataPrefix = "data-";

    public Dictionary<string, string> xAttributes() {
      var dic = new Dictionary<string, string> {
        [nameof(ContentEditable)] = ContentEditable.Value.ToString(),
        [nameof(ContextMenu)] = ContextMenu.Id
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


    /// <summary>
    ///  Specifies whether the content Of an element Is editable Or Not
    /// </summary>
    public bool? ContentEditable { get => Attributes.Get<bool>(nameof(ContentEditable)); set => attributes[nameof(ContentEditable)] = value; }

    /// <summary>
    /// Specifies a context menu For an element. The context menu appears When a user right-clicks On the element
    /// </summary>
    public Menu ContextMenu { get => Attributes.Get<Menu>(nameof(ContextMenu)); set => attributes[nameof(ContextMenu)] = value; }

    /// <summary>
    /// Used To store custom data Private To the page Or application
    /// </summary>
    public Dictionary<string, object> Data { get => Attributes.Get<Dictionary<string, object>>(nameof(Data)); set => attributes[nameof(Data)] = value; }

    /// <summary>
    /// Specifies whether an element Is draggable Or Not
    /// </summary>
    public TrueFalseAuto? Draggable { get => Attributes.Get<TrueFalseAuto>(nameof(Draggable)); set => attributes[nameof(Draggable)] = value; }

    /// <summary>
    /// Specifies whether the dragged data Is copied, moved, Or linked, When dropped
    /// </summary>
    public DropZone_Code? DropZone { get => Attributes.Get<DropZone_Code>(nameof(DropZone)); set => attributes[nameof(DropZone)] = value; }

    /// <summary>
    /// Specifies that an element Is Not yet, Or Is no longer, relevant
    /// </summary>
    public bool? Hidden { get => Attributes.Get<bool>(nameof(Hidden)); set => attributes[nameof(Hidden)] = value; }

    /// <summary>
    /// Specifies whether the element Is To have its spelling And grammar checked Or Not
    /// </summary>
    public bool? SpellCheck { get => Attributes.Get<bool>(nameof(SpellCheck)); set => attributes[nameof(SpellCheck)] = value; }

    /// <summary>
    /// Specifies whether the content Of an element should be translated Or Not
    /// </summary>
    public YesNo? Translate { get => Attributes.Get<YesNo>(nameof(Translate)); set => attributes[nameof(Translate)] = value; }

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
