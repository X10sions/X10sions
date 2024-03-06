using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using xEFCore.xAS400.Metadata;

namespace xEFCore.xAS400.Update.Internal {
  public class AS400UpdateSqlGenerator : UpdateSqlGenerator, IAS400UpdateSqlGenerator {

    public AS400UpdateSqlGenerator(
      [NotNull] UpdateSqlGeneratorDependencies dependencies,
      [NotNull] IRelationalTypeMapper typeMapper
      ) : base(dependencies) {
      _typeMapper = typeMapper;
    }

    readonly IRelationalTypeMapper _typeMapper;
    readonly EFCoreConstants_AS400 _constants = new EFCoreConstants_AS400();

    public virtual ResultSetMapping AppendBulkInsertOperation(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition) {
      if (modificationCommands.Count == 1 && modificationCommands[0].ColumnModifications.All(o => !o.IsKey || !o.IsRead || o.Property.AS400().ValueGenerationStrategy == AS400ValueGenerationStrategy.IdentityColumn)) {
        return AppendInsertOperation(commandStringBuilder, modificationCommands[0], commandPosition);
      }

      var readOperations = modificationCommands[0].ColumnModifications.Where(o => o.IsRead).ToList();
      var writeOperations = modificationCommands[0].ColumnModifications.Where(o => o.IsWrite).ToList();
      var keyOperations = modificationCommands[0].ColumnModifications.Where(o => o.IsKey).ToList();
      var defaultValuesOnly = writeOperations.Count == 0;
      var nonIdentityOperations = (from o in modificationCommands[0].ColumnModifications where o.Property.AS400().ValueGenerationStrategy != AS400ValueGenerationStrategy.IdentityColumn select o).ToList();
      if (defaultValuesOnly) {
        if (nonIdentityOperations.Count == 0 || readOperations.Count == 0) {
          foreach (var modification in modificationCommands) {
            AppendInsertOperation(commandStringBuilder, modification, commandPosition);
          }
          return readOperations.Count == 0 ? ResultSetMapping.NoResultSet : ResultSetMapping.LastInResultSet;
        }
        if (nonIdentityOperations.Count > 1) {
          nonIdentityOperations = new List<ColumnModification> { nonIdentityOperations.First() };
        }
      }
      if (readOperations.Count == 0) {
        return AppendBulkInsertWithoutServerValues(commandStringBuilder, modificationCommands, writeOperations);
      }
      //SQLSERVER      if (defaultValuesOnly) {
      //SQLSERVER      throw new Exception("TODO");
      //SQLSERVER      //TODO  return AppendBulkInsertWithServerValuesOnly(commandStringBuilder, modificationCommands, commandPosition, nonIdentityOperations, keyOperations, readOperations);
      //SQLSERVER    }
      //SQLSERVER      if (modificationCommands[0].Entries.SelectMany(e => e.EntityType.GetAllBaseTypesInclusive()).Any(e => e.AS400().IsMemoryOptimized)) {
      //SQLSERVER        if (!nonIdentityOperations.Any(o => o.IsRead && o.IsKey)) {
      //SQLSERVER      foreach (var modification in modificationCommands) {
      //SQLSERVER      AppendInsertOperation(commandStringBuilder, modification, commandPosition++);
      //SQLSERVER    }
      //SQLSERVER    } else {
      //SQLSERVER      foreach (var modification in modificationCommands) {
      //SQLSERVER      throw new Exception("TODO");
      //SQLSERVER            //TODO AppendInsertOperationWithServerKeys(commandStringBuilder, modification, keyOperations, readOperations, commandPosition++);
      //SQLSERVER    }
      //SQLSERVER    }
      //SQLSERVER      return ResultSetMapping.LastInResultSet;
      //SQLSERVER    }
      //SQLSERVER      throw new Exception("TODO");
      //SQLSERVER      //TODO return AppendBulkInsertWithServerValues(commandStringBuilder, modificationCommands, commandPosition, writeOperations, keyOperations, readOperations);

      foreach (var modification in modificationCommands) {
        AppendInsertOperation(commandStringBuilder, modification, commandPosition);
      }
      return ResultSetMapping.LastInResultSet;
    }

    ResultSetMapping AppendBulkInsertWithoutServerValues(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, List<ColumnModification> writeOperations) {
      Debug.Assert(writeOperations.Count > 0);
      var name = modificationCommands[0].TableName;
      var schema = modificationCommands[0].Schema;
      AppendInsertCommandHeader(commandStringBuilder, name, schema, writeOperations);
      AppendValuesHeader(commandStringBuilder, writeOperations);
      AppendValues(commandStringBuilder, writeOperations);
      for (var i = 1; i < modificationCommands.Count; i++) {
        commandStringBuilder.Append(",").AppendLine();
        AppendValues(commandStringBuilder, modificationCommands[i].ColumnModifications.Where(o => o.IsWrite).ToList());
      }
      commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator).AppendLine();
      return ResultSetMapping.NoResultSet;
    }

    protected override void AppendIdentityWhereCondition([NotNull] StringBuilder commandStringBuilder, [NotNull] ColumnModification columnModification) {
      SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
      commandStringBuilder.Append($" = {_constants.IdentityColumnSql}");
    }
    protected override void AppendRowsAffectedWhereCondition([NotNull] StringBuilder commandStringBuilder, int expectedRowsAffected)
      => commandStringBuilder.Append($"{_constants.RowCountColumnSql} = {expectedRowsAffected.ToString(CultureInfo.InvariantCulture)}");

    protected override ResultSetMapping AppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string name, string schema, int commandPosition) {
      commandStringBuilder.Append($"SELECT {_constants.RowCountColumnSql}{SqlGenerationHelper.StatementTerminator}").AppendLine().AppendLine();
      return ResultSetMapping.LastInResultSet;
    }

  }
}