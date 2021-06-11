using Microsoft.Extensions.DependencyInjection;
namespace xUAParser {
  public static class Extensions {

    public static void AddxUAParser(this IServiceCollection services) {
      services.ConfigureOptions(typeof(PostConfigureOptions));
    }

  }

}