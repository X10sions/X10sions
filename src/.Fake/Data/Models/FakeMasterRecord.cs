using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeMasterRecord")]
  public class FakeMasterRecord {
    public Guid Id { get; set; }
    public int RobotId { get; set; }
    public string RobotName { get; set; }
    public DateTime? LastActivated { get; set; }
  }

}
