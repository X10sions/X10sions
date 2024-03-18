using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbRevisionInCategory")]
public class RevisionInCategory {
  [Column(Order = 0), Key] public int RevisionID { get; set; }
  [Column(Order = 1), Key] public int CategoryID { get; set; }
  [Column] public bool Expanded { get; set; }

  //ALTER TABLE[dbo].[tbRevisionInCategory] ADD DEFAULT((0)) FOR[Expanded]
  //ALTER TABLE[dbo].[tbRevisionInCategory] WITH CHECK ADD FOREIGN KEY([CategoryID]) REFERENCES[dbo].[tbCategory]  ([CategoryID])
  //ALTER TABLE[dbo].[tbRevisionInCategory] WITH CHECK ADD FOREIGN KEY([RevisionID]) REFERENCES[dbo].[tbRevision]   ([RevisionID])
}