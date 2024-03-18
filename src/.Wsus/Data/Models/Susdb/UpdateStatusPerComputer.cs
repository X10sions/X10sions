using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbUpdateStatusPerComputer")]
public class UpdateStatusPerComputer {
  [Column] public int SummarizationState { get; set; }
  [Column(Order = 1), Key] public int LocalUpdateID { get; set; }
  [Column(Order = 0), Key] public int TargetID { get; set; }
  [Column] public DateTime LastChangeTime { get; set; }
  [Column] public DateTime LastRefreshTime { get; set; }
  [Column] public DateTime LastChangeTimeOnServer { get; set; }

  // ALTER TABLE[dbo].[tbUpdateStatusPerComputer] WITH CHECK ADD FOREIGN KEY([LocalUpdateID]) REFERENCES[dbo].[tbUpdate]  ([LocalUpdateID]) ON DELETE CASCADE
  // ALTER TABLE[dbo].[tbUpdateStatusPerComputer] WITH CHECK ADD FOREIGN KEY([TargetID]) REFERENCES[dbo].[tbComputerTarget]   ([TargetID]) ON DELETE CASCADE
}