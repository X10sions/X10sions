namespace CIMv2;
public class Win32_Share {
  // https://msdn.microsoft.com/en-us/library/aa394435(v=vs.85).aspx

  public string Caption { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTime InstallDate { get; set; }
  public string Status { get; set; } = string.Empty;
  // Public Property AccessMask As uint32
  // Public Property AllowMaximum As Boolean
  // Public Property MaximumAllowed As uint32
  public string Name { get; set; } = string.Empty;
  public string Path { get; set; } = string.Empty;
  public uint Type { get; set; }

}
