using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NHibernate_v5_2.Linq {
  /// <summary>
  /// An update builder on which values to update can be specified.
  /// </summary>
  /// <typeparam name="TSource">The type of the entities to update.</typeparam>
  public partial class UpdateBuilder<TSource> {
    private readonly IQueryable<TSource> _source;
    private readonly Assignments<TSource, TSource> _assignments = new Assignments<TSource, TSource>();

    internal UpdateBuilder(IQueryable<TSource> source) {
      _source = source;
    }

    /// <summary>
    /// Set the specified property and return this builder.
    /// </summary>
    /// <typeparam name="TProp">The type of the property.</typeparam>
    /// <param name="property">The property.</param>
    /// <param name="expression">The expression that should be assigned to the property.</param>
    /// <returns>This update builder.</returns>
    public UpdateBuilder<TSource> Set<TProp>(Expression<Func<TSource, TProp>> property, Expression<Func<TSource, TProp>> expression) {
      _assignments.Set(property, expression);
      return this;
    }

    /// <summary>
    /// Set the specified property and return this builder.
    /// </summary>
    /// <typeparam name="TProp">The type of the property.</typeparam>
    /// <param name="property">The property.</param>
    /// <param name="value">The value.</param>
    /// <returns>This update builder.</returns>
    public UpdateBuilder<TSource> Set<TProp>(Expression<Func<TSource, TProp>> property, TProp value) {
      _assignments.Set(property, value);
      return this;
    }

    /// <summary>
    /// Update the entities. The update operation is performed in the database without reading the entities out of it.
    /// </summary>
    /// <returns>The number of updated entities.</returns>
    public int Update() => _source.ExecuteUpdate(DmlExpressionRewriter.PrepareExpression<TSource>(_source.Expression, _assignments.List), false);

    /// <summary>
    /// Perform an <c>update versioned</c> on the entities. The update operation is performed in the database without reading the entities out of it.
    /// </summary>
    /// <returns>The number of updated entities.</returns>
    public int UpdateVersioned() => _source.ExecuteUpdate(DmlExpressionRewriter.PrepareExpression<TSource>(_source.Expression, _assignments.List), true);
  }

  public partial class UpdateBuilder<TSource> {

    /// <summary>
    /// Update the entities. The update operation is performed in the database without reading the entities out of it.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
    /// <returns>The number of updated entities.</returns>
    public Task<int> UpdateAsync(CancellationToken cancellationToken = default(CancellationToken)) {
      if (cancellationToken.IsCancellationRequested) {
        return Task.FromCanceled<int>(cancellationToken);
      }
      try {
        return _source.ExecuteUpdateAsync(DmlExpressionRewriter.PrepareExpression<TSource>(_source.Expression, _assignments.List), false, cancellationToken);
      } catch (Exception ex) {
        return Task.FromException<int>(ex);
      }
    }

    /// <summary>
    /// Perform an <c>update versioned</c> on the entities. The update operation is performed in the database without reading the entities out of it.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
    /// <returns>The number of updated entities.</returns>
    public Task<int> UpdateVersionedAsync(CancellationToken cancellationToken = default(CancellationToken)) {
      if (cancellationToken.IsCancellationRequested) {
        return Task.FromCanceled<int>(cancellationToken);
      }
      try {
        return _source.ExecuteUpdateAsync(DmlExpressionRewriter.PrepareExpression<TSource>(_source.Expression, _assignments.List), true, cancellationToken);
      } catch (Exception ex) {
        return Task.FromException<int>(ex);
      }
    }
  }
}