namespace CIMv2;
public class Win32_OperatingSystem {
  // https://msdn.microsoft.com/en-us/library/aa394239(v=vs.85).aspx
  public string BootDevice { get; set; } = string.Empty;
  public string BuildNumber { get; set; } = string.Empty;
  public string BuildType { get; set; } = string.Empty; 
  public string Caption { get; set; } = string.Empty; 
  public string CodeSet { get; set; } = string.Empty;
  public string CountryCode { get; set; } = string.Empty;
  public string CreationClassName { get; set; } = string.Empty;
  public string CSCreationClassName { get; set; } = string.Empty;
  public string CSDVersion { get; set; } = string.Empty;
  public string CSName { get; set; } = string.Empty; // ComputerName
  public short CurrentTimeZone { get; set; }
  public bool DataExecutionPrevention_Available { get; set; }
  public bool DataExecutionPrevention_32BitApplications { get; set; }
  public bool DataExecutionPrevention_Drivers { get; set; } 
  public byte DataExecutionPrevention_SupportPolicy { get; set; } 
  public bool Debug { get; set; }
  public string Description { get; set; } = string.Empty;
  public bool Distributed { get; set; }
  public uint EncryptionLevel { get; set; }
  public byte ForegroundApplicationBoost { get; set; } = 2;
  public ulong FreePhysicalMemory { get; set; }
  public ulong FreeSpaceInPagingFiles { get; set; }
  public ulong FreeVirtualMemory { get; set; }
  public DateTime InstallDate { get; set; }
  public uint LargeSystemCache { get; set; }
  public DateTime LastBootUpTime { get; set; }
  public DateTime LocalDateTime { get; set; }
  public string Locale { get; set; } = string.Empty;
  public string Manufacturer { get; set; } = string.Empty;
  public uint MaxNumberOfProcesses { get; set; }
  public ulong MaxProcessMemorySize { get; set; }
  public string MUILanguages { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public uint NumberOfLicensedUsers { get; set; }
  public uint NumberOfProcesses { get; set; }
  public uint NumberOfUsers { get; set; }
  public uint OperatingSystemSKU { get; set; }
  public string Organization { get; set; } = string.Empty;
  public string OSArchitecture { get; set; } = string.Empty;
  public uint OSLanguage { get; set; }
  public uint OSProductSuite { get; set; }
  public ushort OSType { get; set; }
  public string OtherTypeDescription { get; set; } = string.Empty;
  public bool PAEEnabled { get; set; }
  public string PlusProductID { get; set; } = string.Empty;
  public string PlusVersionNumber { get; set; } = string.Empty;
  public bool PortableOperatingSystem { get; set; }
  public bool Primary { get; set; }
  public uint ProductType { get; set; }
  public string RegisteredUser { get; set; } = string.Empty;
  public string SerialNumber { get; set; } = string.Empty;
  public ushort ServicePackMajorVersion { get; set; }
  public ushort ServicePackMinorVersion { get; set; }
  public ulong SizeStoredInPagingFiles { get; set; }
  public string Status { get; set; } = string.Empty;
  public uint SuiteMask { get; set; }
  public string SystemDevice { get; set; } = string.Empty;
  public string SystemDirectory { get; set; } = string.Empty;
  public string SystemDrive { get; set; } = string.Empty;
  public ulong TotalSwapSpaceSize { get; set; }
  public ulong TotalVirtualMemorySize { get; set; }
  public ulong TotalVisibleMemorySize { get; set; }
  public string Version { get; set; } = string.Empty;
  public string WindowsDirectory { get; set; } = string.Empty;
  public byte QuantumLength { get; set; }
  public byte QuantumType { get; set; }

}
