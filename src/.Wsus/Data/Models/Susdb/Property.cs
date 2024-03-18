using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbProperty")]
public class Property {
  [Column, Key] public int RevisionID { get; set; }
  [Column] public int PublicationState { get; set; }
  [Column] public DateTime CreationDate { get; set; }
  [Column] public DateTime ReceivedFromCreatorService { get; set; }
  [Column] public bool ExplicitlyDeployable { get; set; }
  [Column] public bool? CanInstall { get; set; }
  [Column] public int? InstallationImpact { get; set; }
  [Column] public bool? InstallRequiresConnectivity { get; set; }
  [Column] public bool? InstallRequiresUserInput { get; set; }
  [Column] public int? InstallRebootBehavior { get; set; }
  [Column] public bool? CanUninstall { get; set; }
  [Column] public int? UninstallImpact { get; set; }
  [Column] public bool? UninstallRequiresConnectivity { get; set; }
  [Column] public bool? UninstallRequiresUserInput { get; set; }
  [Column] public int? UninstallRebootBehavior { get; set; }
  [Column] public int? HandlerID { get; set; }
  [Column] public Guid EulaID { get; set; }
  [Column] public bool? RequiresReacceptanceOfEula { get; set; }
  [Column] public int? DefaultPropertiesLanguageID { get; set; }
  [Column] public string UpdateType { get; set; } = "Software";
  [Column] public bool EulaExplicitlyAccepted { get; set; }
  [Column] public string MsrcSeverity { get; set; } = "Unspecified";
  [Column] public string? CompatibleProtocolVersion { get; set; }
  [Column] public bool? OemOnlyDriver { get; set; }
  [Column] public long DriverMinInstalledVersion { get; set; }
  [Column] public long DriverMaxInstalledVersion { get; set; }

  //ALTER TABLE [dbo].[tbProperty]  WITH CHECK ADD FOREIGN KEY([RevisionID])  REFERENCES[dbo].[tbRevision] ([RevisionID])
}

