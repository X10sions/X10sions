namespace Common.Structures {
  public struct BooleanStringNY {
    public BooleanStringNY(bool value) { BoolValue = value; }
    public BooleanStringNY(string value) {
      BoolValue = GetBoolValue(value);
    }
    public const string FalseString = "N";
    public const string TrueString = "Y";

    public bool BoolValue { get; set; }

    public string StringValue {
      get => BoolValue ? TrueString : FalseString;
      set => BoolValue = GetBoolValue(value);
    }

    public static bool GetBoolValue(string value) => value.ToUpper() == TrueString;

  }

  //public enum BooleanStringNY {
  //  [MapValue("N")] False,
  //  [MapValue("Y")] True
  //}

}
