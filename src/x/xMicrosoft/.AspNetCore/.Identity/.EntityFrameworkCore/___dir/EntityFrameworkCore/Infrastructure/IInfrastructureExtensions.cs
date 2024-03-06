using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.EntityFrameworkCore.Infrastructure {
  public static class IInfrastructureExtensions {

    public static StoreOptions zGetStoreOptions(this IInfrastructure<IServiceProvider> accessor)
      => accessor.GetService<IDbContextOptions>().Extensions
      .OfType<CoreOptionsExtension>().FirstOrDefault()?
      .ApplicationServiceProvider?.GetService<IOptions<IdentityOptions>>()?.Value?.Stores;

  }
}