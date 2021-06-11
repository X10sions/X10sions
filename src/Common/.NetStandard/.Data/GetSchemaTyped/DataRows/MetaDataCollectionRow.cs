using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class MetaDataCollectionRow : _BaseTypedDataRow {
    // public MetaDataCollectionRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.MetaDataCollections) { }
    public MetaDataCollectionRow(DataRow row) : base(row) { }

    public string CollectionName => row.Field<string>(DbMetaDataColumnNames.CollectionName);
    public int NumberOfRestrictions => row.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
    public int NumberOfIdentifierParts => row.Field<int>(DbMetaDataColumnNames.NumberOfIdentifierParts);
  }
}