using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Robot;
[Table("FakeRobot")]
public class FakeRobot {
  [ServiceStack.DataAnnotations.AutoIncrement] public int Id { get; set; }
  public string Name { get; set; }
  public bool IsActivated { get; set; }
  public long CellCount { get; set; }
  public DateTime CreatedDate { get; set; }
}