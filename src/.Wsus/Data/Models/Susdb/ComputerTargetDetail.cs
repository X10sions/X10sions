using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbComputerTargetDetail")]
public class ComputerTargetDetail {
  public const string Windows = "Windows";

  [Key] public int TargetID { get; set; }
  public int? OSMajorVersion { get; set; }
  public int? OSMinorVersion { get; set; }
  public int? OSBuildNumber { get; set; }
  public int? OSServicePackMajorNumber { get; set; }
  public int? OSServicePackMinorNumber { get; set; }
  [MaxLength(10)] public string? OSLocale { get; set; }
  [MaxLength(64)] public string? ComputerMake { get; set; }
  [MaxLength(64)] public string? ComputerModel { get; set; }
  [MaxLength(64)] public string? BiosVersion { get; set; }
  [MaxLength(64)] public string? BiosName { get; set; }
  public DateTime? BiosReleaseDate { get; set; }
  [MaxLength(50)] public string? ProcessorArchitecture { get; set; }
  public DateTime? LastStatusRollupTime { get; set; }
  public int LastReceivedStatusRollupNumber { get; set; }
  public int LastSentStatusRollupNumber { get; set; }
  public int SamplingValue { get; set; }//DEFAULT (CONVERT([int],rand(checksum(newid()))*(1000),0))
  public DateTime CreatedTime { get; set; } = DateTime.Now;
  public short SuiteMask { get; set; }
  public byte OldProductType { get; set; }
  public int NewProductType { get; set; }
  public int SystemMetrics { get; set; }
  [MaxLength(23)] public string? ClientVersion { get; set; }
  public bool TargetGroupMembershipChanged { get; set; }
  [MaxLength(256)] public string OSFamily { get; set; } = Windows;
  [MaxLength(256)] public string? OSDescription { get; set; }
  [MaxLength(64)] public string? OEM { get; set; }
  [MaxLength(64)] public string? DeviceType { get; set; }
  [MaxLength(64)] public string? FirmwareVersion { get; set; }
  [MaxLength(15)] public string? MobileOperator { get; set; }

}

