using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Common.InstalledSoftwareInfo {
  public class InstalledSoftwareInfo {
    public InstalledSoftwareInfo(RegistryKey registryKey) {
      Console.WriteLine("==== " + registryKey.Name);
      GetAppPaths(registryKey.OpenSubKey(AppPathsRegKey));
    }

    public const string AppPathsRegKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths";
    public List<SoftwareInfo> SoftwareInfoList = new List<SoftwareInfo>();


    public JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
      WriteIndented = true
    };

    public string JsonString => JsonSerializer.Serialize(SoftwareInfoList, JsonSerializerOptions);

    public void GetAppPaths(RegistryKey regKey) {
      if (regKey != null) {
        foreach (var v in regKey.GetSubKeyNames()) {
          Console.Write(v + ": ");
          var appPathKey = regKey.OpenSubKey(v, false);
          if (appPathKey != null) {
            try {
              var appPath = appPathKey.GetValue(string.Empty, null).ToString();
              if (appPath != null) {
                Console.Write(appPath);
                SoftwareInfoList.Add(new SoftwareInfo(appPath));
              }
            } catch (Exception ex) {
              Console.WriteLine("============= ERROR =====================");
              Console.WriteLine(ex.Message);
              Console.WriteLine();
            }
            appPathKey.Close();
            Console.WriteLine();
          }
        }
      }
    }

  }
}