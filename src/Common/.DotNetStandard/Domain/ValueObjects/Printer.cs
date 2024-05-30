namespace Common.Domain.ValueObjects;

public readonly record struct Printer(string Server, string Name) {
  public string Path => $@"\\{Server}\{Name}";
};

public interface IPrinter {
  Printer Printer { get; }
}