namespace Common.ValueObjects;

public readonly record struct Printer(string Server, string Name){
  public string Path => $@"\\{Server}\{Name}";

  public override string ToString() => Path;
};

public interface IPrinter {
  Printer Printer { get; }
}