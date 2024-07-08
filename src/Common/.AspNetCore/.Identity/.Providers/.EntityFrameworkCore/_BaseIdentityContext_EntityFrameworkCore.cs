using Microsoft.EntityFrameworkCore;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class _BaseIdentityContext_EntityFrameworkCore : DbContext, IIdentityContext_EntityFrameworkCore {

    public _BaseIdentityContext_EntityFrameworkCore(DbContextOptions options) : base(options) { }

    public async Task DbDeleteAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.xDbDeleteAsync<T>(data, cancellationToken);
    public async Task DbInsertAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.xDbInsertAsync<T>(data, cancellationToken);
    public async Task DbUpdateAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.xDbUpdateAsync<T>(data, cancellationToken);
    public IQueryable<T> DbGetQueryable<T>() where T : class => Set<T>();
  }

}
