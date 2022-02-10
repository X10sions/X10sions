using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class RestrictionRow : BaseTypedDataRow {
    // public RestrictionRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.Restrictions) { }
    public RestrictionRow(DataRow row) : base(row) { }

    public string CollectionName => DataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    public string RestrictionName => DataRow.Field<string>(nameof(RestrictionName));
    public string RestrictionDefault => DataRow.Field<string>(nameof(RestrictionDefault));
    public int RestrictionNumber => DataRow.Field<int>(nameof(RestrictionNumber));

    //public MetaDataCollectionRow MetaDataCollectionRow=> row.Field<string>(DbMetaDataColumnNames.meta); 
  }
}