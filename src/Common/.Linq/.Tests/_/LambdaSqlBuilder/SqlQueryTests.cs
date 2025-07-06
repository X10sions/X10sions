using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using LambdaSqlBuilder.Tests.Entities;
using LambdaSqlBuilder.Tests.Infrastructure;
using NUnit.Framework;
using Dapper;
using FluentAssertions;

namespace LambdaSqlBuilder.Tests {
  [TestFixture]
  public class SqlQueryTests : TestBase {
    /// <summary>
    /// Find the product with name Tofu
    /// </summary>
    [Test]
    public void FindByFieldValue() {
      const string productName = "Tofu";

      var query = new SqlLam<Product>(p => p.ProductName == productName);

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(1);
      results.First().ProductName.Should().Be(productName);
    }

    /// <summary>
    /// Find products with the reorder level being 5, 15, or 25
    /// </summary>
    [Test]
    public void FindByListOfValues() {
      var reorderLevels = new object[] { 5, 15, 25 };

      var query = new SqlLam<Product>()
          .WhereIsIn(p => p.ReorderLevel, reorderLevels);

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(30);
      results.All(p => reorderLevels.Contains(p.ReorderLevel)).Should().BeTrue();
    }

    /// <summary>
    /// Find products by getting the product Ids first using a subquery
    /// </summary>
    [Test]
    public void FindBySubQuery() {
      var productNames = new object[] { "Konbu", "Tofu", "Pavlova" };

      var subQuery = new SqlLam<Product>()
          .WhereIsIn(p => p.ProductName, productNames)
          .Select(p => p.ProductId);

      var query = new SqlLam<Product>()
          .WhereIsIn(p => p.ProductId, subQuery);

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(3);
      results.All(p => productNames.Contains(p.ProductName)).Should().BeTrue();
    }

    /// <summary>
    /// Find products with the reorder level not being 5, 15, or 25
    /// </summary>
    [Test]
    public void FindByListOfValuesNegated() {
      var reorderLevels = new object[] { 5, 15, 25 };

      var query = new SqlLam<Product>()
          .WhereNotIn(p => p.ReorderLevel, reorderLevels);

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(47);
      results.All(p => !reorderLevels.Contains(p.ReorderLevel)).Should().BeTrue();
    }

    /// <summary>
    /// Find products not being included in a subquery
    /// </summary>
    [Test]
    public void FindBySubQueryNegated() {
      var productNames = new object[] { "Konbu", "Tofu", "Pavlova" };

      var subQuery = new SqlLam<Product>()
          .WhereIsIn(p => p.ProductName, productNames)
          .Select(p => p.ProductId);

      var query = new SqlLam<Product>()
          .WhereNotIn(p => p.ProductId, subQuery);

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(74);
      results.All(p => !productNames.Contains(p.ProductName)).Should().BeTrue();
    }

    /// <summary>
    /// Get product Tofu by its Id and select the value of the Unit Price only
    /// </summary>
    [Test]
    public void SelectField() {
      const int productId = 14;

      var query = new SqlLam<Product>(p => p.ProductId == productId)
          .Select(p => p.UnitPrice);

      var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(1);
      results.First().Should().Be(23.25m);
    }

    /// <summary>
    /// Get product Tofu by its Id and selects all its properties
    /// </summary>
    [Test]
    public void SelectAllFields() {
      const int productId = 14;

      var query = new SqlLam<Product>(p => p.ProductId == productId)
          .Select(p => p);

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(1);
    }

    /// <summary>
    /// Get categories sorted by name
    /// </summary>
    [Test]
    public void OrderEntitiesByField() {
      var query = new SqlLam<Category>()
          .OrderBy(p => p.CategoryName);

      var results = Connection.Query<Category>(query.QueryString, query.QueryParameters).ToList();

      for (int i = 1; i < results.Count; ++i) {
        string.CompareOrdinal(results[i - 1].CategoryName, results[i].CategoryName).Should().BeLessThanOrEqualTo(0);
      }
    }

    /// <summary>
    /// Get categories sorted by name descending
    /// </summary>
    [Test]
    public void OrderEntitiesByFieldDescending() {
      var query = new SqlLam<Category>()
          .OrderByDescending(p => p.CategoryName);

      var results = Connection.Query<Category>(query.QueryString, query.QueryParameters).ToList();

      for (int i = 1; i < results.Count; ++i) {
        string.CompareOrdinal(results[i - 1].CategoryName, results[i].CategoryName).Should().BeGreaterThanOrEqualTo(0);
      }
    }

    /// <summary>
    /// Get the number of all products
    /// </summary>
    [Test]
    public void SelectEntityCount() {
      var query = new SqlLam<Product>()
          .SelectCount(p => p.ProductId);

      var resultCount = Connection.Query<int>(query.QueryString, query.QueryParameters).Single();

      resultCount.Should().Be(77);
    }

    /// <summary>
    /// Select number of Product IDs for products with the Reorder Level equal to 25
    /// </summary>
    [Test]
    public void SelectRestrictedEntityCount() {
      var query = new SqlLam<Product>()
          .SelectCount(p => p.ProductId)
          .Where(p => p.ReorderLevel == 25);

      var resultCount = Connection.Query<int>(query.QueryString, query.QueryParameters).Single();

      resultCount.Should().Be(12);
    }

    /// <summary>
    /// Select number of products for individual Reorder Levels
    /// </summary>
    [Test]
    public void SelectGroupedCounts() {
      var groupSizes = new[] { 24, 8, 7, 10, 8, 12, 8 };

      var query = new SqlLam<Product>()
          .SelectCount(p => p.ProductId)
          .GroupBy(p => p.ReorderLevel)
          .OrderBy(p => p.ReorderLevel);

      var results = Connection.Query<int>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(groupSizes.Length);

      for (int i = 0; i < groupSizes.Length; ++i) {
        results[i].Should().Be(groupSizes[i]);
      }
    }

    /// <summary>
    /// Select all distinct possible values of the Reorder Level
    /// </summary>
    [Test]
    public void SelectDistinctValues() {
      var allValues = new[] { 0, 5, 10, 15, 20, 25, 30 };

      var query = new SqlLam<Product>()
          .SelectDistinct(p => p.ReorderLevel)
          .OrderBy(p => p.ReorderLevel);

      var results = Connection.Query<short>(query.QueryString, query.QueryParameters).ToList();

      results.Count.Should().Be(allValues.Length);
      for (int i = 0; i < allValues.Length; ++i) {
        allValues[i].Should().Be(results[i]);
      }
    }

    /// <summary>
    /// Select maximum unit price among all the products
    /// </summary>
    [Test]
    public void SelectMaximumValue() {
      const decimal maximumValue = 263.5M;

      var query = new SqlLam<Product>()
          .SelectMax(p => p.UnitPrice);

      var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

      results.Should().Be(maximumValue);
    }

    /// <summary>
    /// Select minimum unit price among all the products
    /// </summary>
    [Test]
    public void SelectMinimumValue() {
      const decimal minimumValue = 2.5M;

      var query = new SqlLam<Product>()
          .SelectMin(p => p.UnitPrice);

      var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

      results.Should().Be(minimumValue);
    }

    /// <summary>
    /// Select average unit price among all the products
    /// </summary>
    [Test]
    public void SelectAverageValue() {
      const decimal averageValue = 28.8663M;

      var query = new SqlLam<Product>()
          .SelectAverage(p => p.UnitPrice);

      var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

      results.Should().Be(averageValue);
    }

    /// <summary>
    /// Select sum of all unit prices among all the products
    /// </summary>
    [Test]
    public void SelectSum() {
      const decimal sum = 2222.71M;

      var query = new SqlLam<Product>()
          .SelectSum(p => p.UnitPrice);

      var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

      results.Should().Be(sum);
    }

    /// <summary>
    /// Select the product "Tofu" by listing its individual properties using the 'new' construct
    /// </summary>
    [Test]
    public void SelectWithNew() {
      const string productName = "Tofu";

      var query = new SqlLam<Product>()
          .Where(p => p.ProductName == productName)
          .Select(p => new {
            p.ProductId,
            p.ProductName,
            p.CategoryId,
            p.ReorderLevel,
            p.UnitPrice
          });

      var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).Single();

      results.ProductId.Should().NotBe(default);
      results.ProductName.Should().NotBeNull();
      results.CategoryId.Should().NotBe(default);
      results.ReorderLevel.Should().NotBe(default);
      results.UnitPrice.Should().NotBe(default);
      results.EnglishName.Should().BeNull();
    }

    /// <summary>
    /// Load products with reorder level 0 page by page
    /// </summary>
    [Test]
    public void PaginateOverResults() {
      const int reorderLevel = 0;
      const int pageSize = 5;
      const int numberOfPages = 5;
      const int lastPageSize = 4;

      var query = new SqlLam<Product>(p => p.ReorderLevel == reorderLevel)
          .OrderBy(p => p.ProductName);

      for (int page = 1; page < numberOfPages; ++page) {
        var results = Connection.Query<Product>(query.QueryStringPage(pageSize, page), query.QueryParameters).ToList();
        results.Count.Should().Be(pageSize);
        results.All(p => p.ReorderLevel == reorderLevel).Should().BeTrue();
      }

      var lastResults = Connection.Query<Product>(query.QueryStringPage(pageSize, numberOfPages), query.QueryParameters).ToList();
      lastResults.Count.Should().Be(lastPageSize);
      lastResults.All(p => p.ReorderLevel == reorderLevel).Should().BeTrue();
    }

    /// <summary>
    /// Load first 10 products sorted by the product name with reorder level 0 
    /// </summary>
    [Test]
    public void TopTenResults() {
      const int reorderLevel = 0;
      const int pageSize = 10;

      var query = new SqlLam<Product>(p => p.ReorderLevel == reorderLevel)
          .OrderBy(p => p.ProductName);

      var results = Connection.Query<Product>(query.QueryStringPage(pageSize), query.QueryParameters).ToList();
      results.Count.Should().Be(pageSize);
      results.All(p => p.ReorderLevel == reorderLevel).Should().BeTrue();
    }
  }
}
