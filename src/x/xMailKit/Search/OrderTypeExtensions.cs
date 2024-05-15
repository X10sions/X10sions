using MimeKit;

namespace MailKit.Search;
internal static class OrderTypeExtensions {
  internal static Func<MimeMessage, object> GetMimeMessageSortKeySelector(this OrderByType orderByType) => orderByType switch {
    OrderByType.Arrival => x => x.Date,
    //OrderByType.Annotation => x => x.Annotation,
    OrderByType.Cc => x => x.Cc.ToString(),
    OrderByType.Date => x => x.Date,
    OrderByType.DisplayFrom => x => x.Sender?.Name,
    OrderByType.DisplayTo => x => x.To.FirstOrDefault()?.Name,
    OrderByType.From => x => x.From.ToString(),
    //OrderByType.ModSeq => x =>  x.ModSeq,
    OrderByType.Size => x => x.GetMessageSize(),
    OrderByType.Subject => x => x.Subject,
    OrderByType.To => x => x.To.ToString(),
    _ => throw new NotImplementedException()
  };

  internal static Func<FolderMessage, object> GetFolderMessageSortKeySelector(this OrderByType orderByType) {
    switch (orderByType) {
      case (OrderByType.Date):
        return x => x.Message.Date;
      case (OrderByType.Subject):
        return x => x.Message.Subject;
      default:
        throw new NotImplementedException();
    }
  }

}
