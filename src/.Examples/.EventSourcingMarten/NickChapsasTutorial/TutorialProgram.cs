using X10sions.Examples.EventSourcingMarten.NickChapsasTutorial;
using X10sions.Examples.EventSourcingMarten.NickChapsasTutorial.Events;


public static class TutorialProrgam {
  // https://www.youtube.com/watch?v=n_o-xuuVtmw

  public static void TutorialMain() {

    var studentDatabase = new StudentDatabase();

    var studentId = Guid.Parse("410efa39-917b-45d4-83ff-f9a618d760a3");


    studentDatabase.Append(new StudentCreated {
      StudentId = studentId,
      Email = "test@fake.com",
      FullName = "Fake Student",
      DateOfBirth = new(1993, 1, 1)
    });


    studentDatabase.Append(new StudentEnrolled {
      StudentId = studentId,
      CourseName = "Test Course 1"
    });


    studentDatabase.Append(new StudentUpdated {
      StudentId = studentId,
      Email = "test2@fake.com",
      FullName  = "Fake Student Name 2"
    });

    Student student = studentDatabase.GetStudent(studentId);
    Student studentFromView = studentDatabase.GetStudentView(studentId);

    Console.WriteLine();
  }
}