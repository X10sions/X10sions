using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbRevision")]
public class Revision {
  [Column] public int RevisionID { get; set; }
  [Column] public int LocalUpdateID { get; set; }
  [Column] public int RevisionNumber { get; set; }
  [Column] public DateTime? LastIsLeafChange { get; set; }
  [Column] public bool IsLeaf { get; set; } = true;
  [Column] public bool IsBeta { get; set; }
  [Column] public DateTime? TimeToGoLiveOnCatalog { get; set; }
  [Column] public Guid RowID { get; set; } = Guid.NewGuid();
  [Column] public byte State { get; set; } = 1;
  [Column] public int Origin { get; set; }
  [Column] public bool IsCritical { get; set; }
  [Column] public long LanguageMask { get; set; }
  [Column] public bool IsLatestRevision { get; set; } = true;
  [Column] public bool IsMandatory { get; set; }

  //PRIMARY KEY CLUSTERED (  [RevisionID] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY],
  // UNIQUE NONCLUSTERED(   [RowID] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON[PRIMARY]) ON[PRIMARY]
  //ALTER TABLE[dbo].[tbRevision] WITH CHECK ADD CONSTRAINT[PK_constraint_for_table_tbRevision] FOREIGN KEY([LocalUpdateID]) REFERENCES[dbo].[tbUpdate]   ([LocalUpdateID])
  //ALTER TABLE[dbo].[tbRevision] CHECK CONSTRAINT[PK_constraint_for_table_tbRevision]

}