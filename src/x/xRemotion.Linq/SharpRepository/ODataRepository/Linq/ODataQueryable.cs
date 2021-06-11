using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System.Linq;
using System.Linq.Expressions;

namespace SharpRepository.ODataRepository.Linq {
  /// <summary>
  /// Provides the main entry point to a LINQ query.
  /// </summary>
  public class ODataQueryable<T> : QueryableBase<T> {
    // https://github.com/SharpRepository/SharpRepository/blob/odata/SharpRepository.ODataRepository/Linq/ODataQueryable.cs

    private static IQueryExecutor CreateExecutor(string url, string databaseName) => new ODataQueryExecutor(url, databaseName);

    // This constructor is called by our users, create a new IQueryExecutor.
    public ODataQueryable(string url, string databaseName)
      : base(QueryParser.CreateDefault(), CreateExecutor(url, databaseName)) { }

    // This constructor is called indirectly by LINQ's query methods, just pass to base.
    public ODataQueryable(IQueryProvider provider, Expression expression)
        : base(provider, expression) {
    }
  }

}