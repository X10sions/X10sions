using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using xEFCore.xAS400.Infrastructure.Internal;

namespace xEFCore.xAS400.Query.Internal {
  public class AS400CompiledQueryCacheKeyGenerator : RelationalCompiledQueryCacheKeyGenerator {

    public AS400CompiledQueryCacheKeyGenerator(
           [NotNull] CompiledQueryCacheKeyGeneratorDependencies dependencies,
           [NotNull] RelationalCompiledQueryCacheKeyGeneratorDependencies relationalDependencies)
           : base(dependencies, relationalDependencies) {
    }

    public override object GenerateCacheKey(Expression query, bool async)
       => new AS400CompiledQueryCacheKey(
           GenerateCacheKeyCore(query, async),
           RelationalDependencies.ContextOptions.FindExtension<AS400OptionsExtension>()?.RowNumberPaging ?? false);

    struct AS400CompiledQueryCacheKey {
      readonly RelationalCompiledQueryCacheKey _relationalCompiledQueryCacheKey;
      readonly bool _useRowNumberOffset;

      public AS400CompiledQueryCacheKey(
          RelationalCompiledQueryCacheKey relationalCompiledQueryCacheKey, bool useRowNumberOffset) {
        _relationalCompiledQueryCacheKey = relationalCompiledQueryCacheKey;
        _useRowNumberOffset = useRowNumberOffset;
      }

      public override bool Equals(object obj)
          => !ReferenceEquals(null, obj)
             && obj is AS400CompiledQueryCacheKey
             && Equals((AS400CompiledQueryCacheKey)obj);

      bool Equals(AS400CompiledQueryCacheKey other)
         => _relationalCompiledQueryCacheKey.Equals(other._relationalCompiledQueryCacheKey)
            && _useRowNumberOffset == other._useRowNumberOffset;

      public override int GetHashCode() {
        unchecked {
          return (_relationalCompiledQueryCacheKey.GetHashCode() * 397) ^ _useRowNumberOffset.GetHashCode();
        }
      }

    }

  }
}
