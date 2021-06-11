using System;
using System.Linq.Expressions;
using System.Text;

namespace CommonOrm {
  public interface IOrmProperty { }
  public static class IOrmPropertyExtensions { }

  //PropertyBuilder<TProperty> Property<TProperty>( Expression<Func<TEntity, TProperty>> propertyExpression);
  //PropertyMappingBuilder<T>  Property           ( Expression<Func<T      , object   >> func);
  public class OrmProperty<TEntity > : IOrmProperty where TEntity : class {
    public OrmProperty(
      xIOrmClass<TEntity> ormClass,
      Expression<Func<TEntity, object>> propertyExpression
      ) {
      PropertyExpression = propertyExpression;
      //   MemberInfo = propertyExpression.GetMemberInfo();
      OrmClass = ormClass;
      //OrmClass.Properties.SetProperty(propertyExpression);
      OrmClass.SetProperty(this);
    }

    readonly xIOrmClass<TEntity> OrmClass;

    // public MemberInfo MemberInfo { get; }
    // public PropertyInfo PropertyInfo { get; }
    public Expression<Func<TEntity, object>> PropertyExpression { get; }

    [Obsolete("Use FluentMappingBuilderExtensions.AddAssociation")] public string? AssociationPredicateExpressionName { get; set; }
    public OrmJoinType AssociationJoinType { get; set; }
    //[Obsolete("TESTING")] public Expression<Func<TEntity, object>> AssociationPropertyExpression { get; }
    //[Obsolete("TESTING")] public Expression<Func<TEntity, object, bool>> AssociationPredicateExpression { get; }

    public string Name  => PropertyExpression.GetMemberInfo().Name;

    #region "Column"

    public string? ColumnName { get; set; }
    public string? ColumnSqlExpression { get; set; }

    //public Type Type { get; set; }
    //public DataType DataType { get; set; }
    //public DbType DbType { get; set; }
    //public SqlDbType SqlDbType { get; set; }
    public string? DatabaseTypeName { get; set; }

    public int? MaxLength { get; set; }//DecimalPrecision
    public int? AllocatedLength { get; set; }
    public OrmDataLengthType CharacterLengthType { get; set; } = OrmDataLengthType._Unknown;
    public OrmBasicDataType BasicDataType { get; set; } = OrmBasicDataType._Unknown;
    //public int? Precision { get; set; }
    public int? DecimalPlaces { get; set; } //DecimalScale
    public OrmLargeObjectMultiplier LargeObjectMultiplier { get; set; } = OrmLargeObjectMultiplier._None;

    public bool IsNotNull { get; set; }

    public bool IsAscii => !IsUnicode;
    public bool IsIsNational => IsUnicode;

    public bool IsFixedLength => CharacterLengthType == OrmDataLengthType.Fixed;
    public bool IsVaryingLength => CharacterLengthType == OrmDataLengthType.Varying;
    public bool IsLargeObject => CharacterLengthType == OrmDataLengthType.LargeObject;

    public bool HasDate { get; set; }
    public bool HasTime { get; set; }
    public bool HasTimeZone { get; set; }

    public bool IsUnique { get; set; }
    public bool IsUnicode { get; set; } //National
    //TODO  public object DefaultValue { get; set; }
    //TODO  public string DefaultValueSql { get; set; }
    //TODO  public string DefaultInsertExpression { get; set; }
    //TODO  public string DefaultUpdateExpression { get; set; }
    //TODO  public string DefaultDeleteExpression { get; set; }

    public int? PrimaryKeyOrder { get; private set; }

    public decimal? IdentityStartValue { get; set; } //Seed
    public decimal? IdentityIncrement { get; set; }

    #endregion

    public bool SkipOnInsert { get; set; }
    public bool SkipOnUpdate { get; set; }

    //public bool IsColumn() => ColumnName != null;//|| ComputedColumnSql != null;
    public bool IsPrimaryKey() => PrimaryKeyOrder != null;

    //public bool IsDiscriminator { get; set; }

    //public OrmProperty<TEntity> SetDbTypeVarChar(int? maxLength = null, bool isNotNull = false) => SetDbType(DbType.AnsiString) SetMaxLength(maxLength).SetIsFixedLength(IsFixedLength).SetIsNotNull(isNotNull);
    //public OrmProperty<TEntity> SetDbTypeFxdChar(int? maxLength = null, bool isNotNull = false) => SetDbType(DbType.AnsiStringFixedLength).SetMaxLength(maxLength).SetIsFixedLength(isFixedLength).SetIsNotNull(isNotNull);

    //public OrmProperty<TEntity> SetDbTypeFxdChar(int? maxLength = null, bool isNotNull = false) => SetDbType(DbType.AnsiStringFixedLength).SetMaxLength(maxLength).SetIsFixedLength(isFixedLength).SetIsNotNull(isNotNull);

    //public OrmProperty<TEntity> SetColumn(string name, string sqlExpression) { ColumnName = name; ColumnSqlExpression = sqlExpression; return this; }
    public OrmProperty<TEntity> SetColumnName(string name) { ColumnName = name; return this; }
    public OrmProperty<TEntity> SetColumnSqlExpression(string sqlExpression) { ColumnSqlExpression = sqlExpression; return this; }

    private OrmProperty<TEntity> SetBasicDataType(OrmBasicDataType value) { BasicDataType = value; return this; }
    private OrmProperty<TEntity> SetAllocatedLength(int? value) { AllocatedLength = value; return this; }
    private OrmProperty<TEntity> SetCharacterLengthType(OrmDataLengthType value) { CharacterLengthType = value; return this; }

    private OrmProperty<TEntity> SetHasDate(bool value) { HasDate = value; return this; }
    private OrmProperty<TEntity> SetHasTime(bool value) { HasTime = value; return this; }
    private OrmProperty<TEntity> SetHasTimeZone(bool value) { HasTimeZone = value; return this; }

    public OrmProperty<TEntity> SetIsNotNull(bool value = true) { IsNotNull = value; return this; }
    private OrmProperty<TEntity> SetIsUnicode(bool value = true) { IsUnicode = value; return this; }
    private OrmProperty<TEntity> SetLargeObjectMultiplier(OrmLargeObjectMultiplier value) { LargeObjectMultiplier = value; return this; }
    private OrmProperty<TEntity> SetMaxLength(int? value) { MaxLength = value; return this; }
    private OrmProperty<TEntity> SetDecimalPlaces(int? value) { DecimalPlaces = value; return this; }
    //private OrmProperty<TEntity> SetScale(int? value) { Scale = value; return this; }

    private OrmProperty<TEntity> SetLength(OrmBasicDataType basicDataType, OrmDataLengthType lengthType, int maxLength, bool isNotNull = false) =>
      SetBasicDataType(basicDataType).
      SetCharacterLengthType(lengthType).
      SetMaxLength(maxLength).
      SetIsNotNull(isNotNull);

    public OrmProperty<TEntity> SetDataTypeBinaryFixedLength(int maxLength, bool isNotNull = false) => SetLength(OrmBasicDataType.Binary, OrmDataLengthType.Fixed, maxLength, isNotNull);
    public OrmProperty<TEntity> SetDataTypeBinaryVarying(int maxLength, bool isNotNull = false, int? allocatedLength = null) => SetLength(OrmBasicDataType.Binary, OrmDataLengthType.Varying, maxLength, isNotNull).SetAllocatedLength(allocatedLength);
    public OrmProperty<TEntity> SetDataTypeBinaryLargeObject(int maxLength, bool isNotNull = false, OrmLargeObjectMultiplier multiplier = OrmLargeObjectMultiplier._None) => SetLength(OrmBasicDataType.Binary, OrmDataLengthType.LargeObject, maxLength, isNotNull).SetLargeObjectMultiplier(multiplier);

    public OrmProperty<TEntity> SetDataTypeBoolean(bool isNotNull = false) => SetBasicDataType(OrmBasicDataType.Boolean).SetIsNotNull(isNotNull);

    //Non-Unicode or Ascii
    public OrmProperty<TEntity> SetDataTypeCharacterFixedLength(int maxLength, bool isNotNull = false) => SetLength(OrmBasicDataType.CharacterString, OrmDataLengthType.Fixed, maxLength, isNotNull);
    public OrmProperty<TEntity> SetDataTypeCharacterVarying(int maxLength, bool isNotNull = false, int? allocatedLength = null) => SetLength(OrmBasicDataType.CharacterString, OrmDataLengthType.Varying, maxLength, isNotNull).SetAllocatedLength(allocatedLength);
    public OrmProperty<TEntity> SetDataTypeCharacterLargeObject(int maxLength, bool isNotNull = false, OrmLargeObjectMultiplier multiplier = OrmLargeObjectMultiplier._None) => SetLength(OrmBasicDataType.CharacterString, OrmDataLengthType.LargeObject, maxLength, isNotNull).SetLargeObjectMultiplier(multiplier);
    //Unicode or National
    public OrmProperty<TEntity> SetDataTypeCharacterFixedLengthUnicode(int maxLength, bool isNotNull = false) => SetDataTypeCharacterFixedLength(maxLength, isNotNull).SetIsUnicode(true);
    public OrmProperty<TEntity> SetDataTypeCharacterVaryingUnicode(int maxLength, bool isNotNull = false, int? allocatedLength = null) => SetDataTypeCharacterVarying(maxLength, isNotNull, allocatedLength).SetIsUnicode(true);
    public OrmProperty<TEntity> SetDataTypeCharacterLargeObjectUnicode(int maxLength, bool isNotNull = false, OrmLargeObjectMultiplier multiplier = OrmLargeObjectMultiplier._None) => SetDataTypeCharacterLargeObject(maxLength, isNotNull, multiplier).SetIsUnicode(true);

    public OrmProperty<TEntity> SetDataTypeDecimal(int maxLength, int? decimalPlaces, bool isNotNull = false) => SetLength(OrmBasicDataType.Numeric, OrmDataLengthType.Decimal, maxLength, isNotNull).SetDecimalPlaces(decimalPlaces);

    public OrmProperty<TEntity> SetDataTypeDate(bool isNotNull = false) => SetBasicDataType(OrmBasicDataType.DateTime).SetIsNotNull(isNotNull).SetHasDate(true);
    public OrmProperty<TEntity> SetDataTypeTime(bool isNotNull = false) => SetBasicDataType(OrmBasicDataType.DateTime).SetIsNotNull(isNotNull).SetHasTime(true);
    public OrmProperty<TEntity> SetDataTypeTimeWithTimeZone(bool isNotNull = false) => SetDataTypeTime(isNotNull).SetHasTimeZone(true);
    public OrmProperty<TEntity> SetDataTypeTimestamp(bool isNotNull = false) => SetDataTypeDate(isNotNull).SetHasTime(true);
    public OrmProperty<TEntity> SetDataTypeTimestampWithTimeZone(bool isNotNull = false) => SetDataTypeTimestamp(isNotNull).SetHasTimeZone(true);

    //    public OrmProperty<TEntity> SetDataTypeInt(bool isNotNull = false) => SetBasicDataType(OrmBasicDataType.Numeric).SetIsNotNull(isNotNull).SetHasDate(true);
    //    public OrmProperty<TEntity> SetDataTypeIntBig(bool isNotNull = false) => SetBasicDataType(OrmBasicDataType.Numeric).SetIsNotNull(isNotNull).SetHasDate(true);
    //    public OrmProperty<TEntity> SetDataTypeIntSmall(bool isNotNull = false) => SetBasicDataType(OrmBasicDataType.Numeric).SetIsNotNull(isNotNull).SetHasDate(true);

    //public OrmProperty<TEntity> SetDbType(DbType value) { DbType = value; return this; }

    public OrmProperty<TEntity> SetPrimaryKeyOrder(int? value) { PrimaryKeyOrder = value; return this; }
    //public OrmProperty<TEntity> SetAssociation(string predicateExpressionName, OrmJoinType joinType = OrmJoinType.Left) {
    //  AssociationPredicateExpressionName = predicateExpressionName;
    //  AssociationJoinType = joinType;
    //  return this;
    //}

   public OrmProperty<TEntity> SetIdentity(int? incrementValue = 1, int? startValue = 1) {
      IdentityIncrement = incrementValue;
      IdentityStartValue = startValue;
      IsNotNull = true;
      return this;
    }

    //public OrmProperty<TEntity> HasAttribute(Attribute attribute) {  OrmClass.HasAttribute(MemberInfo, attribute);      return this;    }

    //[Obsolete("TESTING")]
    //public OrmProperty<TEntity> Association<S, ID1, ID2>(
    //  Expression<Func<TEntity, S>> prop,
    //  Expression<Func<TEntity, ID1>> thisKey,
    //  Expression<Func<S, ID2>> otherKey) => OrmClass.SetAssociation(prop,  thisKey,  otherKey);

    //[Obsolete("TESTING")]
    //public OrmProperty<TEntity> Association<TOther>(
    //  Expression<Func<TEntity, IEnumerable<TOther>>> prop,
    //  Expression<Func<TEntity, TOther, bool>> predicate,
    //  bool canBeNull = true
    //  ) => OrmClass.SetAssociation(prop, predicate, canBeNull);

    //[Obsolete("TESTING")]
    //public OrmProperty<TEntity> Association<TOther>(
    //  Expression<Func<TEntity, TOther>> prop,
    //  Expression<Func<TEntity, TOther, bool>> predicate,
    //  bool canBeNull = true
    //  ) => OrmClass.SetAssociation(prop, predicate, canBeNull);

    public string ColumnTypeAndLengthAndNotNull() {
      var sb = new StringBuilder($" {DatabaseTypeName} ");
      if (MaxLength.HasValue) {
        sb.Append($"({MaxLength}");
        if (DecimalPlaces.HasValue) {
          sb.Append($", {DecimalPlaces}");
        }
        sb.Append($")");
        if (AllocatedLength.HasValue) {
          sb.Append($" Allocate({AllocatedLength})");
        }
      }
      if (IsNotNull) {
        sb.Append($" Is Not Null");
      }
      return sb.ToString();
    }

  }
}