using MailKit.Search;

namespace MimeKit;
  public static class MimeMessageExtensions {

  public static List<MimeMessage> SortFolderMessageList(this List<MimeMessage> folderMessages, OrderByType orderByType, SortOrder sortOrder) {
    if (sortOrder == SortOrder.None) {
      return folderMessages;
    } else {
      var keySelector = orderByType.GetMimeMessageSortKeySelector();
      return (sortOrder == SortOrder.Descending ? folderMessages.OrderByDescending(keySelector) : folderMessages.OrderBy(keySelector)).ToList();
    }
  }

}
