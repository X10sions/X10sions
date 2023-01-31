using FreeSql.Internal.CommonProvider;

namespace FreeSql.v3_2_684.Internal.CommonProvider;

public abstract partial class xAdoProvider : AdoProvider, xIAdo {
  public xAdoProvider(xDataType dataType, string? connectionString, string[]? slaveConnectionStrings) : base(FreeSql.DataType.Custom, connectionString, slaveConnectionStrings) {
    DataType = dataType;
  }
  public new xDataType DataType { get; }

}