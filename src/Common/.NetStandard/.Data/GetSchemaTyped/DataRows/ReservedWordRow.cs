using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ReservedWordRow : _BaseTypedDataRow {
    // public ReservedWordRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.ReservedWords) { }
    public ReservedWordRow(DataRow row) : base(row) { }

    public string ReservedWord => row.Field<string>(DbMetaDataColumnNames.ReservedWord);
  }
}