namespace Common.Structures {
  public struct BooleanStringEmptyNY {
    public BooleanStringEmptyNY(bool? value) { BoolValue = value; }
    public BooleanStringEmptyNY(string value) {
      BoolValue = GetBoolValue(value);
    }

    public const string FalseString = "N";
    public const string TrueString = "Y";

    public bool? BoolValue { get; set; }

    public string StringValue {
      get => BoolValue.HasValue ? (BoolValue.Value ? TrueString : FalseString) : string.Empty;
      set => BoolValue = GetBoolValue(value);
    }

    public static bool? GetBoolValue(string value)
      => string.IsNullOrWhiteSpace(value) ? null : (bool?)(value.ToUpper() == TrueString);

  }

  //public enum BooleanStringEmptyNY {
  //  [MapValue("")] Empty,
  //  [MapValue("N")] False,
  //  [MapValue("Y")] True
  //}

}