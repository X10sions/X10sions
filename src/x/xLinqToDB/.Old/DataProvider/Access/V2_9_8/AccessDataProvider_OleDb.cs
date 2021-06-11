using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Extensions;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public class AccessDataProvider_OleDb : AccessDataProvider_Base {
    private readonly OleDbType _decimalType = OleDbType.Decimal;
    public AccessDataProvider_OleDb()
      : this(ProviderName.Access, new AccessMappingSchema()) {
    }

    protected AccessDataProvider_OleDb(string name, MappingSchema mappingSchema) : base(name, mappingSchema) {
      if(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ".")
        _decimalType = OleDbType.VarChar;
    }

    public override string ConnectionNamespace => typeof(OleDbConnection).Namespace;
    public override Type DataReaderType => typeof(OleDbDataReader);

    protected override IDbConnection CreateConnectionInternal(string connectionString) => new OleDbConnection(connectionString);

    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new AccessSqlBuilder_OleDb(GetSqlOptimizer(), SqlProviderFlags, mappingSchema.ValueToSqlConverter);

    public override bool IsCompatibleConnection(IDbConnection connection) => ReflectionExtensions.IsSameOrParentOf(typeof(OleDbConnection), Proxy.GetUnderlyingObject((DbConnection)connection).GetType());

    public override ISchemaProvider GetSchemaProvider() => new AccessSchemaProvider_OleDb();

    protected override void SetParameterType(IDbDataParameter parameter, DbDataType dataType) {
      // Do some magic to workaround 'Data type mismatch in criteria expression' error in JET for some european locales.
      switch(dataType.DataType) {
        // OleDbType.Decimal is locale aware, OleDbType.Currency is locale neutral.
        case DataType.Decimal:
        case DataType.VarNumeric:
          ((OleDbParameter)parameter).OleDbType = _decimalType;
          return;
        case DataType.DateTime:
        case DataType.DateTime2:
          ((OleDbParameter)parameter).OleDbType = OleDbType.Date;
          return;
        case DataType.Text:
          ((OleDbParameter)parameter).OleDbType = OleDbType.LongVarChar;
          return;
        case DataType.NText:
          ((OleDbParameter)parameter).OleDbType = OleDbType.LongVarWChar;
          return;
      }
      base_SetParameterType(parameter, dataType);
    }

  }
}