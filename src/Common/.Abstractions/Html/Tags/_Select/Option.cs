using System.Collections;

namespace Common.Html.Tags;
// TODO
public class Option : HtmlTag5Base<Option>, IInnerText {
  //public Option(Select select) {
  //  Select = select;
  //}

  public bool? Disabled { get; set; }

  /// <summary>
  /// Specifies a shorter label For an Option
  /// </summary>
  public string Label { get; set; }

  /// <summary>
  /// Specifies that an Option should be pre-selected When the page loads
  /// </summary>
  public bool? Selected { get; set; }

  /// <summary>
  /// Specifies the value To be sent To a server
  /// </summary>
  public string? Value { get => Attributes.Get<string?>(nameof(Value)); set => Attributes.Set(nameof(Value), value); }
  public OptGroup? OptGroup { get; set; }
  public Select? Select { get; set; }
  public bool? DefaultSelected { get; set; }
  public Form? Form { get; set; }
  public int? Index { get; set; }
  public string? InnerText { get; set; }

  string SelectedHtml => Selected.HasValue && Selected.Value ? " selected=\"selected\" " : string.Empty;
  public override string ToHtml() => $"<option value=\"{Value}\" {SelectedHtml}>{InnerText}</option>";

}
