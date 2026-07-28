using Common.Attributes;

namespace Common.Html.Tags;

public class OptGroup : HtmlTag5Base<OptGroup> {
  #region Attributes

  /// <summary>
  /// Specifies that an option-group should be disabled
  /// </summary>
  public bool? Disabled { get => GetAttribute<bool?>(nameof(Disabled)); set => attributes[nameof(Disabled)] = value; }
  /// <summary>
  /// Specifies a shorter label For an Option
  /// </summary>
  public string? Label { get => GetAttribute<string>(nameof(Label)); set => attributes[nameof(Label)] = value; }

  #endregion

  public List<Option> OptionList { get; set; } = new List<Option>();
  [ToDo] public override string ToHtml() => throw new NotImplementedException();
}