using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System;
using System.ComponentModel;
using System.Linq;

namespace Common.VisualStudio.ProjectVersion {
  public static class _Extensions {
    public static Project GetProject(this string projectPath) => new ProjectCollection().LoadProject(projectPath);

    public static ProjectPropertyGroupElement GetPropertyGroup(this Project project, string configuration, string platform = "AnyCPU") => project.Xml.PropertyGroups.First(e => e.Condition == $" '$(Configuration)|$(Platform)' == '{configuration}|{platform}' ");
    public static ProjectPropertyGroupElement GetPropertyGroupDebug(this Project project, string platform = "AnyCPU") => project.GetPropertyGroup("Debug", platform);
    public static ProjectPropertyGroupElement GetPropertyGroupRelease(this Project project, string platform = "AnyCPU") => project.GetPropertyGroup("Release", platform);

    public static int YYMM(this DateTime d) => ((d.Year - 2000) * 100) + d.Month;
    public static int ddHH(this DateTime d) => (d.Day * 100) + d.Hour;
    public static int mmss(this DateTime d) => (d.Minute * 100) + d.Second;

    public static T As<T>(this object value, T defaultValue = default) {
      if (value is T) {
        return (T)value;
      }
      try {
        //return (T)Convert.ChangeType(value, typeof(T));
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter != null) {
          return (T)converter.ConvertFromString(value.ToString());
        }
        return defaultValue;
      } catch (NotSupportedException) {
        return defaultValue;
      }
    }

  }
}