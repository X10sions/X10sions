namespace Microsoft.Graph.Models;

public static class FileAttachmentExtensions {

  public static FileInfo SaveTo(this FileAttachment fileAttachment, DirectoryInfo directory) 
    => directory.CreateFile(fileAttachment.Name, fileAttachment.ContentBytes);

}
