using SqlSugar;
using X10sions.Wsus.Data.Models.Susdb;

namespace X10sions.Wsus.Data;
public class SusdbSqlSugarClient : SqlSugarClient {
  public SusdbSqlSugarClient(string connectionString) : base(GetConnectionConfig(connectionString)) { }

  public static ConnectionConfig GetConnectionConfig(string connectionString) => new ConnectionConfig() {
    ConnectionString = connectionString,
    DbType = DbType.SqlServer,
    IsAutoCloseConnection = true,
    LanguageType = LanguageType.English
  };


  public record LocalProperty(int RevisionID, bool ExplicitlyDeployable, bool? CanUninstall, DateTime CreationDate, string Title, string? Description) {
    public static ISugarQueryable<LocalProperty> Queryable(SqlSugarClient db)
      => db.Queryable<Property>()
         .InnerJoin<LocalizedPropertyForRevision>((p, lpr) => lpr.RevisionID == p.RevisionID && lpr.LanguageID == p.DefaultPropertiesLanguageID)
         .InnerJoin<LocalizedProperty>((p, lpr, lp) => lp.LocalizedPropertyID == lpr.LocalizedPropertyID)
         .Select((p, lpr, lp) => new LocalProperty(p.RevisionID, p.ExplicitlyDeployable, p.CanUninstall, p.CreationDate, lp.Title, lp.Description));
  }

  public record UpdateRevision(int LocalUpdateId, Guid UpdateId, int RevisionID, int RevisionNumber, bool IsHidden, DateTime ArrivalDateUtc, DateTime? LastUndeclinedTime, bool IsLatestRevision) {
    public static ISugarQueryable<UpdateRevision> Queryable(SqlSugarClient db)
      => db.Queryable<Revision>()
         .InnerJoin<Update>((r, u) => u.LocalUpdateID == r.LocalUpdateID) // --and r.IsLatestRevision = 1
         .Select((r, u) => new UpdateRevision(u.LocalUpdateID, u.UpdateID, r.RevisionID, r.RevisionNumber, u.IsHidden, u.ImportedTime, u.LastUndeclinedTime, r.IsLatestRevision));
  }

  public record Classification(int CategoryID, string Title, string? Description) {
    public static ISugarQueryable<Classification> Queryable(SqlSugarClient db)
      => db.Queryable<Category>()
         .InnerJoin(UpdateRevision.Queryable(db), (c, ur) => c.CategoryID == ur.LocalUpdateId && ur.IsLatestRevision)
         .InnerJoin(LocalProperty.Queryable(db), (c, ur, lp) => lp.RevisionID == ur.RevisionID)
         .Where((c, ur) => c.CategoryType == "UpdateClassification")
         .Select((c, ur, lp) => new Classification(c.CategoryID, lp.Title, lp.Description));
  }

  /// <summary> [PUBLIC_VIEWS].[vUpdateInstallationInfo] </summary>
  public record UpdateInstallationInfoView(Guid UpdateId, string ComputerTargetId, int State) {

    public static ISugarQueryable<UpdateInstallationInfoView> Queryable(SqlSugarClient db)
      => db.Queryable<Update>()
         .InnerJoin<Revision>((u, r) => u.LocalUpdateID == r.LocalUpdateID && r.IsLatestRevision)
         .InnerJoin<Property>((u, r, p) => r.RevisionID == p.RevisionID && p.ExplicitlyDeployable)
         .InnerJoin<ComputerTarget>((u, r, p, ct) => true)//CROSS
         .LeftJoin<UpdateStatusPerComputer>((u, r, p, ct, usc) => u.LocalUpdateID == usc.LocalUpdateID && ct.TargetID == usc.TargetID)
         .Where(u => !u.IsHidden)
         .Select((u, r, p, ct, usc) => new UpdateInstallationInfoView(u.UpdateID, ct.ComputerID,
           usc.SummarizationState == null || usc.SummarizationState == 1 ? ((u.LastUndeclinedTime ?? u.ImportedTime) < ct.EffectiveLastDetectionTime ? 1 : 0) : usc.SummarizationState
         ));


    /*
  CREATE VIEW [PUBLIC_VIEWS].[vUpdateInstallationInfo]
  AS
      SELECT
          UpdateId            = u.UpdateID
          , ComputerTargetId  = ct.ComputerID
          , State             = (CASE
                  WHEN usc.SummarizationState IS NULL OR usc.SummarizationState = 1 THEN (
                      CASE
                          WHEN ISNULL(u.LastUndeclinedTime, u.ImportedTime) < ct.EffectiveLastDetectionTime THEN 1
                          ELSE 0
                      END)
                  ELSE usc.SummarizationState
              END)
      FROM
          dbo.tbUpdate u
          INNER JOIN dbo.tbRevision r ON u.LocalUpdateID = r.LocalUpdateID
          INNER JOIN dbo.tbProperty p ON r.RevisionID = p.RevisionID
          CROSS JOIN tbComputerTarget ct
          LEFT JOIN tbUpdateStatusPerComputer usc ON u.LocalUpdateID = usc.LocalUpdateID AND ct.TargetID = usc.TargetID
      WHERE
          p.ExplicitlyDeployable = 1
          AND r.IsLatestRevision = 1
          AND u.IsHidden = 0
  GO


     */

  }

  public record UpdateInstallationSummary(Guid UpdateId, int Total, int NoStatus, int NotApplicable, int Needed, int Installed, int Failed) {
    public static ISugarQueryable<UpdateInstallationSummary> Queryable(SqlSugarClient db)
      => UpdateInstallationInfoView.Queryable(db)
         .GroupBy(v => v.UpdateId)
         .Select(g => new UpdateInstallationSummary(
           g.UpdateId,
           SqlFunc.AggregateSum(1),
           SqlFunc.AggregateSum(g.State == 0 ? 1 : 0),
           SqlFunc.AggregateSum(g.State == 1 ? 1 : 0),
           SqlFunc.AggregateSum(new[] { 2, 3, 6 }.Contains(g.State) ? 1 : 0),
           SqlFunc.AggregateSum(g.State == 4 ? 1 : 0),
           SqlFunc.AggregateSum(g.State == 5 ? 1 : 0)
         ));
  }

  public record OldNewUpdate(int RevisionID, Guid OldUpdateId, Guid NewUpdateId) {
    public static ISugarQueryable<OldNewUpdate> Queryable(SqlSugarClient db)
      => db.Queryable<RevisionSupersedesUpdate>()
         .InnerJoin(UpdateRevision.Queryable(db), (rsu, ur) => rsu.RevisionID == ur.RevisionID)
         .Select((rsu, ur) => new OldNewUpdate(rsu.RevisionID, rsu.SupersededUpdateID, ur.UpdateId));
  }

  public record DeployementSummary(int RevisionID, int ActionId, int Count) {
    public static ISugarQueryable<DeployementSummary> Queryable(SqlSugarClient db)
      => db.Queryable<Deployment>()
         .GroupBy(d => new { d.RevisionID, d.ActionID })
         .Select(g => new DeployementSummary(g.RevisionID, g.ActionID, SqlFunc.AggregateSum(1)));
  }

  public record UpdateRevisionSummary(
    UpdateRevision UpdateRevision
    , int NoStatus, int NotApplicable, int Needed, int Failed, int Installed
    , int IsNewerThanCount, int IsOlderThanCount
    , int ApprovedInstallCount, int ApprovedRemovalCount
    , DateTime ReleaseDateUtc, bool CanUnInstall, string Title, string ClassificationTitle, string ClassificationDescription) {

    string UpdateSqlcommand(int localUpdateId, Guid updateId, string sqlCommandName) => $"Set @Message = CONVERT(varchar, SYSDATETIME()) + ': {sqlCommandName}: {localUpdateId.ToString("00000000")}';RAISERROR(@message,0,0) WITH NOWAIT; EXEC {sqlCommandName} '{updateId}';";
    //public string UpdateSqlcommand(int localUpdateId, Guid updateId, string sqlCommandName) => $"RAISERROR(CONVERT(varchar, SYSDATETIME()) + ': {localUpdateId.ToString("00000000")}',0,0) WITH NOWAIT; EXEC {sqlCommandName} '{updateId}';";


    public string DeleteSql => UpdateSqlcommand(UpdateRevision.LocalUpdateId, UpdateRevision.UpdateId, "SUSDB.dbo.spDeleteUpdateByUpdateID");
    public string DeclineSql => UpdateSqlcommand(UpdateRevision.LocalUpdateId, UpdateRevision.UpdateId, "SUSDB.dbo.spDeclineUpdate");
    public string UndeclineSql => UpdateSqlcommand(UpdateRevision.LocalUpdateId, UpdateRevision.UpdateId, "SUSDB.dbo.spUndeclineUpdate");

    public static ISugarQueryable<UpdateRevisionSummary> Queryable(SqlSugarClient db)
      => UpdateRevision.Queryable(db)
      .InnerJoin(LocalProperty.Queryable(db), (ur, lp) => lp.RevisionID == ur.RevisionID && lp.ExplicitlyDeployable)
      .InnerJoin<RevisionInCategory>((ur, lp, rc) => rc.RevisionID == ur.RevisionID && !rc.Expanded)
      .InnerJoin(Classification.Queryable(db), (ur, lp, rc, c) => c.CategoryID == rc.CategoryID)
      .LeftJoin(UpdateInstallationSummary.Queryable(db), (ur, lp, rc, c, uis) => ur.UpdateId == uis.UpdateId)
      .LeftJoin(DeployementSummary.Queryable(db), (ur, lp, rc, c, uis, di) => di.RevisionID == ur.RevisionID && di.ActionId == 0)
      .LeftJoin(DeployementSummary.Queryable(db), (ur, lp, rc, c, uis, di, dr) => dr.RevisionID == ur.RevisionID && dr.ActionId == 1)
      .Where(ur => ur.IsLatestRevision)
      .Select((ur, lp, rc, c, uis, di, dr) => new UpdateRevisionSummary(
        ur,
        uis.NoStatus,
        uis.NotApplicable,
        uis.Needed,
        uis.Failed,
        uis.Installed,
        OldNewUpdate.Queryable(db).Count(rsu => rsu.RevisionID == ur.RevisionID),
        OldNewUpdate.Queryable(db).Count(rsu => rsu.OldUpdateId == ur.UpdateId),
        di.Count, dr.Count, lp.CreationDate, lp.CanUninstall ?? false, lp.Title, c.Title, c.Description ?? string.Empty));
  }

}
