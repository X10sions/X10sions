using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public interface IAS400ValueGeneratorCache : IValueGeneratorCache {
    AS400SequenceValueGeneratorState GetOrAddSequenceState([NotNull] IProperty property);
  }
}
