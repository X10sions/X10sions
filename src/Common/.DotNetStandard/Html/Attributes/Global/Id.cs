namespace Common.Html.Attributes {
  public interface IIdHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies a unique id For an element</summary>
  public readonly record struct IdHtmlAttribute(string Value) : IIdHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.id;

    public static IdHtmlAttribute Empty { get; } = new(string.Empty);
  }


  //public interface IUniqueIdHtmlAttribute : IHtmlAttributeString, IUniqueIdString { };

  ///// <summary>Specifies a unique id For an element</summary>
  //public readonly record struct UniqueIdHtmlAttribute(string Prefix) : IHtmlAttributeString  {
  //  public string Name { get; } = "uniqueid";
  //  public string Value { get; } = $"{Prefix}{uniqueIdCount++}";
  //  static int uniqueIdCount = 0;

  //}

}