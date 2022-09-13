using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeProjectItem")]
  public class FakeProjectItem {
    [ServiceStack.DataAnnotations.AutoIncrement, LinqToDB.Mapping.Identity, Column] public int Id { get; set; }
    [Column] public int? AssignedToPersonId { get; set; }
    [Column] public int CreatedByPersonId { get; set; }
    [Column] public int PriorityId { get; set; }
    [Column] public int ProjectId { get; set; }

    [Column] public string TaskDescription { get; set; } = string.Empty;
    [Column] public bool IsOnHold { get; set; }
    [Column] public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    [Column] public DateTime? StartDateTime { get; set; }
    [Column] public DateTime? EndDateTime { get; set; }
    [Column] public TimeSpan EstimatedTime { get; set; }
    [Column] public decimal RatePerHour { get; set; }

    public decimal EstimatedCostValue => (decimal)EstimatedTime.TotalHours * RatePerHour;
    public string Status => EndDateTime.HasValue ? "Complete" : IsOnHold ? "OnHold" : "Active";

    public FakePerson? AssignedToPerson { get; set; }
    public FakePerson CreatedByPerson { get; set; } = null!;
    public FakePriority Priority { get; set; } = null!;
    public FakeProject Project { get; set; } = null!;
  }


}
