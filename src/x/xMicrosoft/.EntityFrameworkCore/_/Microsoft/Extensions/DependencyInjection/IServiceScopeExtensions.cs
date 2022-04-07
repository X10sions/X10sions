using Microsoft.EntityFrameworkCore;

namespace  Microsoft.Extensions.DependencyInjection {
  public static class IServiceScopeExtensions {
    public static async Task ApplyMigration<T>(this IServiceScope serviceScope) where T : DbContext {
      var dbContext = serviceScope.ServiceProvider.GetRequiredService<T>();
      await dbContext.Database.MigrateAsync();
    }

  }
}
