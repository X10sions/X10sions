using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SqlClient {
  public sealed partial class SqlParameter {
    internal sealed class SqlParameterConverter : ExpandableObjectConverter {

      // converter classes should have public ctor
      public SqlParameterConverter() {
      }

      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        if (typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) == destinationType) {
          return true;
        }
        return base.CanConvertTo(context, destinationType);
      }

      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        if (destinationType == null) {
          throw ADP.ArgumentNull("destinationType");
        }
        if ((typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) == destinationType) && (value is SqlParameter)) {
          return ConvertToInstanceDescriptor(value as SqlParameter);
        }
        return base.ConvertTo(context, culture, value, destinationType);
      }

      private System.ComponentModel.Design.Serialization.InstanceDescriptor ConvertToInstanceDescriptor(SqlParameter p) {
        // MDAC 67321 - reducing parameter generated code
        var flags = 0; // if part of the collection - the parametername can't be empty

        if (p.ShouldSerializeSqlDbType()) {
          flags |= 1;
        }
        if (p.ShouldSerializeSize()) {
          flags |= 2;
        }
        if (!ADP.IsEmpty(p.SourceColumn)) {
          flags |= 4;
        }
        if (null != p.Value) {
          flags |= 8;
        }
        if ((ParameterDirection.Input != p.Direction) || p.IsNullable
            || p.ShouldSerializePrecision() || p.ShouldSerializeScale()
            || (DataRowVersion.Current != p.SourceVersion)
            ) {
          flags |= 16; // v1.0 everything
        }

        if (p.SourceColumnNullMapping || !ADP.IsEmpty(p.XmlSchemaCollectionDatabase) ||
            !ADP.IsEmpty(p.XmlSchemaCollectionOwningSchema) || !ADP.IsEmpty(p.XmlSchemaCollectionName)) {
          flags |= 32; // v2.0 everything
        }

        Type[] ctorParams;
        object[] ctorValues;
        switch (flags) {
          case 0: // ParameterName
          case 1: // SqlDbType
            ctorParams = new Type[] { typeof(string), typeof(SqlDbType) };
            ctorValues = new object[] { p.ParameterName, p.SqlDbType };
            break;
          case 2: // Size
          case 3: // Size, SqlDbType
            ctorParams = new Type[] { typeof(string), typeof(SqlDbType), typeof(int) };
            ctorValues = new object[] { p.ParameterName, p.SqlDbType, p.Size };
            break;
          case 4: // SourceColumn
          case 5: // SourceColumn, SqlDbType
          case 6: // SourceColumn, Size
          case 7: // SourceColumn, Size, SqlDbType
            ctorParams = new Type[] { typeof(string), typeof(SqlDbType), typeof(int), typeof(string) };
            ctorValues = new object[] { p.ParameterName, p.SqlDbType, p.Size, p.SourceColumn };
            break;
          case 8: // Value
            ctorParams = new Type[] { typeof(string), typeof(object) };
            ctorValues = new object[] { p.ParameterName, p.Value };
            break;
          default:
            if (0 == (32 & flags)) { // v1.0 everything
              ctorParams = new Type[] {
                                                    typeof(string), typeof(SqlDbType), typeof(int), typeof(ParameterDirection),
                                                    typeof(bool), typeof(byte), typeof(byte),
                                                    typeof(string), typeof(DataRowVersion),
                                                    typeof(object) };
              ctorValues = new object[] {
                                                      p.ParameterName, p.SqlDbType,  p.Size, p.Direction,
                                                      p.IsNullable, p.PrecisionInternal, p.ScaleInternal,
                                                      p.SourceColumn, p.SourceVersion,
                                                      p.Value };
            } else { // v2.0 everything - round trip all browsable properties + precision/scale
              ctorParams = new Type[] {
                                                    typeof(string), typeof(SqlDbType), typeof(int), typeof(ParameterDirection),
                                                    typeof(byte), typeof(byte),
                                                    typeof(string), typeof(DataRowVersion), typeof(bool),
                                                    typeof(object),
                                                    typeof(string), typeof(string),
                                                    typeof(string) };
              ctorValues = new object[] {
                                                      p.ParameterName, p.SqlDbType,  p.Size, p.Direction,
                                                      p.PrecisionInternal, p.ScaleInternal,
                                                      p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping,
                                                      p.Value,
                                                      p.XmlSchemaCollectionDatabase, p.XmlSchemaCollectionOwningSchema,
                                                      p.XmlSchemaCollectionName};
            }
            break;
        }
        var ctor = typeof(SqlParameter).GetConstructor(ctorParams);
        return new System.ComponentModel.Design.Serialization.InstanceDescriptor(ctor, ctorValues);
      }
    }

  }
}