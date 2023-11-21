namespace Common.Domain.ValueObjects;
public readonly record struct FileName(string Value) {
  //public string ReplaceInvalidChars(string replaceInvalidCharsWith = "_") => string.Join(replaceInvalidCharsWith, Value.Split(Path.GetInvalidFileNameChars()));
};

public interface IFileName {
  FileName FileName { get; }
}

public readonly record struct FileExtension(string Value) {
  public string WithDot => ("." + Value).TrimStart("..");
  public string FileName(FileName file) => Path.Combine(file.Value, WithDot);
};