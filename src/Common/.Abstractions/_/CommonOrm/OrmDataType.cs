namespace CommonOrm {
  public enum OrmDataType {
    //Exact Numerics
    Integer,
    IntegerSmall,
    IntegerBig,
    Numeric,
    Decimal,

    //Approximate Numerics:
    Real,
    DoublePrecision,
    Float,
    DecFloat,

    //Binary Strings:
    Binary,
    BinaryVarying,
    BinaryLargeObject,

    //Boolean:
    Boolean,

    //Character Strings:
    Character,
    CharacterVarying,
    CharacterLargeObject,
    NationalCharacter,
    NationalCharacterVarying,
    NationalCharacterLargeObject,

    //Datetimes:
    Date,
    Time,
    TimeWithTimeZone,
    Timestamp,
    TimestampWithTimeZone,

    //Intervals:
    IntervalYear,
    IntervalMonth,
    IntervalDay,
    IntervalHour,
    IntervalMinute,

    //Collection Types:
    Array,
    Multiset,

    //Other Types:
    Row,
    Xml

  }
}