using System;
using System.IO;

namespace Common.VisualStudio.ProjectVersion {
  public class AutoVersionTask : Task {
    public override bool Execute() {
      Log.LogMessage(MessageImportance.Low, "AutoVersion task started");
      try {
        var proj = XDocument.Load(ProjectPath, LoadOptions.PreserveWhitespace);

        var settings = LoadSettingsFromFile(Path.ChangeExtension(ProjectPath, ".autoVersion")) ??
                            LoadSettingsFromFile(Path.Combine(Path.GetDirectoryName(ProjectPath), ".autoVersion")) ??
                            new AutoVersionSettings {
                              BumpTypeVersionPart1 = BumpTypeVersionPart1,
                              BumpTypeVersionPart2 = BumpTypeVersionPart2,
                              BumpTypeVersionPart3 = BumpTypeVersionPart3,
                              BumpTypeVersionPart4 = BumpTypeVersionPart4
                              //LabelDigits = LabelDigits == 0 ? AutoVersionSettings.DefaultLabelDigits : LabelDigits
                            };

        Log.LogMessage(MessageImportance.Low, $"AutoVersion settings = {JObject.FromObject(settings)}");
        var autoVersion = new AutoVersion(ProjectPath, Configuration);
        if(autoVersion.TryAutoVersion(proj, settings)) {
          Log.LogMessage(MessageImportance.Low, "Saving project file");
          using(var stream = File.Create(ProjectPath)) {
            stream.Flush();
            proj.Save(stream);
          }
        }


      } catch(Exception e) {
        Log.LogErrorFromException(e);
        return false;
      }
      return true;
    }

    private AutoVersionSettings LoadSettingsFromFile(string settingsFilePath) {
      if(File.Exists(settingsFilePath)) {
        AutoVersionSettings settings = null;
        Log.LogMessage(MessageImportance.Low, $"Loading AutoVersion settings from file \"{settingsFilePath}\"");
        var settingsCollection = JsonSerializer.Create()
            .Deserialize<AutoVersionSettingsCollection>(new JsonTextReader(File.OpenText(settingsFilePath)));
        if(!string.IsNullOrEmpty(Configuration))
          settingsCollection.Configurations?.TryGetValue(Configuration, out settings);
        return settings ?? settingsCollection;
      }
      Log.LogMessage(MessageImportance.Low, $"AutoVersion settings file \"{settingsFilePath}\" not found");
      return null;
    }


    [Required] public string ProjectPath { get; set; }
    public string Configuration { get; set; }
    public BumpType BumpTypeVersionPart1 { get; set; } = BumpType.SameValue;
    public BumpType BumpTypeVersionPart2 { get; set; } = BumpType.SameValue;
    public BumpType BumpTypeVersionPart3 { get; set; } = BumpType.DateTime_ddHH;
    public BumpType BumpTypeVersionPart4 { get; set; } = BumpType.DateTime_mmss;
    public BumpType BumpTypeVersionSuffix { get; set; } = BumpType.DateTime_YYMM;

    public int NewVersionPart1 { get; set; }
    public int NewVersionPart2 { get; set; }
    public int NewVersionPart3 { get; set; }
    public int? NewVersionPart4 { get; set; }

    [Output] public string NewVersion { get; set; }
  }
}