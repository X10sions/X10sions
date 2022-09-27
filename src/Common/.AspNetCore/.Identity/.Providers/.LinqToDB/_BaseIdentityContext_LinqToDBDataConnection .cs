using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using System.Data.Common;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class _BaseIdentityContext_LinqToDBDataConnection : DataConnection, IIdentityContext_LinqToDB {
    public async Task DbDeleteAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.DeleteAsync(data, token: cancellationToken);
    public async Task DbInsertAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.InsertAsync(data, token: cancellationToken);
    public async Task DbUpdateAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.UpdateAsync(data, token: cancellationToken);
    public IQueryable<T> DbGetQueryable<T>() where T : class => this.GetTable<T>();

    public _BaseIdentityContext_LinqToDBDataConnection(IDataProvider dataProvider, string connectionString) : base(dataProvider, connectionString) { }
    public _BaseIdentityContext_LinqToDBDataConnection(IDataProvider dataProvider, DbConnection connection) : base(dataProvider, connection) { }
    public _BaseIdentityContext_LinqToDBDataConnection(DataConnection dataConnection) : base(dataConnection.DataProvider, dataConnection.Connection) { }
  }
}
