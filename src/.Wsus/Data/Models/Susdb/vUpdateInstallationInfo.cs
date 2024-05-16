//using System.ComponentModel.DataAnnotations.Schema;

//namespace X10sions.Wsus.Data.Models.Susdb;

//[Table("SUSDB.PUBLIC_VIEWS.vUpdateInstallationInfo ")]
//public class vUpdateInstallationInfo {
//  [Column] public Guid UpdateId { get; set; }
//  [Column] public int ComputerTargetId { get; set; }
//  [Column] public int State { get; set; }

//  /*
//CREATE VIEW [PUBLIC_VIEWS].[vUpdateInstallationInfo]
//AS
//    SELECT
//        UpdateId            = u.UpdateID
//        , ComputerTargetId  = ct.ComputerID
//        , State             = (CASE
//                WHEN usc.SummarizationState IS NULL OR usc.SummarizationState = 1 THEN (
//                    CASE
//                        WHEN ISNULL(u.LastUndeclinedTime, u.ImportedTime) < ct.EffectiveLastDetectionTime THEN 1
//                        ELSE 0
//                    END)
//                ELSE usc.SummarizationState
//            END)
//    FROM
//        dbo.tbUpdate u
//        INNER JOIN dbo.tbRevision r ON u.LocalUpdateID = r.LocalUpdateID
//        INNER JOIN dbo.tbProperty p ON r.RevisionID = p.RevisionID
//        CROSS JOIN tbComputerTarget ct
//        LEFT JOIN tbUpdateStatusPerComputer usc ON u.LocalUpdateID = usc.LocalUpdateID AND ct.TargetID = usc.TargetID
//    WHERE
//        p.ExplicitlyDeployable = 1
//        AND r.IsLatestRevision = 1
//        AND u.IsHidden = 0
//GO


//   */

//}

