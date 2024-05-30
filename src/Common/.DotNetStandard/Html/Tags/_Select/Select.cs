using Common.Attributes;

namespace Common.Html.Tags;

public class Select : HtmlTag5Base<Select> {
  public Select() {
    _form = new Form();
  }

  #region Attributes

  /// <summary>
  /// Specifies that the drop-down list should automatically Get focus When the page loads
  /// </summary>
  public bool? AutoFocus { get => GetAttribute<bool?>(nameof(AutoFocus)); set => attributes[nameof(AutoFocus)] = value; }
  /// <summary>
  /// Specifies that a drop-down list should be disabled
  /// </summary>
  public bool? Disabled { get => GetAttribute<bool?>(nameof(Disabled)); set => attributes[nameof(Disabled)] = value; }
  /// <summary>
  /// form_id'	Defines one Or more forms the Select field belongs To
  /// </summary>
  //public string FormId { get => Form.Id; set =>  }
  public string FormId {
    get => GetAttribute<string?>(nameof(FormId)); set {
      Form.Id = value;
      attributes[nameof(FormId)] = Form.Id;
    }
  }
  /// <summary>
  /// Specifies that multiple options can be selected at once
  /// </summary>
  public bool? Multiple { get => GetAttribute<bool?>(nameof(Multiple)); set => attributes[nameof(Multiple)] = value; }
  /// <summary>
  /// Defines a name For the drop-down list
  /// </summary>
  public string? Name { get => GetAttribute<string?>(nameof(Name)); set => attributes[nameof(Name)] = value; }
  /// <summary>
  /// Specifies that the user Is required To Select a value before submitting the form
  /// </summary>
  public bool? Required { get => GetAttribute<bool?>(nameof(Required)); set => attributes[nameof(Required)] = value; }
  /// <summary>
  /// Defines the number Of visible options In a drop-down list
  /// </summary>
  public int Size { get; set; }

  #endregion

  private Form _form;
  public Form Form {
    get => _form;
    set {
      _form = value;
      FormId = value.Id;
    }
  }

  [ToDo] public override string ToHtml() => throw new NotImplementedException();
  public List<Option> OptionList { get; set; } = new List<Option>();
}
