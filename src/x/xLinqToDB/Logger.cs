using System.IO;
namespace LinqToDB;
[Obsolete("GetRidOf")]
public class xLog {

  public const string LogPath = $"c:\\temp\\xLinqToDB.log";

  public static void Debug(string message) {
    using (var file = new StreamWriter(LogPath, true)) {
      //file.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: {message}");
    }
  }

}
