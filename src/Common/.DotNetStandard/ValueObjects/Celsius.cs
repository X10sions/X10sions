namespace Common.ValueObjects;
public readonly record struct Celsius(double Value) : IValueObject<double> {
  //public Celsius(int value) :this((double) value){  }
  public double Value { get; init; } = (Value < AbsoluteZeroInCelsius) ? throw new TemperatureBelowAbsoluteZeroException(Value) : Value;

  const double AbsoluteZeroInCelsius = -273.15;

  public class TemperatureBelowAbsoluteZeroException : Exception {
    public TemperatureBelowAbsoluteZeroException(double degrees) : base($"Temperature cannot be below absolute zero. Current value:{degrees}") { }
  }

}

