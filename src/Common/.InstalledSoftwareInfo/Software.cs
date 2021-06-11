using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Common.InstalledSoftwareInfo {

  public enum Software {
    // Browsers
    Opera,
    Vivaldi,
    //Office
    Word,
    Excel,
    PowerPoint,
    Outlook
  }

  public static class SoftwareExtensions {

    public static string GetComponentKey(this Software component) {
      switch (component) {
        case Software.Word:
          return "winword.exe";
        case Software.Excel:
          return "excel.exe";
        case Software.PowerPoint:
          return "powerpnt.exe";
        case Software.Outlook:
          return "outlook.exe";
        default:
          throw new NotImplementedException();
      }
    }

    public static FileVersionInfo GetFileVersionInfo(this Software component) => component.GetComponentPath().GetFileVersionInfo();

    public static string GetComponentPath(this Software component) {
      const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths";
      var toReturn = string.Empty;
      var _key = component.GetComponentKey();
      //looks inside CURRENT_USER:
      var _mainKey = Registry.CurrentUser;
      try {
        _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
        if (_mainKey != null) {
          toReturn = _mainKey.GetValue(string.Empty).ToString();
        }
      } catch { }
      //if not found, looks inside LOCAL_MACHINE:
      _mainKey = Registry.LocalMachine;
      if (string.IsNullOrEmpty(toReturn)) {
        try {
          _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
          if (_mainKey != null) {
            toReturn = _mainKey.GetValue(string.Empty).ToString();
          }
        } catch { }
      }
      //closing the handle:
      if (_mainKey != null)
        _mainKey.Close();
      return toReturn;
    }


  }

}
