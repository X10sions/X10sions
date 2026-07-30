namespace System.Net.Http;

public static class HttpContentExtensions {
  public async static Task<string> ReadAsStringAsync(this HttpContent content, CancellationToken cancellationToken) => await content.ReadAsStringAsync().WithCancellation(cancellationToken);
  public async static Task<string> ReadAsString2Async(this HttpContent content, CancellationToken cancellationToken) => await content.ReadAsStringAsync().WithCancellationAsync(cancellationToken);
}