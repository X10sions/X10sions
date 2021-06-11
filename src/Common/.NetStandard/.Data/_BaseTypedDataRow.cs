using System.Data;

namespace Common.Data {
  public abstract class _BaseTypedDataRow {
    public _BaseTypedDataRow(DataRow row) {
      this.row = row;
    }
    protected DataRow row;
  }
}