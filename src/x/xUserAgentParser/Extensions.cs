using Microsoft.Extensions.DependencyInjection;
namespace xUserAgentParser {
  public static class xUserAgentParser_ServiceCollectionExtensions {

    public static void AddxUserAgentParser(this IServiceCollection services) {
      services.ConfigureOptions(typeof(PostConfigureOptions));
    }

  }

}