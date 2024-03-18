using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbComputerTarget")]
public class ComputerTarget {
  [Column][Key] public int TargetID { get; set; }
  [Column][MaxLength(256)] public string ComputerID { get; set; } = string.Empty;//UK
  [Column][MaxLength(85)] public string? SID { get; set; }// [varbinary]
  [Column] public DateTime? LastSyncTime { get; set; }
  [Column] public DateTime? LastReportedStatusTime { get; set; }
  [Column] public DateTime? LastReportedRebootTime { get; set; }
  [Column][MaxLength(56)] public string? IPAddress { get; set; }
  [Column][MaxLength(255)] public string? FullDomainName { get; set; }
  [Column] public bool IsRegistered { get; set; }//  [bit]
  [Column] public DateTime? LastInventoryTime { get; set; }
  [Column] public DateTime? LastNameChangeTime { get; set; }
  [Column] public DateTime? EffectiveLastDetectionTime { get; set; }
  [Column] public int? ParentServerTargetID { get; set; }
  [Column] public int LastSyncResult { get; set; }

  //ALTER TABLE[dbo].[tbComputerTarget] WITH CHECK ADD FOREIGN KEY([TargetID]) REFERENCES[dbo].[tbTarget] ([TargetID])
  //ALTER TABLE [dbo].[tbComputerTarget]  WITH CHECK ADD FOREIGN KEY([ParentServerTargetID]) REFERENCES [dbo].[tbDownstreamServerTarget] ([TargetID])
}

