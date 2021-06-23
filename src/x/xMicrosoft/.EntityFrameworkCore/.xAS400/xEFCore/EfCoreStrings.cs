﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Resources;

namespace xEFCore {
  public static class EFCoreStrings {

    static Assembly ass = typeof(EFCoreStrings).Assembly;

    static readonly ResourceManager _resourceManager = new ResourceManager("IBM.EntityFrameworkCore.Properties.Db2Strings", ass);
    static readonly ResourceManager _resourceManager_DB2 = new ResourceManager("IBM.EntityFrameworkCore.Properties.Db2Strings", ass);
    static readonly ResourceManager _resourceManager_Sqlite = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.SqliteStrings", ass);
    static readonly ResourceManager _resourceManager_SqlServer = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.SqlServerStrings", ass);

    public static readonly EventDefinition LogSequencesRequireName = new EventDefinition(EFCoreEventId.SequenceNotNamedWarning, LogLevel.Warning, LoggerMessage.Define(LogLevel.Warning, EFCoreEventId.SequenceNotNamedWarning, _resourceManager.GetString(nameof(LogSequencesRequireName))));
    public static readonly EventDefinition LogUsingSchemaSelectionsWarning = new EventDefinition(EFCoreEventId.SchemasNotSupportedWarning, LogLevel.Warning, LoggerMessage.Define(LogLevel.Warning, EFCoreEventId.SchemasNotSupportedWarning, _resourceManager.GetString(nameof(LogUsingSchemaSelectionsWarning))));
    public static readonly EventDefinition<string, long, string, string> LogFoundForeignKey = new EventDefinition<string, long, string, string>(EFCoreEventId.ForeignKeyFound, LogLevel.Debug, LoggerMessage.Define<string, long, string, string>(LogLevel.Debug, EFCoreEventId.ForeignKeyFound, _resourceManager.GetString(nameof(LogFoundForeignKey))));
    public static readonly EventDefinition<string, string, bool?> LogFoundIndex = new EventDefinition<string, string, bool?>(EFCoreEventId.IndexFound, LogLevel.Debug, LoggerMessage.Define<string, string, bool?>(LogLevel.Debug, EFCoreEventId.IndexFound, _resourceManager.GetString(nameof(LogFoundIndex))));
    public static readonly EventDefinition<string, string, string, bool, string> LogFoundColumnDefinition = new EventDefinition<string, string, string, bool, string>(EFCoreEventId.ColumnFound, LogLevel.Debug, LoggerMessage.Define<string, string, string, bool, string>(LogLevel.Debug, EFCoreEventId.ColumnFound, _resourceManager.GetString(nameof(LogFoundColumnDefinition))));
    public static readonly EventDefinition<string, string, string, int?> LogFoundIndexColumn = new EventDefinition<string, string, string, int?>(EFCoreEventId.IndexColumnFound, LogLevel.Debug, LoggerMessage.Define<string, string, string, int?>(LogLevel.Debug, EFCoreEventId.IndexColumnFound, _resourceManager.GetString(nameof(LogFoundIndexColumn))));
    public static readonly EventDefinition<string, string, string, string> LogPrincipalColumnNotFound = new EventDefinition<string, string, string, string>(EFCoreEventId.ForeignKeyPrincipalColumnMissingWarning, LogLevel.Warning, LoggerMessage.Define<string, string, string, string>(LogLevel.Warning, EFCoreEventId.ForeignKeyPrincipalColumnMissingWarning, _resourceManager.GetString(nameof(LogPrincipalColumnNotFound))));
    public static readonly EventDefinition<string, string, string> LogForeignKeyColumnNotInSelectionSet = new EventDefinition<string, string, string>(EFCoreEventId.ForeignKeyColumnMissingWarning, LogLevel.Warning, LoggerMessage.Define<string, string, string>(LogLevel.Warning, EFCoreEventId.ForeignKeyColumnMissingWarning, _resourceManager.GetString(nameof(LogForeignKeyColumnNotInSelectionSet))));
    public static readonly EventDefinition<string, string, string> LogIndexColumnNotInSelectionSet = new EventDefinition<string, string, string>(EFCoreEventId.IndexColumnSkipped, LogLevel.Warning, LoggerMessage.Define<string, string, string>(LogLevel.Warning, EFCoreEventId.IndexColumnSkipped, _resourceManager.GetString(nameof(LogIndexColumnNotInSelectionSet))));
    public static readonly EventDefinition<string, string, string> LogPrincipalTableNotInSelectionSet = new EventDefinition<string, string, string>(EFCoreEventId.ForeignKeyReferencesMissingPrincipalTableWarning, LogLevel.Warning, LoggerMessage.Define<string, string, string>(LogLevel.Warning, EFCoreEventId.ForeignKeyReferencesMissingPrincipalTableWarning, _resourceManager.GetString(nameof(LogPrincipalTableNotInSelectionSet))));
    public static readonly EventDefinition<string, string> LogByteIdentityColumn = new EventDefinition<string, string>(EFCoreEventId.ByteIdentityColumnWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.ByteIdentityColumnWarning, _resourceManager.GetString(nameof(LogByteIdentityColumn))));
    public static readonly EventDefinition<string, string> LogColumnNameEmptyOnForeignKey = new EventDefinition<string, string>(EFCoreEventId.ForeignKeyColumnNotNamedWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.ForeignKeyColumnNotNamedWarning, _resourceManager.GetString(nameof(LogColumnNameEmptyOnForeignKey))));
    public static readonly EventDefinition<string, string> LogColumnNameEmptyOnIndex = new EventDefinition<string, string>(EFCoreEventId.IndexColumnNotNamedWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.IndexColumnNotNamedWarning, _resourceManager.GetString(nameof(LogColumnNameEmptyOnIndex))));
    public static readonly EventDefinition<string, string> LogColumnNotInSelectionSet = new EventDefinition<string, string>(EFCoreEventId.ColumnSkipped, LogLevel.Debug, LoggerMessage.Define<string, string>(LogLevel.Debug, EFCoreEventId.ColumnSkipped, _resourceManager.GetString(nameof(LogColumnNotInSelectionSet))));
    public static readonly EventDefinition<string, string> LogDefaultDecimalTypeColumn = new EventDefinition<string, string>(EFCoreEventId.DecimalTypeDefaultWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.DecimalTypeDefaultWarning, _resourceManager.GetString(nameof(LogDefaultDecimalTypeColumn))));
    public static readonly EventDefinition<string, string> LogForeignKeyScaffoldErrorPropertyNotFound = new EventDefinition<string, string>(EFCoreEventId.ForeignKeyColumnsNotMappedWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.ForeignKeyColumnsNotMappedWarning, _resourceManager.GetString(nameof(LogForeignKeyScaffoldErrorPropertyNotFound))));
    public static readonly EventDefinition<string, string> LogFoundPrimaryKey = new EventDefinition<string, string>(EFCoreEventId.PrimaryKeyFound, LogLevel.Debug, LoggerMessage.Define<string, string>(LogLevel.Debug, EFCoreEventId.PrimaryKeyFound, _resourceManager.GetString(nameof(LogFoundPrimaryKey))));
    public static readonly EventDefinition<string, string> LogFoundTypeAlias = new EventDefinition<string, string>(EFCoreEventId.TypeAliasFound, LogLevel.Debug, LoggerMessage.Define<string, string>(LogLevel.Debug, EFCoreEventId.TypeAliasFound, _resourceManager.GetString(nameof(LogFoundTypeAlias))));
    public static readonly EventDefinition<string, string> LogFoundUniqueConstraint = new EventDefinition<string, string>(EFCoreEventId.UniqueConstraintFound, LogLevel.Debug, LoggerMessage.Define<string, string>(LogLevel.Debug, EFCoreEventId.UniqueConstraintFound, _resourceManager.GetString(nameof(LogFoundUniqueConstraint))));
    public static readonly EventDefinition<string, string> LogSchemaConfigured = new EventDefinition<string, string>(EFCoreEventId.SchemaConfiguredWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.SchemaConfiguredWarning, _resourceManager.GetString(nameof(LogSchemaConfigured))));
    public static readonly EventDefinition<string, string> LogUnableToFindTableForIndex = new EventDefinition<string, string>(EFCoreEventId.IndexTableMissingWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.IndexTableMissingWarning, _resourceManager.GetString(nameof(LogUnableToFindTableForIndex))));
    public static readonly EventDefinition<string, string> LogUnableToScaffoldIndexMissingProperty = new EventDefinition<string, string>(EFCoreEventId.IndexColumnsNotMappedWarning, LogLevel.Warning, LoggerMessage.Define<string, string>(LogLevel.Warning, EFCoreEventId.IndexColumnsNotMappedWarning, _resourceManager.GetString(nameof(LogUnableToScaffoldIndexMissingProperty))));
    public static readonly EventDefinition<string> LogColumnNameEmptyOnTable = new EventDefinition<string>(EFCoreEventId.ColumnNotNamedWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.ColumnNotNamedWarning, _resourceManager.GetString(nameof(LogColumnNameEmptyOnTable))));
    public static readonly EventDefinition<string> LogForeignKeyNameEmpty = new EventDefinition<string>(EFCoreEventId.ForeignKeyNotNamedWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.ForeignKeyNotNamedWarning, _resourceManager.GetString(nameof(LogForeignKeyNameEmpty))));
    public static readonly EventDefinition<string> LogForeignKeyScaffoldErrorPrincipalTableNotFound = new EventDefinition<string>(EFCoreEventId.ForeignKeyReferencesMissingTableWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.ForeignKeyReferencesMissingTableWarning, _resourceManager.GetString(nameof(LogForeignKeyScaffoldErrorPrincipalTableNotFound))));
    public static readonly EventDefinition<string> LogFoundDefaultSchema = new EventDefinition<string>(EFCoreEventId.DefaultSchemaFound, LogLevel.Debug, LoggerMessage.Define<string>(LogLevel.Debug, EFCoreEventId.DefaultSchemaFound, _resourceManager.GetString(nameof(LogFoundDefaultSchema))));
    public static readonly EventDefinition<string> LogFoundTable = new EventDefinition<string>(EFCoreEventId.TableFound, LogLevel.Debug, LoggerMessage.Define<string>(LogLevel.Debug, EFCoreEventId.TableFound, _resourceManager.GetString(nameof(LogFoundTable))));
    public static readonly EventDefinition<string> LogIndexNameEmpty = new EventDefinition<string>(EFCoreEventId.IndexNotNamedWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.IndexNotNamedWarning, _resourceManager.GetString(nameof(LogIndexNameEmpty))));
    public static readonly EventDefinition<string> LogMissingSchema = new EventDefinition<string>(EFCoreEventId.MissingSchemaWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.MissingSchemaWarning, _resourceManager.GetString(nameof(LogMissingSchema))));
    public static readonly EventDefinition<string> LogMissingTable = new EventDefinition<string>(EFCoreEventId.MissingTableWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.MissingTableWarning, _resourceManager.GetString(nameof(LogMissingTable))));
    public static readonly EventDefinition<string> LogSequenceConfigured = new EventDefinition<string>(EFCoreEventId.SequenceConfiguredWarning, LogLevel.Warning, LoggerMessage.Define<string>(LogLevel.Warning, EFCoreEventId.SequenceConfiguredWarning, _resourceManager.GetString(nameof(LogSequenceConfigured))));
    public static readonly EventDefinition<string> LogTableNotInSelectionSet = new EventDefinition<string>(EFCoreEventId.TableSkipped, LogLevel.Debug, LoggerMessage.Define<string>(LogLevel.Debug, EFCoreEventId.TableSkipped, _resourceManager.GetString(nameof(LogTableNotInSelectionSet))));
    public static readonly FallbackEventDefinition LogFoundColumnName = new FallbackEventDefinition(EFCoreEventId.ColumnFound, LogLevel.Debug, _resourceManager.GetString(nameof(LogFoundColumnName)));
    public static readonly FallbackEventDefinition LogFoundForeignKeyColumn = new FallbackEventDefinition(EFCoreEventId.ForeignKeyColumnFound, LogLevel.Debug, _resourceManager.GetString(nameof(LogFoundForeignKeyColumn)));
    public static readonly FallbackEventDefinition LogFoundSequence = new FallbackEventDefinition(EFCoreEventId.SequenceFound, LogLevel.Debug, _resourceManager.GetString(nameof(LogFoundSequence)));
    public static string AlterIdentityColumn => GetString(nameof(AlterIdentityColumn));
    public static string AlterMemoryOptimizedTable => GetString(nameof(AlterMemoryOptimizedTable));
    public static string IdentityBadType([CanBeNull] object property, [CanBeNull] object entityType, [CanBeNull] object propertyType) => string.Format(GetString(nameof(IdentityBadType), nameof(property), nameof(entityType), nameof(propertyType)), property, entityType, propertyType);
    public static string IncompatibleTableMemoryOptimizedMismatch([CanBeNull] object table, [CanBeNull] object entityType, [CanBeNull] object otherEntityType, [CanBeNull] object memoryOptimizedEntityType, [CanBeNull] object nonMemoryOptimizedEntityType) => string.Format(GetString(nameof(IncompatibleTableMemoryOptimizedMismatch), nameof(table), nameof(entityType), nameof(otherEntityType), nameof(memoryOptimizedEntityType), nameof(nonMemoryOptimizedEntityType)), table, entityType, otherEntityType, memoryOptimizedEntityType, nonMemoryOptimizedEntityType);
    public static string IndexTableRequired => GetString(nameof(IndexTableRequired));
    public static string InvalidMigrationOperation([CanBeNull] object operation) => string.Format(GetString(nameof(InvalidMigrationOperation), nameof(operation)), operation);
    public static string MigrationScriptGenerationNotSupported => GetString(nameof(MigrationScriptGenerationNotSupported));
    public static string MultipleIdentityColumns([CanBeNull] object properties, [CanBeNull] object table) => string.Format(GetString(nameof(MultipleIdentityColumns), nameof(properties), nameof(table)), properties, table);
    public static string NoInitialCatalog => GetString(nameof(NoInitialCatalog));
    public static string NonKeyValueGeneration([CanBeNull] object property, [CanBeNull] object entityType) => string.Format(GetString(nameof(NonKeyValueGeneration), nameof(property), nameof(entityType)), property, entityType);
    public static string SequenceBadType([CanBeNull] object property, [CanBeNull] object entityType, [CanBeNull] object propertyType) => string.Format(GetString(nameof(SequenceBadType), nameof(property), nameof(entityType), nameof(propertyType)), property, entityType, propertyType);
    public static string SequencesNotSupported => GetString(nameof(SequencesNotSupported));
    public static string TransientExceptionDetected => GetString(nameof(TransientExceptionDetected));
    public static string UnqualifiedDataType([CanBeNull] object dataType) => string.Format(GetString(nameof(UnqualifiedDataType), nameof(dataType)), dataType);

    static string GetString(string name, params string[] formatterNames) {
      var value = _resourceManager.GetString(name);
      for (var i = 0; i < formatterNames.Length; i++) {
        value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
      }
      return value;
    }

  }
}