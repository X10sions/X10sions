using Microsoft.AspNetCore.Identity;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.SqlKata {
  public class _BaseIdentityRoleStore_SqlKata<TRole, TKey> : _BaseIdentityRoleStore<TRole, TKey>
    , IQueryableRoleStore<TRole>
    , IRoleStore<TRole>
    where TRole : class, IIdentityRole<TKey>, IIdentityRoleWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStore_SqlKata(Query roleTableQuery, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      RoleTableQuery = roleTableQuery;
    }

    public _BaseIdentityRoleStore_SqlKata(QueryFactory queryFactory, string roleTableName, IdentityErrorDescriber errorDescriber)
      : this(queryFactory.Query(roleTableName), errorDescriber) { }

    public override IQueryable<TRole> Roles => throw new NotImplementedException();
    public Query RoleTableQuery { get; }

    public virtual object InsertRole(TRole role) => UpdateRole(role).MergeToExpandoObject(new { role.Id });

    public virtual object UpdateRole(TRole role) => new {
      role.Name,
      role.NormalizedName
    };

    #region IRoleStore
    public override Func<TRole, string> GetRoleOrUserDescription => x => x.Name;

    protected override async Task<bool> CreateAsync_Insert(TRole role, CancellationToken cancellationToken = default) => await RoleTableQuery.InsertAsync(InsertRole(role)) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TRole role, CancellationToken cancellationToken = default) => await RoleTableQuery.Where(nameof(role.Id), role.Id).DeleteAsync() > 0;
    protected override async Task<TRole> FindByIdAsync_Select(string roleId, CancellationToken cancellationToken = default) => await RoleTableQuery.Where(nameof(DefaultRoleOrUser.Id), roleId).FirstOrDefaultAsync<TRole>();
    protected override async Task<TRole> FindByNameAsync_Select(string normalizedRoleName, CancellationToken cancellationToken = default) => await RoleTableQuery.Where(nameof(DefaultRoleOrUser.NormalizedName), normalizedRoleName.ToLower()).FirstOrDefaultAsync<TRole>();
    protected override async Task<TRole> FindByIdAndConcurrencyStampAsync(TRole role, CancellationToken cancellationToken = default) => await RoleTableQuery.Where(nameof(role.Id), role.Id).Where(nameof(role.ConcurrencyStamp), role.ConcurrencyStamp).FirstOrDefaultAsync<TRole>();
    protected override async Task<bool> UpdateAsync_Update(TRole role, CancellationToken cancellationToken = default) => await RoleTableQuery.Where(nameof(role.Id), role.Id).UpdateAsync(UpdateRole(role)) > 0;
    #endregion
  }
}
