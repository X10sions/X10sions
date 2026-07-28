namespace Common.Html.Attributes {
  public interface IDropZoneHtmlAttribute : IHtmlAttribute<IDropZoneHtmlAttribute.Values?> {
    public enum Values {
      move,  // Dropping the data will result In that the dragged data Is moved To the New location
      copy,  // Dropping the data will result In a copy Of the dragged data
      link  // Dropping the data will result In a link To the original data
    }
  };

  /// <summary>Specifies whether the dragged data Is copied, moved, Or linked, When droppedt</summary>
  public readonly record struct DropZoneHtmlAttribute(IDropZoneHtmlAttribute.Values? Value) : IDropZoneHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.dropzone;
  }
}