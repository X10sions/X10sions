namespace Common.Enums {
  public enum adDataType {
    // https://www.w3schools.com/asp/met_comm_createparameter.asp
    adEmpty = 0,
    adBoolean = 11,
    adDate = 7,
    adDbDate = 133,
    adDbTime = 134,
    adDbTimeStamp = 135,
    adDecimal = 14,
    adDouble = 5,
    adInteger = 3,
    adBigInt = 20,
    adSmallInt = 2,
    adSingle = 4,
    adVarChar = 200
  }

  public static class adDataTypeExtensions {

    public static adDataType GetAdoDataType(string dataTypeName) {
      //' Primitive data types:
      //'   Boolean, Date, Decimal, Double, Integer, Long, Short, Single, String
      //'   Not Used
      //'    Byte, Char, Object, SByte, UInteger, ULong, UShort
      //' iSeries
      //'   dbDate, dbTime, dbTimeStamp
      switch (dataTypeName.ToLower()) {
        case "boolean": return adDataType.adBoolean;
        case "date": return adDataType.adDate;
        case "dbdate": return adDataType.adDbDate;
        case "dbtime": return adDataType.adDbTime;
        case "dbtimestamp": return adDataType.adDbTimeStamp;
        case "decimal": return adDataType.adDecimal;
        case "double": return adDataType.adDouble;
        case "integer": return adDataType.adInteger;
        case "long": return adDataType.adBigInt;
        case "short": return adDataType.adSmallInt;
        case "single": return adDataType.adSingle;
        case "string": return adDataType.adVarChar;
        default: throw new System.Exception($"Data Type '{dataTypeName}' is not defined.");
      }
    }

  }

}