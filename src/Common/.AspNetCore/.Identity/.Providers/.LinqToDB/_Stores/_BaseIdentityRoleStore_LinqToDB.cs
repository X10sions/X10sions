using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Microsoft.AspNetCore.Identity;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {

  public class _BaseIdentityRoleStore_LinqToDB<TRole, TKey> : _BaseIdentityRoleStore<TRole, TKey>
    , IQueryableRoleStore<TRole>
    , IRoleStore<TRole>
    where TRole : class, IIdentityRole<TKey>, IIdentityRoleWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStore_LinqToDB(IDbConnection db, IDataProvider dataProvider, Expression<Func<TRole, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber)
    : this(new DataConnection(dataProvider, db), baseSqlWhere, errorDescriber) { }

    public _BaseIdentityRoleStore_LinqToDB(DataConnection dataConnection, Expression<Func<TRole, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      this.dataConnection = dataConnection;
      BaseSqlWhere = baseSqlWhere;
    }

    private readonly DataConnection dataConnection;
    public Expression<Func<TRole, bool>> BaseSqlWhere { get; }

    #region IQueryableRoleStore
    public override IQueryable<TRole> Roles => dataConnection.GetTable<TRole>().Where(BaseSqlWhere);
    #endregion

    #region IRoleStore
    public override Func<TRole, string> GetRoleOrUserDescription => x => x.Name;

    protected override async Task<bool> CreateAsync_Insert(TRole role, CancellationToken cancellationToken = default) => await dataConnection.InsertAsync(role, token: cancellationToken) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TRole role, CancellationToken cancellationToken = default) => await dataConnection.DeleteAsync(role, token: cancellationToken) > 0;
    protected override async Task<bool> UpdateAsync_Update(TRole role, CancellationToken cancellationToken = default) => await dataConnection.UpdateAsync(role, token: cancellationToken) > 0;
    #endregion
  }
}
