namespace Common.Data;
public class DbProviderKeyword {
  public string? ReplaceWith { get; set; }
  public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
}