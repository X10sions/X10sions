namespace Common.Domain.ValueObjects;
public record Celsius {
  public Celsius(double value) {
    Value = (value < AbsoluteZeroInCelsius) ? throw new TemperatureBelowAbsoluteZeroException(Value) : value;
  }
  //public Celsius(int value) :this((double) value){  }
  public double Value { get; init; }

  const double AbsoluteZeroInCelsius = -273.15;

  public class TemperatureBelowAbsoluteZeroException : Exception {
    public TemperatureBelowAbsoluteZeroException(double degrees) : base($"Temperature cannot be below absolute zero. Current value:{degrees}") { }
  }

}
