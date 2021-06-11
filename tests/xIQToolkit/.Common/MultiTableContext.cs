using IQToolkit;
using IQToolkit.Data.Mapping;

namespace Test {

  public class MultiTableEntity {
    public int ID;
    public string Value1;
    public string Value2;
    public string Value3;
  }

  public class MultiTableContext {
    private readonly IEntityProvider provider;

    public MultiTableContext(IEntityProvider provider) {
      this.provider = provider;
    }

    public IEntityProvider Provider => provider;

    [Table(Name = "TestTable1")]
    [ExtensionTable(Name = "TestTable2", KeyColumns = "ID", RelatedKeyColumns = "ID")]
    [ExtensionTable(Name = "TestTable3", KeyColumns = "ID", RelatedKeyColumns = "ID")]
    [Column(Member = "ID", IsPrimaryKey = true, IsGenerated = true)]
    [Column(Member = "Value1")]
    [Column(Member = "Value2", TableId = "TestTable2")]
    [Column(Member = "Value3", TableId = "TestTable3")]
    public IUpdatable<MultiTableEntity> MultiTableEntities => provider.GetTable<MultiTableEntity>();
  }
}
