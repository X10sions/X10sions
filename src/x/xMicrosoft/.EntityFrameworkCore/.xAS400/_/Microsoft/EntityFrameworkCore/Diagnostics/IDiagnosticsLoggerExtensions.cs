using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using xEFCore;

namespace Microsoft.EntityFrameworkCore.Diagnostics {
  public static class IDiagnosticsLoggerExtensions {

    public static void ByteIdentityColumnWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics,
      [NotNull] IProperty property) {
      var definition = EFCoreStrings.LogByteIdentityColumn;
      // Checking for enabled here to avoid string formatting if not needed.
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(diagnostics, property.Name, property.DeclaringEntityType.DisplayName());
      }
      if (diagnostics.DiagnosticSource.IsEnabled(definition.EventId.Name)) {
        diagnostics.DiagnosticSource.Write(
            definition.EventId.Name,
            new PropertyEventData(definition, ByteIdentityColumnWarning, property));
      }
    }

    static string ByteIdentityColumnWarning(EventDefinitionBase definition, EventData payload) {
      var d = (EventDefinition<string, string>)definition;
      var p = (PropertyEventData)payload;
      return d.GenerateMessage(p.Property.Name, p.Property.DeclaringEntityType.DisplayName());
    }

    public static void ColumnFound(
    [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
    [CanBeNull] string tableName,
    [CanBeNull] string columnName,
    [CanBeNull] string dataTypeName,
    int? ordinal,
    bool? nullable,
    int? primaryKeyOrdinal,
    [CanBeNull] string defaultValue,
    [CanBeNull] string computedValue,
    int? precision,
    int? scale,
    int? maxLength,
    [CanBeNull] bool? identity,
    [CanBeNull] bool? computed) {
      var definition = EFCoreStrings.LogFoundColumnName;
      Debug.Assert(LogLevel.Debug == definition.Level);
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(
            diagnostics,
            l => l.LogDebug(
                definition.EventId,
                null,
                definition.MessageFormat,
                tableName,
                columnName,
                dataTypeName,
                ordinal,
                nullable,
                primaryKeyOrdinal,
                defaultValue,
                computedValue,
                precision,
                scale,
                maxLength,
                identity,
                computed));
      }
    }

    public static void ColumnFoundName([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName, [CanBeNull] string columnName, [CanBeNull] string dataTypeName, int? ordinal, bool? nullable, int? primaryKeyOrdinal, [CanBeNull] string defaultValue, [CanBeNull] string computedValue, int? precision, int? scale, int? maxLength, [CanBeNull] bool? identity, [CanBeNull] bool? computed) {
      FallbackEventDefinition definition = EFCoreStrings.LogFoundColumnName;
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(diagnostics, delegate (ILogger l) {
          l.LogDebug(definition.EventId, null, definition.MessageFormat, tableName, columnName, dataTypeName, ordinal, nullable, primaryKeyOrdinal, defaultValue, computedValue, precision, scale, maxLength, identity, computed);
        });
      }
    }

    public static void ColumnFoundDefinition(
        [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
        [CanBeNull] string tableName,
        [CanBeNull] string columnName,
        [CanBeNull] string dataTypeName,
        bool notNull,
        [CanBeNull] string defaultValue)
        => EFCoreStrings.LogFoundColumnDefinition.Log(diagnostics, tableName, columnName, dataTypeName, notNull, defaultValue);

    public static void ColumnNotNamedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName) => EFCoreStrings.LogColumnNameEmptyOnTable.Log(diagnostics, tableName);
    public static void ColumnSkipped([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName, [CanBeNull] string columnName) => EFCoreStrings.LogColumnNotInSelectionSet.Log(diagnostics, columnName, tableName);

    public static void DecimalTypeDefaultWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics,
      [NotNull] IProperty property
      ) {
      var definition = EFCoreStrings.LogDefaultDecimalTypeColumn;
      var logDefaultDecimalTypeColumn = EFCoreStrings.LogDefaultDecimalTypeColumn;
      // Checking for enabled here to avoid string formatting if not needed.
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(diagnostics, property.Name, property.DeclaringEntityType.DisplayName());
      }
      if (diagnostics.DiagnosticSource.IsEnabled(definition.EventId.Name)) {
        diagnostics.DiagnosticSource.Write(
            definition.EventId.Name,
            new PropertyEventData(definition, DecimalTypeDefaultWarning, property));
      }
    }

    static string DecimalTypeDefaultWarning(EventDefinitionBase definition, EventData payload) {
      var d = (EventDefinition<string, string>)definition;
      var p = (PropertyEventData)payload;
      return d.GenerateMessage(p.Property.Name, p.Property.DeclaringEntityType.DisplayName());
    }

    public static void DefaultSchemaFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string schemaName) => EFCoreStrings.LogFoundDefaultSchema.Log(diagnostics, schemaName);

    public static void ForeignKeyColumnFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [CanBeNull] string tableName,
      [CanBeNull] string foreignKeyName,
      [CanBeNull] string principalTableName,
      [CanBeNull] string columnName,
      [CanBeNull] string principalColumnName,
      [CanBeNull] string updateAction,
      [CanBeNull] string deleteAction, int? ordinal) {
      var definition = EFCoreStrings.LogFoundForeignKeyColumn;
      Debug.Assert(LogLevel.Debug == definition.Level);
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(
            diagnostics,
            l => l.LogDebug(definition.EventId, null, definition.MessageFormat, tableName, foreignKeyName, principalTableName, columnName, principalColumnName, updateAction, deleteAction, ordinal));
      }
    }

    public static void ForeignKeyColumnMissingWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string columnName, [CanBeNull] string foreignKeyName, [CanBeNull] string tableName) => EFCoreStrings.LogForeignKeyColumnNotInSelectionSet.Log(diagnostics, columnName, foreignKeyName, tableName);
    public static void ForeignKeyColumnNotNamedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string foreignKeyName, [CanBeNull] string tableName) => EFCoreStrings.LogColumnNameEmptyOnForeignKey.Log(diagnostics, tableName, foreignKeyName);
    public static void ForeignKeyColumnsNotMappedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string foreignKeyName, [NotNull] IList<string> unmappedColumnNames) => EFCoreStrings.LogForeignKeyScaffoldErrorPropertyNotFound.Log(diagnostics, foreignKeyName, string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedColumnNames));
    public static void ForeignKeyFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName, long id, [CanBeNull] string principalTableName, [CanBeNull] string deleteAction) => EFCoreStrings.LogFoundForeignKey.Log(diagnostics, tableName, id, principalTableName, deleteAction);
    public static void ForeignKeyNotNamedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName) => EFCoreStrings.LogForeignKeyNameEmpty.Log(diagnostics, tableName);
    public static void ForeignKeyPrincipalColumnMissingWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string foreignKeyName, [CanBeNull] string tableName, [CanBeNull] string principalColumnName, [CanBeNull] string principalTableName) => EFCoreStrings.LogPrincipalColumnNotFound.Log(diagnostics, foreignKeyName, tableName, principalColumnName, principalTableName);
    public static void ForeignKeyReferencesMissingPrincipalTableWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string foreignKeyName, [CanBeNull] string tableName, [CanBeNull] string principalTableName) => EFCoreStrings.LogPrincipalTableNotInSelectionSet.Log(diagnostics, foreignKeyName, tableName, principalTableName);
    public static void ForeignKeyReferencesMissingTableWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string foreignKeyName) => EFCoreStrings.LogForeignKeyScaffoldErrorPrincipalTableNotFound.Log(diagnostics, foreignKeyName);
    public static void IndexColumnFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName, [CanBeNull] string indexName, bool? unique, [CanBeNull] string columnName, int? ordinal) => EFCoreStrings.LogFoundIndexColumn.Log(diagnostics, indexName, tableName, columnName, ordinal);
    public static void IndexColumnNotNamedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string indexName, [CanBeNull] string tableName) => EFCoreStrings.LogColumnNameEmptyOnIndex.Log(diagnostics, indexName, tableName);
    public static void IndexColumnSkipped([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName, [CanBeNull] string indexName, [CanBeNull] string columnName) => EFCoreStrings.LogIndexColumnNotInSelectionSet.Log(diagnostics, columnName, indexName, tableName);
    public static void IndexColumnsNotMappedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string indexName, [NotNull] IList<string> unmappedColumnNames) => EFCoreStrings.LogUnableToScaffoldIndexMissingProperty.Log(diagnostics, indexName, string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedColumnNames));
    public static void IndexFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string indexName, [CanBeNull] string tableName, bool? unique) => EFCoreStrings.LogFoundIndex.Log(diagnostics, indexName, tableName, unique);
    public static void IndexNotNamedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName) => EFCoreStrings.LogIndexNameEmpty.Log(diagnostics, tableName);
    public static void IndexTableMissingWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string indexName, [CanBeNull] string tableName) => EFCoreStrings.LogUnableToFindTableForIndex.Log(diagnostics, indexName, tableName);
    public static void MissingSchemaWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string schemaName) => EFCoreStrings.LogMissingSchema.Log(diagnostics, schemaName);
    public static void MissingTableWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName) => EFCoreStrings.LogMissingTable.Log(diagnostics, tableName, null);
    public static void PrimaryKeyFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string primaryKeyName, [CanBeNull] string tableName) => EFCoreStrings.LogFoundPrimaryKey.Log(diagnostics, primaryKeyName, tableName);

    private static string SchemaConfiguredWarning(EventDefinitionBase definition, EventData payload) {
      var d = (EventDefinition<string, string>)definition;
      var p = (EntityTypeSchemaEventData)payload;
      return d.GenerateMessage(
          p.EntityType.DisplayName(),
          p.Schema);
    }

    public static void SchemaConfiguredWarning(
       [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics,
       [NotNull] IEntityType entityType,
       [NotNull] string schema) {
      var definition = EFCoreStrings.LogSchemaConfigured;
      // Checking for enabled here to avoid string formatting if not needed.
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(diagnostics, entityType.DisplayName(), schema);
      }
      if (diagnostics.DiagnosticSource.IsEnabled(definition.EventId.Name)) {
        diagnostics.DiagnosticSource.Write(
            definition.EventId.Name,
            new EntityTypeSchemaEventData(
                definition,
                SchemaConfiguredWarning,
                entityType,
                schema));
      }
    }

    public static void SchemasNotSupportedWarning(
           [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics)
           => EFCoreStrings.LogUsingSchemaSelectionsWarning.Log(diagnostics);

    public static void SequenceConfiguredWarning(
        [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics,
        [NotNull] ISequence sequence) {
      var definition = EFCoreStrings.LogSequenceConfigured;
      definition.Log(diagnostics, sequence.Name);
      if (diagnostics.DiagnosticSource.IsEnabled(definition.EventId.Name)) {
        diagnostics.DiagnosticSource.Write(
            definition.EventId.Name,
            new SequenceEventData(
                definition,
                SequenceConfiguredWarning,
                sequence));
      }
    }

    private static string SequenceConfiguredWarning(EventDefinitionBase definition, EventData payload) {
      var d = (EventDefinition<string>)definition;
      var p = (SequenceEventData)payload;
      return d.GenerateMessage(p.Sequence.Name);
    }

    public static void SequenceFound(
       [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
       [CanBeNull] string sequenceName,
       [CanBeNull] string sequenceTypeName,
       bool? cyclic,
       int? increment,
       long? start,
       long? min,
       long? max) {
      var definition = EFCoreStrings.LogFoundSequence;
      Debug.Assert(LogLevel.Debug == definition.Level);
      if (diagnostics.GetLogBehavior(definition.EventId, definition.Level) != WarningBehavior.Ignore) {
        definition.Log(
            diagnostics,
            l => l.LogDebug(definition.EventId, null, definition.MessageFormat, sequenceName, sequenceTypeName, cyclic, increment, start, min, max));
      }
    }

    public static void SequenceNotNamedWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics) => EFCoreStrings.LogSequencesRequireName.Log(diagnostics);
    public static void TableFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName) => EFCoreStrings.LogFoundTable.Log(diagnostics, tableName);
    public static void TableSkipped([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName) => EFCoreStrings.LogTableNotInSelectionSet.Log(diagnostics, tableName);
    public static void TypeAliasFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string typeAliasName, [CanBeNull] string systemTypeName) => EFCoreStrings.LogFoundTypeAlias.Log(diagnostics, typeAliasName, systemTypeName);
    public static void UniqueConstraintFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string uniqueConstraintName, [CanBeNull] string tableName) => EFCoreStrings.LogFoundUniqueConstraint.Log(diagnostics, uniqueConstraintName, tableName);

  }
}
