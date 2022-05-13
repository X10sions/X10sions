using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class ReservedWordRow : BaseTypedDataRow {
  public ReservedWordRow() { }
    public ReservedWordRow(DataRow dataRow):base(dataRow) { }

  public override void SetValues(DataRow dataRow) {
    ReservedWord = dataRow.Field<string>(DbMetaDataColumnNames.ReservedWord);
  }

   public string ReservedWord { get; set; } = string.Empty;
}
