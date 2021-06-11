using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  #region Issue 1906
  public class CtqResultModel {
    [Column, PrimaryKey, Identity]
    public int ResultId { get; set; }

    [Column, NotNull]
    public int DefinitionId { get; set; }

    [Association(ThisKey = nameof(DefinitionId), OtherKey = nameof(CtqDefinitionModel.DefinitionId), CanBeNull = false)]
    public CtqDefinitionModel Definition { get; set; } = null!;
  }

  public class CtqDefinitionModel {
    [Column, PrimaryKey, Identity]
    public int DefinitionId { get; set; }

    [Column, NotNull]
    public int SetId { get; set; }

    [Association(ThisKey = nameof(SetId), OtherKey = nameof(CtqSetModel.SetId), CanBeNull = true)]
    public CtqSetModel? Set { get; set; }
  }

  public class CtqSetModel {
    [Column, PrimaryKey, Identity]
    public int SetId { get; set; }

    [Column, NotNull]
    public int SectorId { get; set; }

    [Association(ThisKey = nameof(SectorId), OtherKey = nameof(FtqSectorModel.Id), CanBeNull = false)]
    public FtqSectorModel Sector { get; set; } = null!;
  }

  public class FtqSectorModel {
    [Column, PrimaryKey, Identity]
    public int Id { get; set; }
  }

  #endregion
}
