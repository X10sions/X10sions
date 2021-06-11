using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using xEFCore.xAS400.Storage.Internal;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public interface IAS400SequenceValueGeneratorFactory {
    ValueGenerator Create(
           [NotNull] IProperty property,
           [NotNull] AS400SequenceValueGeneratorState generatorState,
           [NotNull] IAS400RelationalConnection connection);
  }
}
