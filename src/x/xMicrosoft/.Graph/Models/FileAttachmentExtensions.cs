namespace Microsoft.Graph.Models;

public static class FileAttachmentExtensions {

  public static FileInfo SaveTo(this FileAttachment fileAttachment, DirectoryInfo directory) {
    var fi = directory.CreateFile(fileAttachment.Name, fileAttachment.ContentBytes);
    var date = fileAttachment.LastModifiedDateTime?.UtcDateTime;
    if (date.HasValue) {
      fi.CreationTimeUtc = date.Value;
      fi.LastWriteTimeUtc = date.Value;
    }
    return fi;
  }

}