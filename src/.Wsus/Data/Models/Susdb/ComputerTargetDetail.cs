using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbComputerTargetDetail")]
public class ComputerTargetDetail {
  public const string Windows = "Windows";

  [Column][Key] public int TargetID { get; set; }
  [Column] public int? OSMajorVersion { get; set; }
  [Column] public int? OSMinorVersion { get; set; }
  [Column] public int? OSBuildNumber { get; set; }
  [Column] public int? OSServicePackMajorNumber { get; set; }
  [Column] public int? OSServicePackMinorNumber { get; set; }
  [Column][MaxLength(10)] public string? OSLocale { get; set; }
  [Column][MaxLength(64)] public string? ComputerMake { get; set; }
  [Column][MaxLength(64)] public string? ComputerModel { get; set; }
  [Column][MaxLength(64)] public string? BiosVersion { get; set; }
  [Column][MaxLength(64)] public string? BiosName { get; set; }
  [Column] public DateTime? BiosReleaseDate { get; set; }
  [Column][MaxLength(50)] public string? ProcessorArchitecture { get; set; }
  [Column] public DateTime? LastStatusRollupTime { get; set; }
  [Column] public int LastReceivedStatusRollupNumber { get; set; }
  [Column] public int LastSentStatusRollupNumber { get; set; }
  [Column] public int SamplingValue { get; set; }//DEFAULT (CONVERT([int],rand(checksum(newid()))*(1000),0))
  [Column] public DateTime CreatedTime { get; set; } = DateTime.Now;
  [Column] public short SuiteMask { get; set; }
  [Column] public byte OldProductType { get; set; }
  [Column] public int NewProductType { get; set; }
  [Column] public int SystemMetrics { get; set; }
  [Column][MaxLength(23)] public string? ClientVersion { get; set; }
  [Column] public bool TargetGroupMembershipChanged { get; set; }
  [Column][MaxLength(256)] public string OSFamily { get; set; } = Windows;
  [Column][MaxLength(256)] public string? OSDescription { get; set; }
  [Column][MaxLength(64)] public string? OEM { get; set; }
  [Column][MaxLength(64)] public string? DeviceType { get; set; }
  [Column][MaxLength(64)] public string? FirmwareVersion { get; set; }
  [Column][MaxLength(15)] public string? MobileOperator { get; set; }
}

