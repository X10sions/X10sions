using X10sions.Examples.EventSourcingMarten.NickChapsasTutorial.Events;
using Amazon.DynamoDBv2;

namespace X10sions.Examples.EventSourcingMarten.NickChapsasTutorial;

public class StudentDatabase {

  private readonly Dictionary<Guid, SortedList<DateTime, Event>> _studentEvents = new();
  private readonly Dictionary<Guid, Student> _students = new();

  //private readonly IAmazonDynamoDB _amazonDynamoDB = new AmazonDynamoDBClient(Amazon.RegionEndpoint.EUWest2);
  //private const string TableName = "students";

  //public async Task AppendAmazonDynamoDBAsync<T>(T @event) where T: Event { 
  //} 


  public void Append(Event @event) {
    var stream = _studentEvents!.GetValueOrDefault(@event.StreamId, null);
    if (stream is null) {
      _studentEvents[@event.StreamId] = new SortedList<DateTime, Event>();
    }
    @event.CreatedAtUtc = DateTime.UtcNow;
    _studentEvents[@event.StreamId].Add(@event.CreatedAtUtc, @event);

    _students[@event.StreamId] = GetStudent(@event.StreamId)!;
  }

  public Student? GetStudent(Guid studentId) {
    if (!_studentEvents.ContainsKey(studentId)) {
      return null;
    }
    var student = new Student();
    var studentEvents = _studentEvents[studentId];
    foreach (var studentEvent in studentEvents) {
      student.Apply(studentEvent.Value);
    }
    return student;
  }

  public Student? GetStudentView(Guid studentId) {
    return _students!.GetValueOrDefault(studentId, null);
  }


}
