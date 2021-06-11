using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Dapper {
  public class _BaseIdentityRoleStore_Dapper<TRole, TKey> : _BaseIdentityRoleStore<TRole, TKey>
    , IQueryableRoleStore<TRole>
    , IRoleStore<TRole>
    where TRole : class, IIdentityRole<TKey>, IIdentityRoleWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStore_Dapper(IDbConnection db, string baseSqlFrom, string baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      this.db = db;
      BaseSqlFrom = baseSqlFrom;
      BaseSqlWhere = baseSqlWhere;
    }

    private readonly IDbConnection db;
    public string BaseSqlFrom { get; }
    public string BaseSqlWhere { get; }

    #region IQueryableRoleStore
    public override IQueryable<TRole> Roles => throw new NotImplementedException();
    #endregion

    #region IRoleStore 
    public override Func<TRole, string> GetRoleOrUserDescription => x => x.Name;

    protected override async Task<bool> CreateAsync_Insert(TRole role, CancellationToken cancellationToken = default) => await db.InsertAsync(role) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TRole role, CancellationToken cancellationToken = default) => await db.DeleteAsync(role);

    protected override async Task<TRole> FindByIdAsync_Select(string roleId, CancellationToken cancellationToken = default) {
      var param = new { Id = roleId };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(param.Id)} = @{nameof(param.Id)}";
      return await db.QuerySingleOrDefaultAsync<TRole>(sql, param);
    }

    protected override async Task<TRole> FindByNameAsync_Select(string normalizedRoleName, CancellationToken cancellationToken = default) {
      var param = new { NormalizedName = normalizedRoleName.ToLower() };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(param.NormalizedName)} = @{nameof(param.NormalizedName)}";
      return await db.QuerySingleOrDefaultAsync<TRole>(sql, param);
    }

    protected override async Task<TRole> FindByIdAndConcurrencyStampAsync(TRole role, CancellationToken cancellationToken = default) {
      var param = new { role.Id, role.ConcurrencyStamp };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(DefaultRoleOrUser.Id)} = @{nameof(param.Id)} And {nameof(DefaultRoleOrUser.ConcurrencyStamp)} = @{nameof(param.ConcurrencyStamp)}";
      return await db.QuerySingleOrDefaultAsync<TRole>(sql, param);
    }

    protected override async Task<bool> UpdateAsync_Update(TRole role, CancellationToken cancellationToken = default) => await db.UpdateAsync(role);

    #endregion

  }
}
