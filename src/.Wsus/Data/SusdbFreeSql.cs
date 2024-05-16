//using X10sions.Wsus.Data.Models.Susdb;

//namespace X10sions.Wsus.Data {


//  public interface IFreeSqlSqlServer : IFreeSql { }

//  public class SusdbFreeSql() {
//    public static IFreeSqlSqlServer GetSqlServer(string connectionString) => (IFreeSqlSqlServer)new FreeSql.FreeSqlBuilder()
//      .UseConnectionString(FreeSql.DataType.SqlServer, connectionString)
//      .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))
//      .UseAutoSyncStructure(true).Build();

//    //  protected override void OnModelCreating(ModelBuilder modelBuilder) {
//    //    base.OnModelCreating(modelBuilder);

//    //    modelBuilder.Entity<LocalizedPropertyForRevision>().HasKey(x => new { x.RevisionID, x.LanguageID, x.LocalizedPropertyID });
//    //    modelBuilder.Entity<RevisionInCategory>().HasKey(x => new { x.RevisionID, x.CategoryID });
//    //    modelBuilder.Entity<RevisionSupersedesUpdate>().HasKey(x => new { x.RevisionID, x.SupersededUpdateID });
//    //    modelBuilder.Entity<UpdateStatusPerComputer>().HasKey(x => new { x.TargetID, x.LocalUpdateID });
//    //  }

//  }

//  public record LocalProperty(int RevisionID, bool ExplicitlyDeployable, bool? CanUninstall, DateTime CreationDate, string Title, string? Description) {
//    public static IQueryable<LocalProperty> Queryable(IFreeSql db)
//      => db.Select<Property>()
//         .InnerJoin<LocalizedPropertyForRevision>((p, lpr) => lpr.RevisionID == p.RevisionID && lpr.LanguageID == p.DefaultPropertiesLanguageID)
//         .InnerJoin<LocalizedProperty>((p, lpr, lp) => lp.LocalizedPropertyID == lpr.LocalizedPropertyID)
//         select new LocalProperty(p.RevisionID, p.ExplicitlyDeployable, p.CanUninstall, p.CreationDate, lp.Title, lp.Description);
//  }

//  public record UpdateRevision(int LocalUpdateId, Guid UpdateId, int RevisionID, int RevisionNumber, bool IsHidden, DateTime ArrivalDateUtc, DateTime? LastUndeclinedTime, bool IsLatestRevision) {
//    public static IQueryable<UpdateRevision> Queryable(IFreeSql db)
//      => from r in db.Set<Revision>()
//         from u in db.Set<Update>().Where(u => u.LocalUpdateID == r.LocalUpdateID) // --and r.IsLatestRevision = 1
//         select new UpdateRevision(u.LocalUpdateID, u.UpdateID, r.RevisionID, r.RevisionNumber, u.IsHidden, u.ImportedTime, u.LastUndeclinedTime, r.IsLatestRevision);

//  }

//  public record Classification(int CategoryID, string Title, string? Description) {
//    public static IQueryable<Classification> Queryable(IFreeSql db)
//      => from c in db.Set<Category>()
//         from ur in UpdateRevision.Queryable(db).Where(ur => c.CategoryID == ur.LocalUpdateId && ur.IsLatestRevision)
//         from lp in LocalProperty.Queryable(db).Where(lp => lp.RevisionID == ur.RevisionID)
//         where c.CategoryType == "UpdateClassification"
//         select new Classification(c.CategoryID, lp.Title, lp.Description);
//  }

//  /// <summary> [PUBLIC_VIEWS].[vUpdateInstallationInfo] </summary>
//  public record UpdateInstallationInfoView(Guid UpdateId, string ComputerTargetId, int State) {

//    public static IQueryable<UpdateInstallationInfoView> Queryable(IFreeSql db)
//      => from u in db.Set<Update>()
//         from r in db.Set<Revision>().Where(r => u.LocalUpdateID == r.LocalUpdateID)
//         from p in db.Set<Property>().Where(p => r.RevisionID == p.RevisionID)
//         from ct in db.Set<ComputerTarget>()//CROSS
//         from usc in db.Set<UpdateStatusPerComputer>().Where(usc => u.LocalUpdateID == usc.LocalUpdateID && ct.TargetID == usc.TargetID).DefaultIfEmpty()
//         where p.ExplicitlyDeployable && r.IsLatestRevision && !u.IsHidden
//         select new UpdateInstallationInfoView(u.UpdateID, ct.ComputerID,
//         usc.SummarizationState == null || usc.SummarizationState == 1 ? ((u.LastUndeclinedTime ?? u.ImportedTime) < ct.EffectiveLastDetectionTime ? 1 : 0) : usc.SummarizationState
//         );


//    /*
//  CREATE VIEW [PUBLIC_VIEWS].[vUpdateInstallationInfo]
//  AS
//      SELECT
//          UpdateId            = u.UpdateID
//          , ComputerTargetId  = ct.ComputerID
//          , State             = (CASE
//                  WHEN usc.SummarizationState IS NULL OR usc.SummarizationState = 1 THEN (
//                      CASE
//                          WHEN ISNULL(u.LastUndeclinedTime, u.ImportedTime) < ct.EffectiveLastDetectionTime THEN 1
//                          ELSE 0
//                      END)
//                  ELSE usc.SummarizationState
//              END)
//      FROM
//          dbo.tbUpdate u
//          INNER JOIN dbo.tbRevision r ON u.LocalUpdateID = r.LocalUpdateID
//          INNER JOIN dbo.tbProperty p ON r.RevisionID = p.RevisionID
//          CROSS JOIN tbComputerTarget ct
//          LEFT JOIN tbUpdateStatusPerComputer usc ON u.LocalUpdateID = usc.LocalUpdateID AND ct.TargetID = usc.TargetID
//      WHERE
//          p.ExplicitlyDeployable = 1
//          AND r.IsLatestRevision = 1
//          AND u.IsHidden = 0
//  GO


//     */

//  }

//  public record UpdateInstallationSummary(Guid UpdateId, int Total, int NoStatus, int NotApplicable, int Needed, int Installed, int Failed) {
//    public static IQueryable<UpdateInstallationSummary> Queryable(IFreeSql db)
//      => from v in UpdateInstallationInfoView.Queryable(db)
//         group v by v.UpdateId into g
//         select new UpdateInstallationSummary(
//           g.Key,
//           g.Count(),
//           g.Sum(s => s.State == 0 ? 1 : 0),
//           g.Sum(s => s.State == 1 ? 1 : 0),
//           g.Sum(s => new[] { 2, 3, 6 }.Contains(s.State) ? 1 : 0),
//           g.Sum(s => s.State == 4 ? 1 : 0),
//           g.Sum(s => s.State == 5 ? 1 : 0)
//         );

//  }

//  public record OldNewUpdate(int RevisionID, Guid OldUpdateId, Guid NewUpdateId) {
//    public static IQueryable<OldNewUpdate> Queryable(IFreeSql db)
//      => from rsu in db.Set<RevisionSupersedesUpdate>()
//         from ur in UpdateRevision.Queryable(db).Where(ur => rsu.RevisionID == ur.RevisionID)
//         select new OldNewUpdate(rsu.RevisionID, rsu.SupersededUpdateID, ur.UpdateId);
//  }

//  public record DeployementSummary(int RevisionID, int ActionId, int Count) {
//    public static IQueryable<DeployementSummary> Queryable(IFreeSql db)
//      => from d in db.Set<Deployment>()
//         group d by new { d.RevisionID, d.ActionID } into g
//         select new DeployementSummary(g.Key.RevisionID, g.Key.ActionID, g.Count());
//  }

//  public record UpdateRevisionSummary(
//    UpdateRevision UpdateRevision
//    , int NoStatus, int NotApplicable, int Needed, int Failed, int Installed
//    , int IsNewerThanCount, int IsOlderThanCount
//    , int ApprovedInstallCount, int ApprovedRemovalCount
//    , DateTime ReleaseDateUtc, bool CanUnInstall, string Title, string ClassificationTitle, string ClassificationDescription) {

//    string UpdateSqlcommand(int localUpdateId, Guid updateId, string sqlCommandName) => $"Set @Message = CONVERT(varchar, SYSDATETIME()) + ': {sqlCommandName}: {localUpdateId.ToString("00000000")}';RAISERROR(@message,0,0) WITH NOWAIT; EXEC {sqlCommandName} '{updateId}';";
//    //public string UpdateSqlcommand(int localUpdateId, Guid updateId, string sqlCommandName) => $"RAISERROR(CONVERT(varchar, SYSDATETIME()) + ': {localUpdateId.ToString("00000000")}',0,0) WITH NOWAIT; EXEC {sqlCommandName} '{updateId}';";


//    public string DeleteSql => UpdateSqlcommand(UpdateRevision.LocalUpdateId, UpdateRevision.UpdateId, "SUSDB.dbo.spDeleteUpdateByUpdateID");
//    public string DeclineSql => UpdateSqlcommand(UpdateRevision.LocalUpdateId, UpdateRevision.UpdateId, "SUSDB.dbo.spDeclineUpdate");
//    public string UndeclineSql => UpdateSqlcommand(UpdateRevision.LocalUpdateId, UpdateRevision.UpdateId, "SUSDB.dbo.spUndeclineUpdate");

//    public static IQueryable<UpdateRevisionSummary> Queryable(IFreeSql db)
//      => from ur in UpdateRevision.Queryable(db)
//         from lp in LocalProperty.Queryable(db).Where(lp => lp.RevisionID == ur.RevisionID && lp.ExplicitlyDeployable)
//         from rc in db.Set<RevisionInCategory>().Where(rc => rc.RevisionID == ur.RevisionID && !rc.Expanded)
//         from c in Classification.Queryable(db).Where(c => c.CategoryID == rc.CategoryID)
//         from uis in UpdateInstallationSummary.Queryable(db).Where(uis => ur.UpdateId == uis.UpdateId).DefaultIfEmpty()
//         from di in DeployementSummary.Queryable(db).Where(di => di.RevisionID == ur.RevisionID && di.ActionId == 0).DefaultIfEmpty()
//         from dr in DeployementSummary.Queryable(db).Where(dr => dr.RevisionID == ur.RevisionID && dr.ActionId == 1).DefaultIfEmpty()
//         where ur.IsLatestRevision
//         select new UpdateRevisionSummary(
//           ur,
//           uis.NoStatus,
//           uis.NotApplicable,
//           uis.Needed,
//           uis.Failed,
//           uis.Installed,
//           OldNewUpdate.Queryable(db).Count(rsu => rsu.RevisionID == ur.RevisionID),
//           OldNewUpdate.Queryable(db).Count(rsu => rsu.OldUpdateId == ur.UpdateId),
//           di.Count, dr.Count, lp.CreationDate, lp.CanUninstall ?? false, lp.Title, c.Title, c.Description ?? string.Empty);
//  }

//}