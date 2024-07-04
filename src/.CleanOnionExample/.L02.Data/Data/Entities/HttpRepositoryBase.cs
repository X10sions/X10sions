using System.Text;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleanOnionExample.Data.Entities;

/// <summary>
/// https://github.com/devpro/withywoods/blob/master/src/Net.Http/HttpRepositoryBase.cs
/// </summary>
public abstract class HttpRepositoryBase(HttpClient HttpClient, ILogger Logger) {
  public const string Application_Json = "application/json";
  //protected abstract string HttpClientName { get; }
  protected virtual void EnrichRequestHeaders(HttpClient client) { }

  protected virtual async Task<T?> GetAsync<T>(string url) where T : class {
    var response = await HttpClient.GetAsync(url);
    var stringResult = await response.Content.ReadAsStringAsync();
    CheckStatusCodeAndResult(url, response, stringResult);
    return Deserialize<T>(url, HttpMethod.Get, stringResult);
  }

  protected virtual async Task<T?> PostAsync<T>(string url, object body, string mediaType = Application_Json) where T : class {
    var response = await HttpClient.PostAsync(url, new StringContent(body.ToJson(), Encoding.UTF8, mediaType));
    var stringResult = await response.Content.ReadAsStringAsync();
    CheckStatusCodeAndResult(url, response, stringResult);
    return Deserialize<T>(url, HttpMethod.Post, stringResult);
  }

  protected virtual async Task PutAsync(string url, object body, string mediaType = Application_Json) {
    var response = await HttpClient.PutAsync(url, new StringContent(body.ToJson(), Encoding.UTF8, mediaType));
    var stringResult = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
    CheckStatusCode(url, response, stringResult);
  }

  protected virtual async Task DeleteAsync(string url, bool ignoreNotFound) {
    var response = await HttpClient.DeleteAsync(url);
    var stringResult = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
    if (!ignoreNotFound || response.StatusCode != HttpStatusCode.NotFound) {
      CheckStatusCode(url, response, stringResult);
    }
  }

  public class ConnectivityException : Exception {
    public ConnectivityException(string Message) : base(Message) { }
    public ConnectivityException(string Message, Exception InnerException) : base(Message, InnerException) { }
  }

  private void CheckStatusCode(string url, HttpResponseMessage response, string result) {
    if (!response.IsSuccessStatusCode) {
      Logger.LogDebug($"Status code doesn't indicate success [HttpRequestUrl={url}] [HttpResponseStatusCode={response.StatusCode}] [HttpResponseContent={result}]");
      throw new ConnectivityException($"The response status \"{response.StatusCode}\" is not a success (reason=\"{response.ReasonPhrase}\")");
    }
  }

  private void CheckStatusCodeAndResult(string url, HttpResponseMessage response, string result) {
    CheckStatusCode(url, response, result);
    if (string.IsNullOrEmpty(result)) {
      throw new ConnectivityException($"Empty response received while calling {url}");
    }
  }

  private T? Deserialize<T>(string url, HttpMethod httpMethod, string result) {
    try {
      return result.FromJson<T>();
    } catch (Exception exc) {
      Logger.LogWarning($"Cannot deserialize {httpMethod.Method} call response content [HttpRequestUrl={url}] [SerializationType={typeof(T)}] [ExceptionMessage={exc.Message}]");
      Logger.LogDebug($"[HttpResponseContent={result}]");
      Logger.LogDebug($"[Stacktrace={exc.StackTrace}]");
      throw new ConnectivityException($"Invalid data received when calling \"{url}\": {exc.Message}", exc);
    }
  }

}
