using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbDeployment")]
public class Deployment {
  [Column] public int DeploymentID { get; set; }
  [Column] public DateTime LastChangeTime { get; set; }
  [Column] public long LastChangeNumber { get; set; }
  [Column] public byte DeploymentStatus { get; set; }
  [Column] public int ActionID { get; set; }
  [Column] public DateTime DeploymentTime { get; set; } = DateTime.UtcNow;
  [Column] public DateTime GoLiveTime { get; set; }
  [Column] public DateTime? Deadline { get; set; }
  [Column] public string AdminName { get; set; }
  [Column] public byte DownloadPriority { get; set; } = 1;
  [Column] public Guid DeploymentGuid { get; set; } = Guid.NewGuid();
  [Column] public bool IsAssigned { get; set; }
  [Column] public int RevisionID { get; set; }
  [Column] public Guid TargetGroupID { get; set; }
  [Column] public byte TargetGroupTypeID { get; set; }
  [Column] public bool IsLeaf { get; set; } = true;
  [Column] public string UpdateType { get; set; } = "Software";
  [Column] public bool IsCritical { get; set; }
  [Column] public int Priority { get; set; }
  [Column] public bool? IsFeatured { get; set; }
  [Column] public byte? AutoSelect { get; set; }
  [Column] public byte? AutoDownload { get; set; }
  [Column] public byte? SupersedenceBehavior { get; set; }
  [Column] public bool IsPartOfSet { get; set; }
  [Column] public DateTime? SetCreationTime { get; set; }

  // PRIMARY KEY NONCLUSTERED(  [DeploymentID] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ) ON[PRIMARY]
  // ALTER TABLE[dbo].[tbDeployment] WITH CHECK ADD FOREIGN KEY([RevisionID]) REFERENCES[dbo].[tbRevision]  ([RevisionID])
  // ALTER TABLE[dbo].[tbDeployment] WITH CHECK ADD FOREIGN KEY([TargetGroupID]) REFERENCES[dbo].[tbTargetGroup]   ([TargetGroupID])
  // ALTER TABLE[dbo].[tbDeployment] WITH CHECK ADD CHECK(([IsAssigned]= (1) OR[Deadline] IS NULL))
}