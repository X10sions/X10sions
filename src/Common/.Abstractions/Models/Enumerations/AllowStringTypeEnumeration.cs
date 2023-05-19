namespace Common.Models.Enumerations {

  public class AllowStringTypeEnumeration : Enumeration<int, string> {
    public static AllowStringTypeEnumeration Any = new AllowStringTypeEnumeration(1, "Any");
    public static AllowStringTypeEnumeration NotNull = new AllowStringTypeEnumeration(2, "Not Null");
    public static AllowStringTypeEnumeration NotNullOrEmpty = new AllowStringTypeEnumeration(3, "Not Null Or Empty");
    public static AllowStringTypeEnumeration NotNullOrWhitespace = new AllowStringTypeEnumeration(3, "Not Null Or Whitespace");

    //protected AllowStringTypeEnumeration() { }
    public AllowStringTypeEnumeration(int id, string name) : base(id, name) { }

    public static IEnumerable<AllowStringTypeEnumeration> List { get; } = new[] { Any, NotNull, NotNullOrEmpty, NotNullOrWhitespace };

  }
}