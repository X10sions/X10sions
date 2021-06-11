namespace System.Data {
  public static class DataColumnExtensions {

    public static TypeCode GetTypeCode(this DataColumn dataColumn) => Type.GetTypeCode(dataColumn.DataType);

  }
}