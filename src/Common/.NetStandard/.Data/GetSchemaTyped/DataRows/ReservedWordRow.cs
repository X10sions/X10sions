using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ReservedWordRow : BaseTypedDataRow {
    // public ReservedWordRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.ReservedWords) { }
    public ReservedWordRow(DataRow row) : base(row) { }

    public string ReservedWord => DataRow.Field<string>(DbMetaDataColumnNames.ReservedWord);
  }
}