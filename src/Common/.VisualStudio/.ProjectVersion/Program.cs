//using Buildalyzer;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Common.VisualStudio.ProjectVersion {
  static class Program {

    public const string Version = nameof(Version);
    public const string VersionSuffix = nameof(VersionSuffix);
    public const string VersionPrefix = nameof(VersionPrefix);
    public const string VersionPart1 = nameof(VersionPart1);
    public const string VersionPart2 = nameof(VersionPart2);
    public const string VersionPart3 = nameof(VersionPart3);
    public const string VersionPart4 = nameof(VersionPart4);

    static void Main(string[] args) {
      Console.WriteLine($"{ nameof(ProjectVersion)} (v{AppVersion})");
      if (args.Length > 0) {
        var projectPath = args[0];
        var configuration = args[1] ?? "Debug";
        var file = new FileInfo(projectPath);
        Console.WriteLine(file.Extension);
        if (file.Exists && string.Equals(file.Extension, ".csproj", StringComparison.OrdinalIgnoreCase)) {
          //SetProjectVersionInfo3(projectPath, configuration);
          SetProjectVersionInfo2(projectPath, configuration);
          //SetProjectVersionInfo(projectPath);
        }
      }
    }

    public static void SetProjectVersionInfo2(string projectPath, string configuration) {
      Console.WriteLine("AutoVersion task started");
      var xmlProject = XDocument.Load(projectPath, LoadOptions.PreserveWhitespace);
      var autoVersion = new AutoVersion(projectPath, configuration);
      if (autoVersion.TryAutoVersion(xmlProject)) {
        Console.WriteLine("Saving project file");
        using (var stream = File.Create(projectPath)) {
          stream.Flush();
          xmlProject.Save(stream);
        }
      }
    }

    //public static void SetProjectVersionInfo3(string projectPath, string configuration) {
    //  var autoVersion = new AutoVersionTask() {
    //    ProjectPath = projectPath,
    //    Configuration = configuration
    //  };
    //  autoVersion.Execute();
    //}

    //public static void SetProjectVersionInfo_Buildalyzer(string projectPath) {
    //  var manager = new AnalyzerManager();
    //  var projectAnalyzer = manager.GetProject(projectPath);
    //  var analyzerResults = projectAnalyzer.Build().Results;
    //  var result = analyzerResults.First();
    //}

    public static void SetProjectVersionInfo(string projectPath) {
      var project = _Extensions.GetProject(projectPath);
      var configuration = project.GetProperty("$(Configuration)").EvaluatedValue;
      var newVersion = new CsProjectVersion();
      newVersion.BumpPrefixAndSuffix(DateTime.UtcNow, configuration);

      newVersion.VersionPart1 = project.GetProperty(nameof(newVersion.VersionPart1)).EvaluatedValue.As(newVersion.VersionPart1);
      newVersion.VersionPart2 = project.GetProperty(nameof(newVersion.VersionPart2)).EvaluatedValue.As(newVersion.VersionPart2);
      newVersion.VersionPart3 = project.GetProperty(nameof(newVersion.VersionPart3)).EvaluatedValue.As(newVersion.VersionPart3);
      newVersion.VersionPart4 = project.GetProperty(nameof(newVersion.VersionPart4)).EvaluatedValue.As(newVersion.VersionPart4);
      project.SetProperty(VersionPrefix, newVersion.VersionSuffix);
      project.SetProperty(VersionSuffix, newVersion.VersionSuffix);
      // Select Debug configuration
      var debugPropertyGroup = project.GetPropertyGroupDebug();
      debugPropertyGroup.SetProperty(VersionSuffix, newVersion.VersionSuffix);
      // Select Release configuration
      var releasePropertyGroup = project.GetPropertyGroupRelease();
      releasePropertyGroup.SetProperty(VersionSuffix, string.Empty);
      //Save
      project.Save();
    }

    public static Version AppVersion => Assembly.GetEntryAssembly().GetName().Version;
    public static DateTimeOffset UtcDateTimeOffset(this DateTime utcDateTime) => new DateTimeOffset(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc));


  }
}
