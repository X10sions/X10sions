using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbUpdateStatusPerComputer")]
public class UpdateStatusPerComputer {
  [Column] public int SummarizationState { get; set; }
  [Column(Order = 1), Key] public int LocalUpdateID { get; set; }
  [Column(Order = 0), Key] public int TargetID { get; set; }
  [Column] public DateTime LastChangeTime { get; set; }
  [Column] public DateTime LastRefreshTime { get; set; }
  [Column] public DateTime LastChangeTimeOnServer { get; set; }

  // ALTER TABLE[dbo].[tbUpdateStatusPerComputer] WITH CHECK ADD FOREIGN KEY([LocalUpdateID]) REFERENCES[dbo].[tbUpdate]  ([LocalUpdateID]) ON DELETE CASCADE
  // ALTER TABLE[dbo].[tbUpdateStatusPerComputer] WITH CHECK ADD FOREIGN KEY([TargetID]) REFERENCES[dbo].[tbComputerTarget]   ([TargetID]) ON DELETE CASCADE
}


public record vUpdateStatusPerComputer(Guid UpdateId, string ComputerDomainName, int SummarizationState, DateTime LastChangeTime, DateTime LastRefreshTime, DateTime LastChangeTimeOnServer) {
  public static List<vUpdateStatusPerComputer> GetList(IQueryable<UpdateStatusPerComputer> uscQuery, IQueryable<Update> uQuery, IQueryable<ComputerTarget> ctQuery) {
    var qry = from usc in uscQuery
              from u in uQuery
              from ct in ctQuery
              where ct.FullDomainName != string.Empty
              select new vUpdateStatusPerComputer(u.UpdateID, ct.FullDomainName ?? string.Empty, usc.SummarizationState, usc.LastChangeTime, usc.LastRefreshTime, usc.LastChangeTimeOnServer);
    return qry.ToList();
  }
  /*
select top 100 u.UpdateID, ct.FullDomainName, usc.SummarizationState, usc.LastChangeTime, usc.LastRefreshTime, usc.LastChangeTimeOnServer
From tbUpdateStatusPerComputer usc
Join tbComputerTarget ct on ct.TargetID = usc.TargetID
Join dbo.tbUpdate u ON u.LocalUpdateID = usc.LocalUpdateID 
   */
}

public static class UpdateStatusPerComputerExtensions { 

}