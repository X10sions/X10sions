using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class MetaDataCollectionRow : BaseTypedDataRow {
    // public MetaDataCollectionRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.MetaDataCollections) { }
    public MetaDataCollectionRow(DataRow row) : base(row) { }

    public string CollectionName => DataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    public int NumberOfRestrictions => DataRow.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
    public int NumberOfIdentifierParts => DataRow.Field<int>(DbMetaDataColumnNames.NumberOfIdentifierParts);
  }
}