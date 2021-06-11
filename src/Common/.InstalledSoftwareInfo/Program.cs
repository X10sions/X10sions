using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace Common.InstalledSoftwareInfo {
  static class Program {
    static void Main(string[] args) {

      Console.WriteLine($"{nameof(InstalledSoftwareInfo)} (v{AppVersion})");

      var computerInfo = new InstalledSoftwareInfo(Registry.LocalMachine);
      Console.WriteLine($"===== computerInfo: \n{computerInfo}");

      var userInfo = new InstalledSoftwareInfo(Registry.CurrentUser);
      Console.WriteLine($"===== userInfo: \n{userInfo}");

      if (args.Length > 0) {
        var exportDirectory = Path.GetDirectoryName(args[0]);
        Export(exportDirectory, args.Length > 1 ? args[1] : $"{nameof(InstalledSoftwareInfo)}-{Environment.MachineName}.json", computerInfo.JsonString);
        Export(exportDirectory, args.Length > 2 ? args[1] : $"{nameof(InstalledSoftwareInfo)}-{Environment.MachineName}-{Environment.UserName}.json", userInfo.JsonString);
      }
      //var endApp = false;
      //var endKey = "x";
      //while(!endApp) {
      //  Console.Write($"Press '{endKey}' and Enter to close the app, or press any other key and Enter to continue: ");
      //  if(Console.ReadLine() == endKey)
      //    endApp = true;
      //}
    }

    static void Export(string exportDirectory, string exportFile, string contents) {
      var exportPath = Path.Combine(exportDirectory, exportFile);
      Console.WriteLine($"Export to Path: {exportPath}");
      File.WriteAllText(exportPath, contents);
    }

    public static Version AppVersion => Assembly.GetEntryAssembly().GetName().Version;

    public static DateTimeOffset UtcDateTimeOffset(this DateTime utcDateTime) => new DateTimeOffset(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc));

  }

}
