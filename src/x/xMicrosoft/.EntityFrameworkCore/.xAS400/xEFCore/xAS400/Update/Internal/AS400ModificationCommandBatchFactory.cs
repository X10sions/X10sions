using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using xEFCore.xAS400.Infrastructure.Internal;

namespace xEFCore.xAS400.Update.Internal {
  public class AS400ModificationCommandBatchFactory : IModificationCommandBatchFactory {

    public AS400ModificationCommandBatchFactory(
       [NotNull] IRelationalCommandBuilderFactory commandBuilderFactory,
       [NotNull] ISqlGenerationHelper sqlGenerationHelper,
       [NotNull] IAS400UpdateSqlGenerator updateSqlGenerator,
       [NotNull] IRelationalValueBufferFactoryFactory valueBufferFactoryFactory,
       [NotNull] IDbContextOptions options,
       [NotNull] IRelationalConnection connection) {
      Check.NotNull(commandBuilderFactory, nameof(commandBuilderFactory));
      Check.NotNull(sqlGenerationHelper, nameof(sqlGenerationHelper));
      Check.NotNull(updateSqlGenerator, nameof(updateSqlGenerator));
      Check.NotNull(valueBufferFactoryFactory, nameof(valueBufferFactoryFactory));
      Check.NotNull(options, nameof(options));
      Check.NotNull(connection, nameof(connection));

      _commandBuilderFactory = commandBuilderFactory;
      _sqlGenerationHelper = sqlGenerationHelper;
      _updateSqlGenerator = updateSqlGenerator;
      _valueBufferFactoryFactory = valueBufferFactoryFactory;
      _options = options;
      _connection = connection;
    }

    readonly IRelationalCommandBuilderFactory _commandBuilderFactory;
    readonly ISqlGenerationHelper _sqlGenerationHelper;
    readonly IAS400UpdateSqlGenerator _updateSqlGenerator;
    readonly IRelationalValueBufferFactoryFactory _valueBufferFactoryFactory;
    readonly IDbContextOptions _options;
    readonly IRelationalConnection _connection;

    public virtual ModificationCommandBatch Create() {
      var optionsExtension = _options.Extensions.OfType<AS400OptionsExtension>().FirstOrDefault();
      return new AS400ModificationCommandBatch(
          _commandBuilderFactory,
          _sqlGenerationHelper,
          _updateSqlGenerator,
          _valueBufferFactoryFactory,
          _connection,
          optionsExtension?.MaxBatchSize);
    }

  }
}
