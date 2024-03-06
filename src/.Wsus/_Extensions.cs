using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace X10sions.Wsus {
  public static class _Extensions {

    public static DataTable GetDataTable(this DbConnection conn, string sql) {
      DataTable dt = new DataTable();
      dt.Load(conn, sql);
      return dt;
    }

    public static void Load(this DataTable dt, DbConnection conn, string commandText) {
      using (DbCommand cmd = conn.CreateCommand()) {
        cmd.CommandText = commandText;
        dt.Load(cmd.ExecuteReader());
      }
    }

    public static void UseChromeoPrintToPdf(string url, string pdfName = "c:\\temp\\printout.pdf") {
      // "https://stackoverflow.com/questions/564650/convert-html-to-pdf-in-net"
      string chromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
      //string output = Path.Combine(Environment.CurrentDirectory, pdfName);
      using (Process p = new Process()) {
        p.StartInfo.FileName = chromePath;
        p.StartInfo.Arguments = $"--headless --disable-gpu --print-to-pdf={pdfName} {url}";
        p.Start();
        p.WaitForExit();
      }
    }

    public static DataTable GetDataTableComputerTarget(this DbConnection conn) {
      string commandText = @"
Select
c.Name
, c.ComputerTargetId
, c.IPAddress
, c.LastSyncResult
, c.LastSyncTime
, c.LastReportedStatusTime
, c.ClientVersion
, c.OSArchitecture
, c.Make
, c.Model
, c.BiosName
, c.BiosVersion
, c.BiosReleaseDate
, c.OSMajorVersion
, c.OSMinorVersion
, c.OSBuildNumber
, c.OSServicePackMajorNumber
, c.OSDefaultUILanguage
From [PUBLIC_VIEWS].[vComputerTarget] c
order by 1;";
      return GetDataTable(conn, commandText);
    }

    public static DataTable GetDataTableComputerTarget2(this DbConnection conn) {
      string commandText = @"SELECT
C.FULLDOMAINNAME AS ComputertName
, VU.DEFAULTTITLE AS UpdateName
, CASE WHEN UP.SummarizationState = 1 THEN 'NotNeeded'
WHEN UP.SummarizationState = 2 THEN 'Needed'
WHEN UP.SummarizationState = 3 THEN 'Downloaded'
WHEN UP.SummarizationState = 4 THEN 'Installed'
WHEN UP.SummarizationState = 5 THEN 'Failed'
WHEN UP.SummarizationState = 6 THEN 'RebootRequired'
END As Status
FROM TBCOMPUTERTARGET C
INNER JOIN tbUpdateStatusPerComputer UP ON C.TargetID = UP.TargetID
INNER JOIN tbUpdate U ON UP.LocalUpdateID = U.LocalUpdateID
INNER JOIN [PUBLIC_VIEWS].[vUpdate] VU ON U.UpdateID = VU.UpdateId
";
      return GetDataTable(conn, commandText);
    }

    public static DataTable GetDataTableComputerTarget3(this DbConnection conn) {
      string commandText = @"With UpdateSummary As(
Select UP.TargetID
, Sum(CASE WHEN UP.SummarizationState = 1 THEN 1 ELSE 0 END) NotNeeded
, Sum(CASE WHEN UP.SummarizationState = 2 THEN 1 ELSE 0 END) Needed
, Sum(CASE WHEN UP.SummarizationState = 3 THEN 1 ELSE 0 END) Downloaded
, Sum(CASE WHEN UP.SummarizationState = 4 THEN 1 ELSE 0 END) Installed
, Sum(CASE WHEN UP.SummarizationState = 5 THEN 1 ELSE 0 END) Failed
, Sum(CASE WHEN UP.SummarizationState = 6 THEN 1 ELSE 0 END) RebootRequired
, Sum(CASE WHEN UP.SummarizationState Between 1 And 6 THEN 0 ELSE 1 END) Other
From tbUpdateStatusPerComputer UP
Group By UP.TargetID
)
SELECT
C.FULLDOMAINNAME AS ComputertName
, C.LastSyncTime
, c.LastReportedStatusTime
, c.LastReportedRebootTime
, c.IPAddress
, c.EffectiveLastDetectionTime
, c.LastSyncResult
, UP.NotNeeded
, UP.Needed
, UP.Downloaded
, UP.Installed
, UP.Failed
, UP.RebootRequired
, (UP.Needed + UP.Downloaded + UP.Failed + up.RebootRequired) ToBeInstalled
, Case When UP.RebootRequired > 0 Then 'Reboot'
When UP.Failed > 0 Then 'Failed'
When UP.Downloaded > 0 Then 'Downloaded '
When UP.Needed > 0 Then 'Needed'
Else ''
End Status
FROM TBCOMPUTERTARGET C
INNER JOIN UpdateSummary UP ON C.TargetID = UP.TargetID
Order By ComputertName
";
      return GetDataTable(conn, commandText);
    }

  }
}
