using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microsoft.AspNetCore.Mvc.ModelBinding;
public static class IModelMetadataProviderExtensions {

  public static IEnumerable<SelectListItem> GetEnumSelectList<TEnum>(this IModelMetadataProvider metadataProvider) where TEnum : struct => metadataProvider.GetEnumSelectList(typeof(TEnum));

  public static IEnumerable<SelectListItem> GetEnumSelectList(this IModelMetadataProvider metadataProvider, Type enumType) {
    ArgumentNullException.ThrowIfNull(enumType);
    var metadata = metadataProvider.GetMetadataForType(enumType);
    if (!metadata.IsEnum || metadata.IsFlagsEnum) {
      var message = $"Resources.FormatHtmlHelper_TypeNotSupported_ForGetEnumSelectList({enumType.FullName}, {nameof(Enum).ToLowerInvariant()}, {nameof(FlagsAttribute)})";
      throw new ArgumentException(message, nameof(enumType));
    }
    return metadata.GetEnumSelectList();
  }

}
