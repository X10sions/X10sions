﻿using Remotion.Linq;
using SharpRepository.ODataRepository.Linq.QueryGeneration;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.ODataRepository.Linq {
  // Called by re-linq when a query is to be executed.
  public class ODataQueryExecutor : IQueryExecutor {
    private readonly string _url;
    private readonly string _databaseName;

    public ODataQueryExecutor(string url, string databaseName) {
      _url = url;
      _databaseName = databaseName;
    }

    // Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
    public T ExecuteScalar<T>(QueryModel queryModel) => ExecuteCollection<T>(queryModel).Single();

    // Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
    public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty) => returnDefaultWhenEmpty ? ExecuteCollection<T>(queryModel).SingleOrDefault() : ExecuteCollection<T>(queryModel).Single();

    // Executes a query with a collection result.
    public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel) {
      var commandData = ODataApiGeneratorQueryModelVisitor.GenerateODataApiQuery(queryModel);
      var query = commandData.CreateQuery(_url, _databaseName);
      return query.Enumerable<T>();
    }
  }

}