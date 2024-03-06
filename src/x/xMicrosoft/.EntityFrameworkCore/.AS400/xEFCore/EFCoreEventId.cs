using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;

namespace xEFCore {

  public static class EFCoreEventId {

    enum Id {
      // Model validation events
      ByteIdentityColumnWarning = CoreEventId.ProviderBaseId,
      DecimalTypeDefaultWarning,
      SchemaConfiguredWarning,
      SequenceConfiguredWarning,
      // Scaffolding events
      ColumnFound = CoreEventId.ProviderDesignBaseId,
      ColumnNotNamedWarning,
      ColumnSkipped,
      DefaultSchemaFound,
      ForeignKeyColumnFound,
      ForeignKeyColumnMissingWarning,
      ForeignKeyColumnNotNamedWarning,
      ForeignKeyColumnsNotMappedWarning,
      ForeignKeyFound,
      ForeignKeyNotNamedWarning,
      ForeignKeyPrincipalColumnMissingWarning,
      ForeignKeyReferencesMissingPrincipalTableWarning,
      ForeignKeyReferencesMissingTableWarning,
      IndexColumnFound,
      IndexColumnNotNamedWarning,
      IndexColumnSkipped,
      IndexColumnsNotMappedWarning,
      IndexFound,
      IndexNotNamedWarning,
      IndexTableMissingWarning,
      MissingSchemaWarning,
      MissingTableWarning,
      PrimaryKeyFound,
      SchemasNotSupportedWarning,
      SequenceFound,
      SequenceNotNamedWarning,
      TableFound,
      TableSkipped,
      TypeAliasFound,
      UniqueConstraintFound,
    }

    public static readonly EventId ByteIdentityColumnWarning = MakeValidationId(Id.ByteIdentityColumnWarning);
    public static readonly EventId ColumnFound = MakeScaffoldingId(Id.ColumnFound);
    public static readonly EventId ColumnNotNamedWarning = MakeScaffoldingId(Id.ColumnNotNamedWarning);
    public static readonly EventId ColumnSkipped = MakeScaffoldingId(Id.ColumnSkipped);
    public static readonly EventId DecimalTypeDefaultWarning = MakeValidationId(Id.DecimalTypeDefaultWarning);
    public static readonly EventId DefaultSchemaFound = MakeScaffoldingId(Id.DefaultSchemaFound);
    public static readonly EventId ForeignKeyColumnFound = MakeScaffoldingId(Id.ForeignKeyColumnFound);
    public static readonly EventId ForeignKeyColumnMissingWarning = MakeScaffoldingId(Id.ForeignKeyColumnMissingWarning);
    public static readonly EventId ForeignKeyColumnNotNamedWarning = MakeScaffoldingId(Id.ForeignKeyColumnNotNamedWarning);
    public static readonly EventId ForeignKeyColumnsNotMappedWarning = MakeScaffoldingId(Id.ForeignKeyColumnsNotMappedWarning);
    public static readonly EventId ForeignKeyFound = MakeScaffoldingId(Id.ForeignKeyFound);
    public static readonly EventId ForeignKeyNotNamedWarning = MakeScaffoldingId(Id.ForeignKeyNotNamedWarning);
    public static readonly EventId ForeignKeyPrincipalColumnMissingWarning = MakeScaffoldingId(Id.ForeignKeyPrincipalColumnMissingWarning);
    public static readonly EventId ForeignKeyReferencesMissingPrincipalTableWarning = MakeScaffoldingId(Id.ForeignKeyReferencesMissingPrincipalTableWarning);
    public static readonly EventId ForeignKeyReferencesMissingTableWarning = MakeScaffoldingId(Id.ForeignKeyReferencesMissingTableWarning);
    public static readonly EventId IndexColumnFound = MakeScaffoldingId(Id.IndexColumnFound);
    public static readonly EventId IndexColumnNotNamedWarning = MakeScaffoldingId(Id.IndexColumnNotNamedWarning);
    public static readonly EventId IndexColumnSkipped = MakeScaffoldingId(Id.IndexColumnSkipped);
    public static readonly EventId IndexColumnsNotMappedWarning = MakeScaffoldingId(Id.IndexColumnsNotMappedWarning);
    public static readonly EventId IndexFound = MakeScaffoldingId(Id.IndexFound);
    public static readonly EventId IndexNotNamedWarning = MakeScaffoldingId(Id.IndexNotNamedWarning);
    public static readonly EventId IndexTableMissingWarning = MakeScaffoldingId(Id.IndexTableMissingWarning);
    public static readonly EventId MissingSchemaWarning = MakeScaffoldingId(Id.MissingSchemaWarning);
    public static readonly EventId MissingTableWarning = MakeScaffoldingId(Id.MissingTableWarning);
    public static readonly EventId PrimaryKeyFound = MakeScaffoldingId(Id.PrimaryKeyFound);
    public static readonly EventId SchemaConfiguredWarning = MakeValidationId(Id.SchemaConfiguredWarning);
    public static readonly EventId SchemasNotSupportedWarning = MakeScaffoldingId(Id.SchemasNotSupportedWarning);
    public static readonly EventId SequenceConfiguredWarning = MakeValidationId(Id.SequenceConfiguredWarning);
    public static readonly EventId SequenceFound = MakeScaffoldingId(Id.SequenceFound);
    public static readonly EventId SequenceNotNamedWarning = MakeScaffoldingId(Id.SequenceNotNamedWarning);
    public static readonly EventId TableFound = MakeScaffoldingId(Id.TableFound);
    public static readonly EventId TableSkipped = MakeScaffoldingId(Id.TableSkipped);
    public static readonly EventId TypeAliasFound = MakeScaffoldingId(Id.TypeAliasFound);
    public static readonly EventId UniqueConstraintFound = MakeScaffoldingId(Id.UniqueConstraintFound);

    static EventId MakeScaffoldingId(Id id) => EventIdFactory.Create((int)id, _scaffoldingPrefix + id);
    static EventId MakeValidationId(Id id) => EventIdFactory.Create((int)id, _validationPrefix + id);
    static readonly string _scaffoldingPrefix = DbLoggerCategory.Scaffolding.Name + ".";
    static readonly string _validationPrefix = DbLoggerCategory.Model.Validation.Name + ".";

    static class EventIdFactory {
      public static EventId Create(int id, string name) {
        if (AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue9437", out var isEnabled)
            && isEnabled) {
          if (id >= CoreEventId.ProviderDesignBaseId) {
            id = MassageId(id, CoreEventId.ProviderDesignBaseId);
          } else if (id >= CoreEventId.ProviderBaseId) {
            id = MassageId(id, CoreEventId.ProviderBaseId);
          } else if (id >= CoreEventId.RelationalBaseId) {
            id = MassageId(id, CoreEventId.RelationalBaseId);
          } else if (id >= CoreEventId.CoreBaseId) {
            id = MassageId(id, CoreEventId.CoreBaseId);
          }
        }
        return new EventId(id, name);
      }
      static int MassageId(int id, int baseId) => (id - baseId) + (baseId * 10);
    }

  }
}