namespace Common.Html.Tags;

public class Option : HtmlTag5Base<Option>, IInnerText {
  public Option(string? value = null, string? innerText = null) {
    Value = value;
    InnerText = innerText;
  }

  //public Option(Select select) {
  //  Select = select;
  //}

  #region Attributes
  public bool? Disabled { get => GetAttribute<bool?>(nameof(Disabled)); set => attributes[nameof(Disabled)] = value; }

  /// <summary>
  /// Specifies a shorter label For an Option
  /// </summary>
  public string? Label { get => GetAttribute<string?>(nameof(Label)); set => attributes[nameof(Label)] = value; }

  /// <summary>
  /// Specifies that an Option should be pre-selected When the page loads
  /// </summary>
  public bool? Selected { get => GetAttribute<bool?>(nameof(Selected)); set => attributes[nameof(Selected)] = value; }

  /// <summary>
  /// Specifies the value To be sent To a server
  /// </summary>
  public string? Value { get => GetAttribute<string?>(nameof(Value)); set => attributes[nameof(Value)] = value; }
  #endregion

  public OptGroup? OptGroup { get; set; }
  public Select? Select { get; set; }
  public bool? DefaultSelected { get; set; }
  public Form? Form { get; set; }
  public int? Index { get; set; }
  public string? InnerText { get; set; }

  string SelectedHtml => Selected.HasValue && Selected.Value ? " selected=\"selected\" " : string.Empty;
  public override string ToHtml() => $"<option value=\"{Value}\" {SelectedHtml}>{InnerText}</option>";

}
