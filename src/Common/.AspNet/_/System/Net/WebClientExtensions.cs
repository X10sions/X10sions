using System.Web;

namespace System.Net {
  public static class WebClientExtensions {

    public static IHtmlString WebPage(this WebClient webClient, string url) => new HtmlString(webClient.DownloadString(url));

  }
}
