using System.ComponentModel;

namespace Common.Html.Attributes {
  public interface IDirHtmlAttribute : IHtmlAttribute<IDirHtmlAttribute.Value> {
    public enum Value {
      [Description("LeftToRight")] ltr,
      [Description("RightToleft")] rtl,
      auto
    }
  };

  /// <summary>Specifies the text direction For the content In an element</summary>
  public readonly record struct DirHtmlAttribute(IDirHtmlAttribute.Value Value) : IDirHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.dir;
//    public TextDirectionCode Value { get => Tag.GetAttributeValue(this); set => Tag.SetAttributeValue(this, value); }
  }

}