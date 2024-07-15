namespace Common.ValueObjects;
public readonly record struct ColourCode(string Value) : IValueObject<string> {
  public static ColourCode From(string code) {
    var colour = new ColourCode (code );
    if (!SupportedColours.Contains(colour)) {
      throw new Exception("UnsupportedColourException(code)");
    }
    return colour;
  }
  public static ColourCode Default { get; } = Black;
  public static ColourCode Black { get; } = new("#000000");
  public static ColourCode White { get; } = new("#FFFFFF");
  public static ColourCode Red { get; } = new("#FF5733");
  public static ColourCode Orange { get; } = new("#FFC300");
  public static ColourCode Yellow { get; } = new("#FFFF66");
  public static ColourCode Green { get; } = new("#CCFF99 ");
  public static ColourCode Blue { get; } = new("#6666FF");
  public static ColourCode Purple { get; } = new("#9966CC");
  public static ColourCode Grey { get; } = new("#999999");

  public static explicit operator ColourCode(string code) => From(code);
  public override string ToString() => Value;
  static IEnumerable<ColourCode> SupportedColours { get; } = [White, Black, Red, Orange, Yellow, Green, Blue, Purple, Grey];

}