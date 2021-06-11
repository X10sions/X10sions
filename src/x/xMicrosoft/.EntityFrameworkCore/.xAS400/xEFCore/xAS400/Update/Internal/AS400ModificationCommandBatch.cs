using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace xEFCore.xAS400.Update.Internal {
  public class AS400ModificationCommandBatch : AffectedCountModificationCommandBatch {

    public AS400ModificationCommandBatch(
      [NotNull] IRelationalCommandBuilderFactory commandBuilderFactory,
      [NotNull] ISqlGenerationHelper sqlGenerationHelper,
      [NotNull] IAS400UpdateSqlGenerator updateSqlGenerator,
      [NotNull] IRelationalValueBufferFactoryFactory valueBufferFactoryFactory,
      [NotNull] IRelationalConnection connection, //TODO: GetRidOf?
      [CanBeNull] int? maxBatchSize
      ) : base(
        commandBuilderFactory,
        sqlGenerationHelper,
        updateSqlGenerator,
        valueBufferFactoryFactory
        ) {
      if (maxBatchSize.HasValue && maxBatchSize.Value <= 0) {
        throw new ArgumentOutOfRangeException(nameof(maxBatchSize), RelationalStrings.InvalidMaxBatchSize);
      }
      _connection = connection;
      _maxBatchSize = Math.Min(maxBatchSize ?? int.MaxValue, MaxRowCount);
    }

    const int DefaultNetworkPacketSizeBytes = 4096;
    const int MaxScriptLength = 65536 * DefaultNetworkPacketSizeBytes / 2;
    const int MaxParameterCount = 2100;
    const int MaxRowCount = 1000;

    int _parameterCount = 1; // Implicit parameter for the command text
    readonly int _maxBatchSize;
    readonly List<ModificationCommand> _bulkInsertCommands = new List<ModificationCommand>();
    int _commandsLeftToLengthCheck = 50;
    readonly IRelationalConnection _connection;

    protected new virtual IAS400UpdateSqlGenerator UpdateSqlGenerator => (IAS400UpdateSqlGenerator)base.UpdateSqlGenerator;

    protected override bool CanAddCommand(ModificationCommand modificationCommand) {
      if (ModificationCommands.Count >= _maxBatchSize) {
        return false;
      }
      var additionalParameterCount = CountParameters(modificationCommand);
      if (_parameterCount + additionalParameterCount >= MaxParameterCount) {
        return false;
      }
      _parameterCount += additionalParameterCount;
      return true;
    }

    protected override bool IsCommandTextValid() {
      if (--_commandsLeftToLengthCheck < 0) {
        var commandTextLength = GetCommandText().Length;
        if (commandTextLength >= MaxScriptLength) {
          return false;
        }
        var avarageCommandLength = commandTextLength / ModificationCommands.Count;
        var expectedAdditionalCommandCapacity = (MaxScriptLength - commandTextLength) / avarageCommandLength;
        _commandsLeftToLengthCheck = Math.Max(1, expectedAdditionalCommandCapacity / 4);
      }
      return true;
    }

    protected override int GetParameterCount() => _parameterCount;

    static int CountParameters(ModificationCommand modificationCommand) {
      var parameterCount = 0;
      foreach (var columnModification in modificationCommand.ColumnModifications) {
        #region "db2"
        //if (columnModification.ParameterName != null) {
        //  parameterCount++;
        //}
        //if (columnModification.OriginalParameterName != null) {
        //  parameterCount++;
        //}
        #endregion
        #region "sqlserver"
        if (columnModification.UseCurrentValueParameter) {
          parameterCount++;
        }
        if (columnModification.UseOriginalValueParameter) {
          parameterCount++;
        }
        #endregion
      }
      return parameterCount;
    }

    protected override void ResetCommandText() {
      base.ResetCommandText();
      _bulkInsertCommands.Clear();
    }

    protected override string GetCommandText() => base.GetCommandText() + GetBulkInsertCommandText(ModificationCommands.Count);

    string GetBulkInsertCommandText(int lastIndex) {
      if (_bulkInsertCommands.Count == 0) {
        return string.Empty;
      }
      var stringBuilder = new StringBuilder();
      var resultSetMapping = UpdateSqlGenerator.AppendBulkInsertOperation(stringBuilder, _bulkInsertCommands, lastIndex - _bulkInsertCommands.Count);
      for (var i = lastIndex - _bulkInsertCommands.Count; i < lastIndex; i++) {
        CommandResultSet[i] = resultSetMapping;
      }
      if (resultSetMapping != ResultSetMapping.NoResultSet) {
        CommandResultSet[lastIndex - 1] = ResultSetMapping.LastInResultSet;
      }
      return stringBuilder.ToString();
    }

    protected override void UpdateCachedCommandText(int commandPosition) {
      var newModificationCommand = ModificationCommands[commandPosition];
      if (newModificationCommand.EntityState == EntityState.Added) {
        if (_bulkInsertCommands.Count > 0 && !CanBeInsertedInSameStatement(_bulkInsertCommands[0], newModificationCommand)) {
          CachedCommandText.Append(GetBulkInsertCommandText(commandPosition));
          _bulkInsertCommands.Clear();
        }
        _bulkInsertCommands.Add(newModificationCommand);
        LastCachedCommandIndex = commandPosition;
      } else {
        CachedCommandText.Append(GetBulkInsertCommandText(commandPosition));
        _bulkInsertCommands.Clear();
        base.UpdateCachedCommandText(commandPosition);
      }
    }

    static bool CanBeInsertedInSameStatement(ModificationCommand firstCommand, ModificationCommand secondCommand)
      => string.Equals(firstCommand.TableName, secondCommand.TableName, StringComparison.Ordinal)
      && string.Equals(firstCommand.Schema, secondCommand.Schema, StringComparison.Ordinal)
      && GetWriteColumnModificationsNames(firstCommand).SequenceEqual(GetWriteColumnModificationsNames(secondCommand))
      && GetReadColumnModificationsNames(firstCommand).SequenceEqual(GetReadColumnModificationsNames(secondCommand));

    static IEnumerable<string> GetWriteColumnModificationsNames(ModificationCommand cmd) {
      return from o in cmd.ColumnModifications where o.IsWrite select o.ColumnName;
    }

    static IEnumerable<string> GetReadColumnModificationsNames(ModificationCommand cmd) {
      return from o in cmd.ColumnModifications where o.IsRead select o.ColumnName;
    }
    
    #region "db2"

    //public override void Execute(IRelationalConnection connection) {
    //  Check.NotNull(connection, "connection");
    //  RawSqlCommand rawSqlCommand = CreateStoreCommand();
    //  try {
    //    using (RelationalDataReader reader = rawSqlCommand.RelationalCommand.ExecuteReader(connection, rawSqlCommand.ParameterValues)) {
    //      Consume(reader);
    //    }
    //  } catch (DbUpdateException) {
    //    throw;
    //  } catch (Exception innerException) {
    //    throw new DbUpdateException(RelationalStrings.UpdateStoreException, innerException);
    //  }
    //}

    //protected override void Consume([NotNull] RelationalDataReader reader) {
    //  int num = 0;
    //  try {
    //    int num2 = 0;
    //    do {
    //      num = (ModificationCommands[num].RequiresResultPropagation ? ConsumeResultSetWithPropagation(num, reader) : ConsumeResultSetWithoutPropagation(num, reader));
    //      num2++;
    //    }
    //    while (num < CommandResultSet.Count && reader.DbDataReader.NextResult());
    //  } catch (DbUpdateException) {
    //    throw;
    //  } catch (Exception innerException) {
    //    throw new DbUpdateException(RelationalStrings.UpdateStoreException, innerException, ModificationCommands[num].Entries);
    //  }
    //}

    //protected override int ConsumeResultSetWithPropagation(int commandIndex, [NotNull] RelationalDataReader reader) {
    //  int num = 0;
    //  do {
    //    ModificationCommand modificationCommand = ModificationCommands[commandIndex];
    //    if (!reader.Read()) {
    //      int num2 = num + 1;
    //      while (++commandIndex < CommandResultSet.Count && CommandResultSet[commandIndex - 1] == ResultSetMapping.NotLastInResultSet) {
    //        num2++;
    //      }
    //      ThrowAggregateUpdateConcurrencyException(commandIndex, num2, num);
    //    }
    //    IRelationalValueBufferFactory relationalValueBufferFactory = CreateValueBufferFactory(modificationCommand.ColumnModifications);
    //    modificationCommand.PropagateResults(relationalValueBufferFactory.Create(reader.DbDataReader));
    //    num++;
    //  }
    //  while (++commandIndex < CommandResultSet.Count && CommandResultSet[commandIndex - 1] == ResultSetMapping.NotLastInResultSet);
    //  return commandIndex;
    //}

    //protected override int ConsumeResultSetWithoutPropagation(int commandIndex, [NotNull] RelationalDataReader reader) {
    //  int num = 1;
    //  while (++commandIndex < CommandResultSet.Count) {
    //    num++;
    //  }
    //  int recordsAffected = reader.DbDataReader.RecordsAffected;
    //  if (recordsAffected != num) {
    //    ThrowAggregateUpdateConcurrencyException(commandIndex, num, recordsAffected);
    //  }
    //  return commandIndex;
    //}

    #endregion

  }
}