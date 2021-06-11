using System.Collections.Generic;

namespace Common.VisualStudio.ProjectVersion {
  public class AutoVersionSettingsCollection : AutoVersionSettings {
    public Dictionary<string, AutoVersionSettings> Configurations { get; set; }
  }
}