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
  public class _BaseIdentityUserStore_LinqToDB<TUser, TKey> : _BaseIdentityUserStore<TUser, TKey>
    , IQueryableUserStore<TUser>
    , IUserEmailStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : class, IIdentityUser<TKey>, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityUserStore_LinqToDB(IDbConnection db, IDataProvider dataProvider, Expression<Func<TUser, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber)
      : this(new DataConnection(dataProvider, db), baseSqlWhere, errorDescriber) { }

    public _BaseIdentityUserStore_LinqToDB(DataConnection dataConnection, Expression<Func<TUser, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      this.dataConnection = dataConnection;
      BaseSqlWhere = baseSqlWhere;
    }

    private readonly DataConnection dataConnection;
    public Expression<Func<TUser, bool>> BaseSqlWhere { get; }

    #region IQueryableUserStore
    public override IQueryable<TUser> Users => dataConnection.GetTable<TUser>().Where(BaseSqlWhere);
    #endregion

    #region IUserStore
    protected override async Task<bool> CreateAsync_Insert(TUser user, CancellationToken cancellationToken = default) => await dataConnection.InsertAsync(user, token: cancellationToken) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TUser user, CancellationToken cancellationToken = default) => await dataConnection.DeleteAsync(user, token: cancellationToken) > 0;
    protected override async Task<bool> UpdateAsync_Update(TUser user, CancellationToken cancellationToken = default) => await dataConnection.UpdateAsync(user, token: cancellationToken) > 0;
    #endregion

    #region IUserEmailStore
    #endregion

    #region IUserPasswordStore
    #endregion

  }
}
