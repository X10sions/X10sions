using System;

namespace Common.Models.Win32 {
  public class Win32_Share {
    // https://msdn.microsoft.com/en-us/library/aa394435(v=vs.85).aspx

    public string Caption { get; set; }
    public string Description { get; set; }
    public DateTime InstallDate { get; set; }
    public string Status { get; set; }
    // Public Property AccessMask As uint32
    // Public Property AllowMaximum As Boolean
    // Public Property MaximumAllowed As uint32
    public string Name { get; set; }
    public string Path { get; set; }
    public uint Type { get; set; }

  }
}