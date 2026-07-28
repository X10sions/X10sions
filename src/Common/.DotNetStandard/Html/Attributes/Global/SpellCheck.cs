namespace Common.Html.Attributes {
  public interface ISpellCheckHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Specifies whether the element Is To have its spelling And grammar checked Or Not</summary>
  public readonly record struct SpellCheckHtmlAttribute(bool Value) : ISpellCheckHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.spellcheck;
  }

}