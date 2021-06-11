using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  public enum TestEnum {
    [MapValue("A")] AA,
    [MapValue("B")] BB,
  }
}
