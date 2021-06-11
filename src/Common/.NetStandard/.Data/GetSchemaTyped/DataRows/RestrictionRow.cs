using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class RestrictionRow : _BaseTypedDataRow {
    // public RestrictionRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.Restrictions) { }
    public RestrictionRow(DataRow row) : base(row) { }

    public string CollectionName => row.Field<string>(DbMetaDataColumnNames.CollectionName);
    public string RestrictionName => row.Field<string>(nameof(RestrictionName));
    public string RestrictionDefault => row.Field<string>(nameof(RestrictionDefault));
    public int RestrictionNumber => row.Field<int>(nameof(RestrictionNumber));

    //public MetaDataCollectionRow MetaDataCollectionRow=> row.Field<string>(DbMetaDataColumnNames.meta); 
  }
}