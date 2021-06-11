using System;

namespace Common.VisualStudio.ProjectVersion {
  public class CsProjectVersion {

    public CsProjectVersion() { }

    public CsProjectVersion(string part1, string part2, string part3, string part4) {
      SetVersionParts(part1, part2, part3, part4);
    }

    public int VersionPart1 { get; set; }
    public int VersionPart2 { get; set; }
    public int VersionPart3 { get; set; }
    public int? VersionPart4 { get; set; }

    public string VersionPrefix => $"{VersionPart1}.{VersionPart2}.{VersionPart3}" + (VersionPart4.HasValue ? $".{VersionPart4}" : string.Empty);
    public string VersionSuffix { get; set; } = string.Empty;
    public string Version => VersionPrefix + (string.IsNullOrWhiteSpace(VersionSuffix) ? string.Empty : "-" + VersionSuffix);

    public string AssemblyVersion => VersionPrefix;
    public string FileVersion => AssemblyVersion;
    public string InformationalVersion => Version;
    public string PackageVersion => Version;

    //public string Version => $"{VersionPrefix}-{VersionSuffix}";


    public void SetVersionParts(string part1, string part2, string part3, string part4) {
      VersionPart1 = string.IsNullOrWhiteSpace(part1) ? part1.As(VersionPart1) : VersionPart1;
      VersionPart2 = string.IsNullOrWhiteSpace(part2) ? part1.As(VersionPart2) : VersionPart2;
      VersionPart3 = string.IsNullOrWhiteSpace(part3) ? part1.As(VersionPart3) : VersionPart3;
      VersionPart4 = string.IsNullOrWhiteSpace(part4) ? part1.As(VersionPart4) : VersionPart4;
    }

    public void BumpVersionPart1(int newValue = 1) => VersionPart1 += newValue;
    public void BumpVersionPart2(int newValue = 1) => VersionPart2 += newValue;
    public void BumpVersionPart3(int newValue = 1) => VersionPart3 += newValue;
    public void BumpVersionPart3(DateTime versionDate) => VersionPart3 = versionDate.YYMM();
    public void BumpVersionPart4(DateTime versionDate) => VersionPart4 = versionDate.ddHH();
    public void BumpVersionPart4(int newValue = 1) => VersionPart4 += newValue;

    public void BumpPrefix(DateTime versionDate) {
      BumpVersionPart3(versionDate);
      BumpVersionPart4(versionDate);
    }

    public void BumpVersionSuffix(DateTime versionDate, string configuration) => VersionSuffix = !string.Equals(configuration, "release", StringComparison.OrdinalIgnoreCase) ? $"dev{versionDate.mmss()}" : string.Empty;

    public void BumpPrefixAndSuffix(DateTime versionDate, string configuration) {
      BumpPrefix(versionDate);
      BumpVersionSuffix(versionDate, configuration);
    }


  }
}
