using Microsoft.AspNetCore.Identity;
using RepoDb;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.RepoDb {
  public class _BaseIdentityRoleStore_RepoDb<TRole, TKey> : _BaseIdentityRoleStore<TRole, TKey>
    , IQueryableRoleStore<TRole>
    , IRoleStore<TRole>
    where TRole : class, IIdentityRole<TKey>, IIdentityRoleWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStore_RepoDb(IDbConnection dbConnection, Expression<Func<TRole, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      db = dbConnection;
      BaseSqlWhere = baseSqlWhere;
    }

    private readonly IDbConnection db;
    public Expression<Func<TRole, bool>> BaseSqlWhere { get; }

    #region IQueryableRoleStore
    public override IQueryable<TRole> Roles => throw new NotImplementedException();
    #endregion

    #region IRoleStore
    public override Func<TRole, string> GetRoleOrUserDescription => x => x.Name;

    protected override async Task<bool> CreateAsync_Insert(TRole role, CancellationToken cancellationToken = default) => await db.InsertAsync<TRole, TKey>(role) != null;
    protected override async Task<bool> DeleteAsync_Delete(TRole role, CancellationToken cancellationToken = default) => await db.DeleteAsync(role) > 0;
    protected override async Task<bool> UpdateAsync_Update(TRole role, CancellationToken cancellationToken = default) => await db.UpdateAsync(role) > 0;
    #endregion
  }
}
