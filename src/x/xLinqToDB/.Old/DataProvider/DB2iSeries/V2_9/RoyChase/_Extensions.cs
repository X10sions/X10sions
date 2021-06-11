using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using System.Diagnostics;
using System;
using System.Data;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.RoyChase {
  public static class _Extensions_RoyChase {

    public static Action<IDbDataParameter> _setBlob;

    public static void OnConnectionTypeCreated_DB2iSeries(this DataProviderBase dataProviderBase, Type connectionType      , Action<Type, Type, string> setProviderField      , Func<Type, Action<IDbDataParameter>> getSetParameter) {
      DB2iSeriesTypes.InitDB2i(connectionType);
      setProviderField(DB2iSeriesTypes.BigInt, typeof(long), "Get" + nameof(iDB2BigInt));
      setProviderField(DB2iSeriesTypes.Binary, typeof(byte[]), "Get" + nameof(iDB2Binary));
      setProviderField(DB2iSeriesTypes.Blob, typeof(byte[]), "Get" + nameof(iDB2Blob));
      setProviderField(DB2iSeriesTypes.Char, typeof(string), "Get" + nameof(iDB2Char));
      setProviderField(DB2iSeriesTypes.CharBitData, typeof(byte[]), "Get" + nameof(iDB2CharBitData));
      setProviderField(DB2iSeriesTypes.Clob, typeof(string), "Get" + nameof(iDB2Clob));
      setProviderField(DB2iSeriesTypes.DataLink, typeof(string), "Get" + nameof(iDB2DataLink));
      setProviderField(DB2iSeriesTypes.Date, typeof(DateTime), "Get" + nameof(iDB2Date));
      setProviderField(DB2iSeriesTypes.DbClob, typeof(string), "Get" + nameof(iDB2DbClob));
      setProviderField(DB2iSeriesTypes.DecFloat16, typeof(decimal), "Get" + nameof(iDB2DecFloat16));
      setProviderField(DB2iSeriesTypes.DecFloat34, typeof(decimal), "Get" + nameof(iDB2DecFloat34));
      setProviderField(DB2iSeriesTypes.Decimal, typeof(decimal), "Get" + nameof(iDB2Decimal));
      setProviderField(DB2iSeriesTypes.Double, typeof(double), "Get" + nameof(iDB2Double));
      setProviderField(DB2iSeriesTypes.Graphic, typeof(string), "Get" + nameof(iDB2Graphic));
      setProviderField(DB2iSeriesTypes.Integer, typeof(int), "Get" + nameof(iDB2Integer));
      setProviderField(DB2iSeriesTypes.Numeric, typeof(decimal), "Get" + nameof(iDB2Numeric));
      setProviderField(DB2iSeriesTypes.Real, typeof(float), "Get" + nameof(iDB2Real));
      setProviderField(DB2iSeriesTypes.RowId, typeof(byte[]), "Get" + nameof(iDB2Rowid));
      setProviderField(DB2iSeriesTypes.SmallInt, typeof(short), "Get" + nameof(iDB2SmallInt));
      setProviderField(DB2iSeriesTypes.Time, typeof(DateTime), "Get" + nameof(iDB2Time));
      setProviderField(DB2iSeriesTypes.TimeStamp, typeof(DateTime), "Get" + nameof(iDB2TimeStamp));
      setProviderField(DB2iSeriesTypes.VarBinary, typeof(byte[]), "Get" + nameof(iDB2VarBinary));
      setProviderField(DB2iSeriesTypes.VarChar, typeof(string), "Get" + nameof(iDB2VarChar));
      setProviderField(DB2iSeriesTypes.VarCharBitData, typeof(byte[]), "Get" + nameof(iDB2VarCharBitData));
      setProviderField(DB2iSeriesTypes.VarGraphic, typeof(string), "Get" + nameof(iDB2VarGraphic));
      setProviderField(DB2iSeriesTypes.Xml, typeof(string), "Get" + nameof(iDB2Xml));
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.BigInt, DB2iSeriesTypes.BigInt.GetNullValue(), true, DataType.Int64);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Binary, DB2iSeriesTypes.Binary.GetNullValue(), true, DataType.Binary);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Blob, DB2iSeriesTypes.Blob.GetNullValue(), true, DataType.Blob);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Char, DB2iSeriesTypes.Char.GetNullValue(), true, DataType.Char);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.CharBitData, DB2iSeriesTypes.CharBitData.GetNullValue(), true, DataType.Binary);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Clob, DB2iSeriesTypes.Clob.GetNullValue(), true, DataType.NText);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.DataLink, DB2iSeriesTypes.DataLink.GetNullValue(), true, DataType.NText);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Date, DB2iSeriesTypes.Date.GetNullValue(), true, DataType.Date);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.DbClob, DB2iSeriesTypes.DbClob.GetNullValue(), true, DataType.NText);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.DecFloat16, DB2iSeriesTypes.DecFloat16.GetNullValue(), true, DataType.Decimal);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.DecFloat34, DB2iSeriesTypes.DecFloat34.GetNullValue(), true, DataType.Decimal);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Decimal, DB2iSeriesTypes.Decimal.GetNullValue(), true, DataType.Decimal);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Double, DB2iSeriesTypes.Double.GetNullValue(), true, DataType.Double);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Graphic, DB2iSeriesTypes.Graphic.GetNullValue(), true, DataType.NText);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Integer, DB2iSeriesTypes.Integer.GetNullValue(), true, DataType.Int32);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Numeric, DB2iSeriesTypes.Numeric.GetNullValue(), true, DataType.Decimal);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Real, DB2iSeriesTypes.Real.GetNullValue(), true, DataType.Single);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.RowId, DB2iSeriesTypes.RowId.GetNullValue(), true, DataType.VarBinary);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.SmallInt, DB2iSeriesTypes.SmallInt.GetNullValue(), true, DataType.Int16);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Time, DB2iSeriesTypes.Time.GetNullValue(), true, DataType.Time);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.TimeStamp, DB2iSeriesTypes.TimeStamp.GetNullValue(), true, DataType.DateTime2);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.VarBinary, DB2iSeriesTypes.VarBinary.GetNullValue(), true, DataType.VarBinary);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.VarChar, DB2iSeriesTypes.VarChar.GetNullValue(), true, DataType.VarChar);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.VarCharBitData, DB2iSeriesTypes.VarCharBitData.GetNullValue(), true, DataType.VarBinary);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.VarGraphic, DB2iSeriesTypes.VarGraphic.GetNullValue(), true, DataType.NText);
      dataProviderBase.MappingSchema.AddScalarType(DB2iSeriesTypes.Xml, DB2iSeriesTypes.Xml.GetNullValue(), true, DataType.Xml);
      _setBlob = getSetParameter(connectionType);
      if (DataConnection.TraceSwitch.TraceInfo) {
        DataConnection.WriteTraceLine(dataProviderBase.DataReaderType.Assembly.FullName, DataConnection.TraceSwitch.DisplayName, TraceLevel.Info);
      }
      DB2iSeriesTools.Initialized();

    }

  }
}
