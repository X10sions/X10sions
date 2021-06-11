using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Extensions;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public class AccessDataProvider_Odbc : AccessDataProvider_Base {
    private readonly OdbcType _decimalType = OdbcType.Decimal;
    public AccessDataProvider_Odbc()
      : this(ProviderName.Access, new AccessMappingSchema()) {
    }

    protected AccessDataProvider_Odbc(string name, MappingSchema mappingSchema) : base(name, mappingSchema) {
      if(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ".")
        _decimalType = OdbcType.VarChar;
    }

    public override string ConnectionNamespace => typeof(OdbcConnection).Namespace;
    public override Type DataReaderType => typeof(OdbcDataReader);

    protected override IDbConnection CreateConnectionInternal(string connectionString) => new OdbcConnection(connectionString);

    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new AccessSqlBuilder_Odbc(GetSqlOptimizer(), SqlProviderFlags, mappingSchema.ValueToSqlConverter);

    public override bool IsCompatibleConnection(IDbConnection connection) => ReflectionExtensions.IsSameOrParentOf(typeof(OdbcConnection), Proxy.GetUnderlyingObject((DbConnection)connection).GetType());

    public override ISchemaProvider GetSchemaProvider() => new AccessSchemaProvider_Odbc();

    protected override void SetParameterType(IDbDataParameter parameter, DbDataType dataType) {
      // Do some magic to workaround 'Data type mismatch in criteria expression' error
      // in JET for some european locales.
      //
      switch(dataType.DataType) {
        // OdbcType.Decimal is locale aware, OdbcType.Currency is locale neutral.
        case DataType.Decimal:
        case DataType.VarNumeric:
          ((OdbcParameter)parameter).OdbcType = _decimalType;
          return;
        // OdbcType.DBTimeStamp is locale aware, OdbcType.Date is locale neutral.
        //2020-04-09  case DataType.DateTime:
        //2020-04-09  case DataType.DateTime2: ((OdbcParameter)parameter).OdbcType = OdbcType.DateTime; return;
        case DataType.Text:
          ((OdbcParameter)parameter).OdbcType = OdbcType.VarChar;
          return;
        case DataType.NText:
          ((OdbcParameter)parameter).OdbcType = OdbcType.NVarChar;
          return;
      }
      base_SetParameterType(parameter, dataType);
    }

  }
}