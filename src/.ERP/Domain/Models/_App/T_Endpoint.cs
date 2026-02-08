namespace X10sions.ERP.Data.Models;

public class T_Endpoint {
  public int Id { get; set; }
  public string Path { get; set; }
  public string Type { get; set; }
  public int CUSC51 { get; set; }
  public int APSR51 { get; set; }
  public int APPL51 { get; set; }
  public int VRSN51 { get; set; }
  public int RLSL51 { get; set; }
  public int MDES51 { get; set; }
  public int DATA51 { get; set; }
}

public class T_EndpointOption {
  public int Id { get; set; }
  public int EndpointId { get; set; }
  public int? Row { get; set; }
  public int? Column { get; set; }
  public int? StartPosition { get; set; }
  public int? EndPosition { get; set; }
  /*
   			ColumnData	COLR61	RLEN61	RWRD61	COLR62	FLEN62	xFO	XMOPT	FORO52	MOPT52	FUNC52	PreProcessingEndpointIdId	TargetEndpointIdId	BatchEndpointIdId	TargetQuery	APPL52	MNDS	CUSC
   */
}

public class T_UserOrGroup {
  public int Id { get; set; }
  public string Name { get; set; }
  public int? GroupId { get; set; }
  public string? DefaultEndpointId { get; set; }
  public int? AM112Id { get; set; }
  public string? ExternalId { get; set; }
  public int? ExternalGroupId { get; set; }
  public string? IsGroup { get; set; }
  public int? SecurityLevel { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
}