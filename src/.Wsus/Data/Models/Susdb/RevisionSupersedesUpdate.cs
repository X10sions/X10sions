using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbRevisionSupersedesUpdate")]
public class RevisionSupersedesUpdate {
  [Column(Order = 0), Key] public int RevisionID { get; set; }
  [Column(Order = 1), Key] public Guid SupersededUpdateID { get; set; }

  //ALTER TABLE[dbo].[tbRevisionSupersedesUpdate] WITH CHECK ADD FOREIGN KEY([RevisionID]) REFERENCES[dbo].[tbRevision]  ([RevisionID])

}