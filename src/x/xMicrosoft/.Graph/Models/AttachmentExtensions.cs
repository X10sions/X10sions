namespace Microsoft.Graph.Models;

public static class AttachmentExtensions {

  public readonly static string ExpandAttachment = nameof(Message.Attachments);
  public readonly static string ExpandAttachmentNameSize = $"{nameof(Message.Attachments)}($select={nameof(Attachment.Name)},{nameof(Attachment.Size)})";

  public static FileInfo SaveTo(this Attachment attachment, DirectoryInfo directory) => attachment switch {
    FileAttachment fileAttachment => fileAttachment.SaveTo(directory),
    ItemAttachment => throw new NotSupportedException(attachment.ContentType),
    ReferenceAttachment => throw new NotSupportedException(attachment.ContentType),
    _ => throw new NotSupportedException(attachment.ContentType)
  };

  public static void SaveTo(this IEnumerable<Attachment> attachments, DirectoryInfo directory) {
    foreach (var attachment in attachments) {
      attachment.SaveTo(directory);
    }
  }

}
