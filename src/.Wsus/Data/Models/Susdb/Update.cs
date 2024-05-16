using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbUpdate")]
public class Update {
  [Column] public int LocalUpdateID { get; set; }
  [Column] public Guid UpdateID { get; set; }
  [Column] public Guid UpdateTypeID { get; set; }
  [Column] public bool IsClientSelfUpdate { get; set; }
  [Column] public Guid PublisherID { get; set; }
  [Column] public bool IsPublic { get; set; }
  [Column] public bool IsHidden { get; set; }
  [Column] public string? DetectoidType { get; set; }
  [Column] public string? LegacyName { get; set; }
  [Column] public DateTime? LastUndeclinedTime { get; set; }
  [Column] public bool IsLocallyPublished { get; set; }
  [Column] public DateTime ImportedTime { get; set; }
}