namespace MimeKit;
public static class AttachmentCollectionExtensions {
  public static MimeEntity Add(this AttachmentCollection attachments, FileInfo file, CancellationToken cancellationToken = default) => attachments.Add(file.FullName, cancellationToken);
  public static MimeEntity Add(this AttachmentCollection attachments, FileInfo file, string attchmentName, CancellationToken cancellationToken = default) {
    var attachment = attachments.Add(file, cancellationToken);
    attachment.ContentDisposition.FileName = attchmentName;
    return attachment;
  }
  public static MimeEntity Add(this AttachmentCollection attachments, string fileName, string attchmentName, CancellationToken cancellationToken = default) {
    var attachment = attachments.Add(fileName, cancellationToken);
    attachment.ContentDisposition.FileName = attchmentName;
    return attachment;
  }

}