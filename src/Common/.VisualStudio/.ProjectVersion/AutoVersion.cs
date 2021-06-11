using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Common.VisualStudio.ProjectVersion {
  public class AutoVersion {

    public AutoVersion(string projectPath, string configuration) {
      ProjectPath = projectPath;
      Configuration = configuration;
    }

    [Required] public string ProjectPath { get; set; }
    public string Configuration { get; set; }
    [Output] public string NewVersion { get; set; }

    public DateTime NewVersionDate { get; set; } = DateTime.UtcNow;

    public bool TryAutoVersion(XDocument xmlProject) {
      var defaultNamespace = xmlProject.Root.GetDefaultNamespace();
      var defaultNamespacePrefix = "ns";
      var xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      xmlNamespaceManager.AddNamespace(defaultNamespacePrefix, defaultNamespace.NamespaceName);
      XElement GetPropertyGroupElement(string xElementName) {
        return xmlProject.Root.XPathSelectElement($"{defaultNamespacePrefix}:PropertyGroup/{defaultNamespacePrefix}:{xElementName}", xmlNamespaceManager);
      }

      var oldVersion = new CsProjectVersion(
        GetPropertyGroupElement(Program.VersionPart1)?.Value ?? string.Empty,
        GetPropertyGroupElement(Program.VersionPart2)?.Value ?? string.Empty,
        GetPropertyGroupElement(Program.VersionPart3)?.Value ?? string.Empty,
        GetPropertyGroupElement(Program.VersionPart4)?.Value ?? string.Empty
        );

      var tagName = "Version";

      Console.WriteLine($"Old {tagName} is {oldVersion.Version}");

      //Log.LogMessage(MessageImportance.Low, $"Old {Program.VersionSuffix} is {elementVersionSuffix.Value}");

      //int? GetNewVersion(BumpType bumpType, int? oldValue, int? newValue = null) {
      //  switch (bumpType) {
      //    case BumpType.Bump: return oldValue + 1;
      //    case BumpType.CustomValue: return newValue;
      //    case BumpType.DateTime_ddHH: return NewVersionDate.ddHH();
      //    case BumpType.DateTime_mmss: return NewVersionDate.mmss();
      //    case BumpType.DateTime_YYMM: return NewVersionDate.YYMM();
      //    case BumpType.Reset: return 0;
      //  }
      //  return oldValue;
      //}

      var newVersion = oldVersion;
      newVersion.BumpVersionPart3(NewVersionDate);
      newVersion.BumpVersionPart4(NewVersionDate);
      newVersion.BumpVersionSuffix(NewVersionDate, Configuration);
      //newVersion.VersionPart1 = GetNewVersion(settings.BumpTypeVersionPart1, newVersion.VersionPart1, settings.NewVersionPart1).Value;
      //newVersion.VersionPart2 = GetNewVersion(settings.BumpTypeVersionPart2, newVersion.VersionPart2, settings.NewVersionPart2).Value;
      //newVersion.VersionPart3 = GetNewVersion(settings.BumpTypeVersionPart3, newVersion.VersionPart3, settings.NewVersionPart3).Value;
      //newVersion.VersionPart4 = GetNewVersion(settings.BumpTypeVersionPart4, newVersion.VersionPart4, settings.NewVersionPart4);

      Console.WriteLine($"New {tagName} is {newVersion.Version}");

      if (newVersion.Version != oldVersion.Version) {
        Console.WriteLine($"Changing {tagName} to {newVersion.Version}...");
        GetPropertyGroupElement(Program.VersionPrefix).Value = newVersion.VersionPrefix;
        GetPropertyGroupElement(Program.VersionSuffix).Value = newVersion.VersionSuffix;
        GetRequiredPropertyInfo("New" + tagName).SetValue(this, newVersion.Version);
        return true;
      }
      return false;
    }

    private PropertyInfo GetRequiredPropertyInfo(string propertyName) => GetType().GetProperty(propertyName) ?? throw new Exception($"Property {propertyName} is missing from type {GetType().Name}");

  }
}