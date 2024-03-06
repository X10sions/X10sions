using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbComputerTarget")]
public class ComputerTarget {
  [Key] public int TargetID { get; set; }
  [MaxLength(256)] public string ComputerID { get; set; } = string.Empty;//UK
  [MaxLength(85)] public string? SID { get; set; }// [varbinary]
  public DateTime? LastSyncTime { get; set; }
  public DateTime? LastReportedStatusTime { get; set; }
  public DateTime? LastReportedRebootTime { get; set; }
  [MaxLength(56)] public string? IPAddress { get; set; }
  [MaxLength(255)] public string? FullDomainName { get; set; }
  public bool IsRegistered { get; set; }//  [bit]
  public DateTime? LastInventoryTime { get; set; }
  public DateTime? LastNameChangeTime { get; set; }
  public DateTime? EffectiveLastDetectionTime { get; set; }
  public int? ParentServerTargetID { get; set; }
  public int LastSyncResult { get; set; }

  //ALTER TABLE[dbo].[tbComputerTarget] WITH CHECK ADD FOREIGN KEY([TargetID]) REFERENCES[dbo].[tbTarget] ([TargetID])
  //ALTER TABLE [dbo].[tbComputerTarget]  WITH CHECK ADD FOREIGN KEY([ParentServerTargetID]) REFERENCES [dbo].[tbDownstreamServerTarget] ([TargetID])
}

[Table("tbProperty")]
public class Property {

}

[Table("tbLocalizedPropertyForRevision")]
public class LocalizedPropertyForRevision {

}

[Table("tbLocalizedProperty")]
public class LocalizedProperty {

}


[Table("tbRevision")]
public class Revision {

}


[Table("tbUpdate")]
public class Update {

}

[Table("tbCategory")]
public class Category {

}


[Table("tbRevisionSupersedesUpdate")]
public class RevisionSupersedesUpdate {

}

[Table("tbDeployment")]
public class Deployment {

}
[Table("tbRevisionInCategory")]
public class RevisionInCategory {

}


[Table("SUSDB.PUBLIC_VIEWS.vUpdateInstallationInfo ")]
public class vUpdateInstallationInfo {

}

