using System.Text.Json;

namespace System.IO;
public static class xxFileInfoExtensions {
  public static Task<string> ReadAllTextAsync(this FileInfo fileInfo, CancellationToken token = default) => File.ReadAllTextAsync(fileInfo.FullName, token);
  public static T? ReadAllTextJson<T>(this FileInfo fileInfo) => JsonSerializer.Deserialize<T>(File.ReadAllText(fileInfo.FullName) ?? string.Empty);
  public static async Task<T?> ReadAllTextJsonAsync<T>(this FileInfo fileInfo, CancellationToken token = default) => JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(fileInfo.FullName, token) ?? string.Empty);
  public static async Task<T> ReadAllTextJsonElseSaveAsync<T>(this FileInfo fileInfo, T defaultValue, CancellationToken token = default) {
    var value = await fileInfo.ReadAllTextJsonAsync<T>(token);
    if (value == null) {
      value = defaultValue;
      await fileInfo.WriteAllTextJsonAsync(value, token);
    }
    return value;
  }
  public static async Task WriteAllTextAsync(this FileInfo fileInfo, string contents, CancellationToken token = default) => await File.WriteAllTextAsync(fileInfo.FullName, contents, token);
  public static void WriteAllTextJson<T>(this FileInfo fileInfo, T result) => fileInfo.WriteAll(JsonSerializer.Serialize(result));
  public static async Task WriteAllTextJsonAsync<T>(this FileInfo fileInfo, T result, CancellationToken token = default) => await fileInfo.WriteAllTextAsync(JsonSerializer.Serialize(result), token);
}