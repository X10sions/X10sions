using System;
using System.Linq;

namespace NHibernate_v5_2.Linq {
  /// <summary>
  /// The extended <see cref="T:System.Linq.IQueryProvider" /> that supports setting options for underlying <see cref="T:NHibernate.IQuery" />.
  /// </summary>
  public interface IQueryProviderWithOptions : IQueryProvider {
    /// <summary>
    /// Creates a copy of a current provider with set query options.
    /// </summary>
    /// <param name="setOptions">An options setter.</param>
    /// <returns>A new <see cref="IQueryProvider"/> with options.</returns>
    IQueryProvider WithOptions(Action<NhQueryableOptions> setOptions);
  }
}