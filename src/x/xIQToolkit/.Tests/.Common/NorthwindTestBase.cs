namespace Test {

  public abstract class NorthwindTestBase : QueryTestBase {
    protected Northwind db;

    public override void Setup(string[] args) {
      base.Setup(args);
      db = new Northwind(GetProvider());
    }

  }
}