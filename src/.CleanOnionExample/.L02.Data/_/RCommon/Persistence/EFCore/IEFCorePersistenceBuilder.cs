using Microsoft.EntityFrameworkCore;

namespace RCommon.Persistence.EFCore;

public interface IEFCorePersistenceBuilder : IPersistenceBuilder {
  IEFCorePersistenceBuilder AddDbContext<TDbContext>(string dataStoreName, Action<DbContextOptionsBuilder>? options) where TDbContext : RCommonDbContext;
}
