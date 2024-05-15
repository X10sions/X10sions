using MailKit.Search;
using MimeKit;

namespace MailKit;

public class FolderMessage {
  // https://stackoverflow.com/questions/34676863/mailkit-imap-get-read-and-unread-status-of-a-mail
  public FolderMessage(IMailFolder mailfolder, UniqueId uid) {
    Message = mailfolder.GetMessage(uid);
    Info = mailfolder.Fetch(new[] { uid }, MessageSummaryItems.Flags);
  }
  public MimeMessage Message { get; }
  public IList<IMessageSummary> Info { get; }
  public MessageFlags? Flags => Info[0].Flags;
  public bool IsDeleted => Flags.IsDeleted();
  public bool IsDraft => Flags.IsDraft();
  public bool IsFlagged => Flags.IsFlagged();
  public bool IsSeen => Flags.IsSeen();
  string[] FromSplit => Message.From.ToString().Split('<');
  //bool HasSenderName => !string.IsNullOrWhiteSpace(Message.Sender?.Name);
  //bool HasSenderAddress => !string.IsNullOrWhiteSpace(Message.Sender?.Address);
  //public string FromName => HasSenderName ? Message.Sender.Name : FromSplit.FirstOrDefault().Trim('"');
  public string FromName => Message.Sender?.Name ?? (FromSplit.Count() > 1 ? FromSplit.FirstOrDefault().Trim().Trim('"') : string.Empty);
  public string FromAddress => Message.Sender?.Address ?? FromSplit.LastOrDefault().TrimStart('<').TrimEnd('>');
}
public static class FolderMessageExtensions {

  public static List<FolderMessage> SortFolderMessageList(this List<FolderMessage> folderMessages, OrderByType orderByType, SortOrder sortOrder) {
    var keySelector = orderByType.GetFolderMessageSortKeySelector();
    return (sortOrder == SortOrder.Descending ? folderMessages.OrderByDescending(keySelector) : folderMessages.OrderBy(keySelector)).ToList();
  }
}
