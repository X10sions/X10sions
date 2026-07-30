namespace System.Net.Http;

public static class HttpClientExtensions {
  public static async Task<string> GetStringAsync(this HttpClient httpClient, string requestUri, CancellationToken cancellationToken) => await httpClient.GetStringAsync(requestUri).WithCancellation(cancellationToken);
  public static async Task<string> GetStringAsync(this HttpClient httpClient, Uri requestUri, CancellationToken cancellationToken) => await httpClient.GetStringAsync(requestUri).WithCancellation(cancellationToken);
}
