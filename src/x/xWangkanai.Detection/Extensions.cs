using Microsoft.Extensions.DependencyInjection;
namespace xWangkanai.Detection {
  public static class Extensions {

    public static void AddxWangkanai_Detection(this IServiceCollection services) {
      services.ConfigureOptions(typeof(PostConfigureOptions));
    }

  }

}

