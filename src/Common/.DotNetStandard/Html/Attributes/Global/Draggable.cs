namespace Common.Html.Attributes {

  public interface IDraggableHtmlAttribute : IHtmlAttribute<IDraggableHtmlAttribute.Value> {
    public enum Value { @true, @false, auto }
  };

  /// <summary>Specifies whether an element is draggable or not</summary>
  public readonly record struct DraggableHtmlAttribute(IDraggableHtmlAttribute.Value Value) : IDraggableHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.draggable;
  }

}