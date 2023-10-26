using System.Security.Principal;

namespace Common.Domain.ValueObjects;
public readonly record struct FileName(string Value) {
  public string ReplaceInvalidChars(string replaceInvalidCharsWith = "_") => string.Join(replaceInvalidCharsWith, Value.Split(Path.GetInvalidFileNameChars()));
  public string UserTempFilePath(string basePath, string replaceInvalidCharsWith = "_") => Path.Combine(basePath, ReplaceInvalidChars(replaceInvalidCharsWith));
  public FileInfo FileInfo(string basePath, string replaceInvalidCharsWith = "_") => new FileInfo(Path.Combine(basePath, ReplaceInvalidChars(replaceInvalidCharsWith)));
  public string UserTempFilePath(string basePath, IIdentity identity, string replaceInvalidCharsWith = "_") => Path.Combine(basePath, identity.Name, ReplaceInvalidChars(replaceInvalidCharsWith));
  //public string UserTempFilePath(IPathToAppSettings pathTo, IIdentity identity) => pathTo.UserTempFilePath(identity, Value);
};

public readonly record struct FileExtension(string Value) {
  public string WithDot => ("." + Value).TrimStart("..");
  public string FileName(FileName file) => Path.Combine(file.Value, WithDot);
  //  Path.Combine(settings.BaseUsersDirectory, userName, "temp", fileName.ReplaceInvalidChars("_"));
};