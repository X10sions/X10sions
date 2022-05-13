using System.Data;

namespace Common.Data;

public interface ITypedDataRow {
  public void SetValues(DataRow dataRow);
}

public abstract class BaseTypedDataRow : ITypedDataRow {
  public BaseTypedDataRow() { }

  public BaseTypedDataRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public abstract void SetValues(DataRow dataRow);

}
