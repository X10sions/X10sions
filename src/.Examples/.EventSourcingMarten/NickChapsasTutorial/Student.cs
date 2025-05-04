using System.Linq;
using X10sions.Examples.EventSourcingMarten.NickChapsasTutorial.Events;

namespace X10sions.Examples.EventSourcingMarten.NickChapsasTutorial;

public class Student {
  public Guid Id { get; set; }
  public string FullName { get; set; }
  public string Email { get; set; }
  public List<string> EnrolledCourses { get; set; } = new();
  public DateTime DateOfBirth { get; set; }

  public void Apply(Event @event) {
    switch (@event) {
      case StudentCreated studentCreated: Apply(studentCreated); break;
      case StudentUpdated studentUpdated: Apply(studentUpdated); break;
      case StudentEnrolled studentEnrolled: Apply(studentEnrolled); break;
      case StudentUnEnrolled studentUnEnrolled: Apply(studentUnEnrolled); break;

    }
  }

  private void Apply(StudentCreated studentCreated) {
    Id = studentCreated.StudentId;
    FullName = studentCreated.FullName;
    Email = studentCreated.Email;
  }

  private void Apply(StudentUpdated studentUpdated) {
    FullName = studentUpdated.FullName;
    Email = studentUpdated.Email;
  }

  private void Apply(StudentEnrolled studentEnrolled) {
    if (!EnrolledCourses.Contains(studentEnrolled.CourseName)) {
      EnrolledCourses.Add(studentEnrolled.CourseName);
    }
  }

  private void Apply(StudentUnEnrolled studentUnEnrolled) {
    if (EnrolledCourses.Contains(studentUnEnrolled.CourseName)) {
      EnrolledCourses.Remove(studentUnEnrolled.CourseName);
    }
  }



}
