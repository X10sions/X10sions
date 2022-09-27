using System.Data;

namespace Common.Data;

public interface ITypedDataRow {
  public void SetValues(DataRow dataRow);
}
