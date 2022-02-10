using System.Data;

namespace Common.Data {

  //public interface IDataRow {  }

  public interface ITypedDataRow  {
    public DataRow DataRow { get; }
  }

  public abstract class BaseTypedDataRow : ITypedDataRow {
    public BaseTypedDataRow(DataRow dataRow) {
      DataRow = dataRow;
    }
    public DataRow DataRow { get; }
  }
}