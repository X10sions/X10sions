using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class MetaDataCollectionRow: BaseTypedDataRow {
    public MetaDataCollectionRow() { }

    public MetaDataCollectionRow(DataRow dataRow) :base(dataRow) { }
    public override void SetValues(DataRow dataRow) {
    CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
      NumberOfRestrictions = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
      NumberOfIdentifierParts = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfIdentifierParts);
    }
    //public MetaDataCollectionRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.MetaDataCollections) { }

    public string CollectionName { get; set; } = string.Empty;
    public int NumberOfRestrictions { get; set; }
    public int NumberOfIdentifierParts { get; set; }
  }
}