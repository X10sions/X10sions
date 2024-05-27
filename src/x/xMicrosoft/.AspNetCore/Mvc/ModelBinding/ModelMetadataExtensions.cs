using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ModelMetadataExtensions {

  public static IEnumerable<SelectListItem> GetEnumSelectList(this ModelMetadata metadata) {
    ArgumentNullException.ThrowIfNull(metadata);
    if (!metadata.IsEnum || metadata.IsFlagsEnum) {
      var message = $"Resources.FormatHtmlHelper_TypeNotSupported_ForGetEnumSelectList({metadata.ModelType.FullName}, {nameof(Enum).ToLowerInvariant()}, {nameof(FlagsAttribute)})";
      throw new ArgumentException(message, nameof(metadata));
    }
    var selectList = new List<SelectListItem>();
    var groupList = new Dictionary<string, SelectListGroup>();
    foreach (var keyValuePair in metadata.EnumGroupedDisplayNamesAndValues) {
      var selectListItem = new SelectListItem {
        Text = keyValuePair.Key.Name,
        Value = keyValuePair.Value,
      };
      if (!string.IsNullOrEmpty(keyValuePair.Key.Group)) {
        if (!groupList.TryGetValue(keyValuePair.Key.Group, out var group)) {
          group = new SelectListGroup() { Name = keyValuePair.Key.Group };
          groupList[keyValuePair.Key.Group] = group;
        }
        selectListItem.Group = group;
      }
      selectList.Add(selectListItem);
    }
    return selectList;
  }

}
