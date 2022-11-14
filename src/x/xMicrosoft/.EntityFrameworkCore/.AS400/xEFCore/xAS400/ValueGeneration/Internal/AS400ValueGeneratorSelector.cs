using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using xEFCore.xAS400.Metadata;
using xEFCore.xAS400.Storage.Internal;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public class AS400ValueGeneratorSelector : RelationalValueGeneratorSelector {

    public AS400ValueGeneratorSelector(
         [NotNull] ValueGeneratorSelectorDependencies dependencies,
         [NotNull] IAS400SequenceValueGeneratorFactory sequenceFactory,
         [NotNull] IAS400RelationalConnection connection)
         : base(dependencies) {
      Check.NotNull(sequenceFactory, nameof(sequenceFactory));
      Check.NotNull(connection, nameof(connection));

      _sequenceFactory = sequenceFactory;
      _connection = connection;
    }

    readonly IAS400SequenceValueGeneratorFactory _sequenceFactory;
    readonly IAS400RelationalConnection _connection;

    public new virtual IAS400ValueGeneratorCache Cache => (IAS400ValueGeneratorCache)base.Cache;

    public override ValueGenerator Select(IProperty property, IEntityType entityType) {
      Check.NotNull(property, nameof(property));
      Check.NotNull(entityType, nameof(entityType));

      return property.GetValueGeneratorFactory() == null
             && property.AS400().ValueGenerationStrategy == AS400ValueGenerationStrategy.SequenceHiLo
          ? _sequenceFactory.Create(property, Cache.GetOrAddSequenceState(property), _connection)
          : base.Select(property, entityType);
    }

    public override ValueGenerator Create(IProperty property, IEntityType entityType) {
      Check.NotNull(property, nameof(property));
      Check.NotNull(entityType, nameof(entityType));

      return property.ClrType.UnwrapNullableType() == typeof(Guid)
          ? property.ValueGenerated == ValueGenerated.Never
            || property.AS400().DefaultValueSql != null
              ? (ValueGenerator)new TemporaryGuidValueGenerator()
              : new SequentialGuidValueGenerator()
          : base.Create(property, entityType);
    }

  }
}