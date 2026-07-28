using Common.Html.Attributes;

namespace Common.Html.Tags {
  public interface IHtmlTag_v5 : IHtmlTag_v4_01 { }

  public static class IHtmlTag_v5Extensions {
    //[Obsolete(ObsoleteReason.Not_supported_in_HTML_5)] public static Align Align(this IHtmlTag_v5 tag) => throw new NotImplementedException();
    //public static ContentEditable ContentEditable(this IHtmlTag_v5 tag) => tag.GetAttributeValue<ContentEditable>();
    //public static ContextMenu ContextMenu(this IHtmlTag_v5 tag) => tag.GetAttributeValue<ContextMenu>();
    //public static Attributes.Data Data(this IHtmlTag_v5 tag) => tag.GetAttributeValue<Attributes.Data>();

    public static IDraggableHtmlAttribute Draggable(this IHtmlTag tag) => tag.GetHtmlAttribute<IDraggableHtmlAttribute>(IHtmlAttribute.Key.draggable);
    public static T Draggable<T>(this T tag, DraggableHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

    //public static DropZone DropZone(this IHtmlTag_v5 tag) => tag.GetAttributeValue<DropZone>();
    //public static EnterKeyHint EnterKeyHint(this IHtmlTag_v5 tag) => tag.GetAttributeValue<EnterKeyHint>();

    public static IHiddenHtmlAttribute Hidden(this IHtmlTag tag) => tag.GetHtmlAttribute<IHiddenHtmlAttribute>(IHtmlAttribute.Key.hidden);
    public static T Hidden<T>(this T tag, HiddenHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

    //public static Inert Inert(this IHtmlTag_v5 tag) => tag.GetAttributeValue<Inert>();
    //public static InputMode InputMode(this IHtmlTag_v5 tag) => tag.GetAttributeValue<InputMode>();
    //public static Popover Popover(this IHtmlTag_v5 tag) => tag.GetAttributeValue<Popover>();
    //public static SpellCheck SpellCheck(this IHtmlTag_v5 tag) => tag.GetAttributeValue<SpellCheck>();

  }
}