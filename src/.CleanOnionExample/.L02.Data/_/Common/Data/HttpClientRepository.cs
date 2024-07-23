using Common.Domain;
using Common.Domain.Repositories;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace Common.Data;

public interface IHttpClientRepository<T> : IRepositoryAsync<T, HttpClient, IQueryable<T>> where T : class { }
public class HttpClientQuery<T> : IQuery<T> where T : class {
  public HttpClientQuery(HttpClient httpClient, string baseApiPath) {
    //  Queryable = queryable;
    Database = httpClient;
    BaseApiPath = baseApiPath;
  }
  public IQueryable<T> Queryable { get; }
  HttpClient? Database;
  public string BaseApiPath { get; }

  public  async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) =>  await Database.GetFromJsonAsync<bool>($"/api{BaseApiPath}/any", token);
  public  async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Database.GetFromJsonAsync<int>($"/api{BaseApiPath}/count", token);
  public  async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Database.GetFromJsonAsync<T?>($"/api{BaseApiPath}/get", token);
  public  async Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) {
    var response = await Database.PostAsJsonAsync($"/api{BaseApiPath}/list", token);
    return await response.Content.ReadFromJsonAsync<List<T>>(cancellationToken: token);
  }
}

public  class HttpClientRepository<T> : IHttpClientRepository<T> where T : class {
  public HttpClientRepository(HttpClient httpClient, string baseApiPath) {
    Database = httpClient;
    Table = null;
    Query = new HttpClientQuery<T>(httpClient, baseApiPath);
    BaseApiPath = baseApiPath; ;
  }

  public HttpClient Database { get; }
  public IQueryable<T> Table { get; }
  public IQuery<T> Query { get; }
  //public IQueryable<T> Queryable => Table;
  public string  BaseApiPath{ get; }


  #region IWriteRepository
  public  async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      var response = await Database.PostAsJsonAsync($"/api{BaseApiPath}/delete", row, token);
      var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: token);
      rowCount++;
    }
    return rowCount;
  }

  public  async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      var response = await Database.PostAsJsonAsync($"/api{BaseApiPath}/add", row, token);
      var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: token);
      rowCount++;
    }
    return rowCount;
  }
  [Obsolete(nameof(NotImplementedException))]
  public async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey> idSelector, CancellationToken token = default) => throw new NotImplementedException();

  [Obsolete(nameof(NotImplementedException))]
  public async Task<int> UpdateAsync(T row, CancellationToken token = default) => throw new NotImplementedException();
  #endregion
}

public static class HttpClientRepositoryExtensions {
  public static async Task<T?> GetByIdAsync<T, TId>(this HttpClientRepository<T> repo, TId key, CancellationToken token = default) where T : class where TId : notnull {
    var response = await repo.Database.PostAsJsonAsync($"/api{repo.BaseApiPath}/getById", key, token);
    return await response.Content.ReadFromJsonAsync<T>(cancellationToken: token);
  }

}