using CIMv2;
using System.Net;

namespace System.Management {

  public static class ManagementObjectSearcherExtensions {
    public static void GetNetworkPrinters() {
      foreach (var item in ListWin32_Printer(new ManagementObjectSearcher())) {
        var name = item.Name;
        var status = item.Status;
        var isDefault = item.Default;
        var isNetworkPrinter = item.Network;
        Console.WriteLine($"{name} (Status: {status}, Default: {isDefault}, Network: {isNetworkPrinter}");
      }
    }
    public static ManagementPath GetManagementPath(string nameOrIpAddress) => new ManagementPath($"\\\\{nameOrIpAddress}\\root\\cimv2");

    public static ManagementScope GetManagementScope(string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      nameOrIpAddress ??= string.Empty;
      var scope = new ManagementScope();
      var host = Dns.GetHostEntry(nameOrIpAddress);
      if (host.IsLocal()) {
        scope.Path = ManagementPath.DefaultPath;
      } else {
        if (connectionOptions == null) {
          scope.Options = new ConnectionOptions {
            Username = string.Empty,
            Password = string.Empty,
            Authority = "ntlmdomain:DOMAIN"
          };
        }
        scope.Path = GetManagementPath(nameOrIpAddress);
        scope.Connect();
      }
      return scope;
    }

    public static IEnumerable<Win32_OperatingSystem> ListWin32_OperatingSystem(this ManagementObjectSearcher @this, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      @this.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      @this.Query = new ObjectQuery($"SELECT * from {nameof(Win32_OperatingSystem)}");
      return from ManagementObject x in @this.Get() select x.MapToWin32_OperatingSystem();
    }

    public static IEnumerable<Win32_ComputerSystem> ListWin32_ComputerSystem(this ManagementObjectSearcher mos, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      mos.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      mos.Query = new ObjectQuery($"SELECT * from {nameof(Win32_ComputerSystem)}");
      return from ManagementObject x in mos.Get()
             select new Win32_ComputerSystem {
               Caption = x.GetPropertyValue<string>(nameof(Win32_ComputerSystem.Caption)),
               Domain = x.GetPropertyValue<string>(nameof(Win32_ComputerSystem.Domain)),
               InstallDate = x.GetPropertyValue<DateTime>(nameof(Win32_ComputerSystem.InstallDate)),
               Name = x.GetPropertyValue<string>(nameof(Win32_ComputerSystem.Name)),
               Status = x.GetPropertyValue<string>(nameof(Win32_ComputerSystem.Status)),
               UserName = x.GetPropertyValue<string>(nameof(Win32_ComputerSystem.UserName)),
               Workgroup = x.GetPropertyValue<string>(nameof(Win32_ComputerSystem.Workgroup))
             };
    }

    public static IEnumerable<Win32_Printer> ListWin32_Printer(this ManagementObjectSearcher @this, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      @this.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      @this.Query = new ObjectQuery($"SELECT * from {nameof(Win32_Printer)}");
      return from ManagementObject x in @this.Get()
             select new Win32_Printer {
               Default = (bool)x.GetPropertyValue(nameof(Win32_Printer.Default)),
               Name = (string)x.GetPropertyValue(nameof(Win32_Printer.Name)),
               Network = (bool)x.GetPropertyValue(nameof(Win32_Printer.Network)),
               ShareName = (string)x.GetPropertyValue(nameof(Win32_Printer.ShareName)),
               Status = (string)x.GetPropertyValue(nameof(Win32_Printer.Status))
             };
    }

    public static IEnumerable<Win32_PrinterConfiguration> ListWin32_PrinterConfiguration(this ManagementObjectSearcher @this, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      @this.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      @this.Query = new ObjectQuery($"SELECT * from {nameof(Win32_PrinterConfiguration)}");
      return from ManagementObject x in @this.Get()
             select new Win32_PrinterConfiguration {
               Caption = (string)x.GetPropertyValue(nameof(Win32_PrinterConfiguration.Caption)),
               Description = (string)x.GetPropertyValue(nameof(Win32_PrinterConfiguration.Description)),
               DeviceName = (string)x.GetPropertyValue(nameof(Win32_PrinterConfiguration.DeviceName)),
               FormName = (string)x.GetPropertyValue(nameof(Win32_PrinterConfiguration.FormName)),
               PaperSize = (string)x.GetPropertyValue(nameof(Win32_PrinterConfiguration.PaperSize))
             };
    }

    public static IEnumerable<Win32_PrintJob> ListWin32_PrintJob(this ManagementObjectSearcher mos, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      mos.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      mos.Query = new ObjectQuery($"SELECT * from {nameof(Win32_PrintJob)}");
      return from ManagementObject x in mos.Get()
             select new Win32_PrintJob {
               Color = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Color)),
               Caption = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Caption)),
               DataType = x.GetPropertyValue<string>(nameof(Win32_PrintJob.DataType)),
               Description = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Description)),
               Document = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Document)),
               DriverName = x.GetPropertyValue<string>(nameof(Win32_PrintJob.DriverName)),
               ElapsedTime = x.GetPropertyValue<DateTime>(nameof(Win32_PrintJob.ElapsedTime)),
               HostPrintQueue = x.GetPropertyValue<string>(nameof(Win32_PrintJob.HostPrintQueue)),
               InstallDate = x.GetPropertyValue<DateTime>(nameof(Win32_PrintJob.InstallDate)),
               JobId = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.JobId)),
               JobStatus = x.GetPropertyValue<string>(nameof(Win32_PrintJob.JobStatus)),
               Name = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Name)),
               Notify = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Notify)),
               Owner = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Owner)),
               PagesPrinted = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.PagesPrinted)),
               PaperLength = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.PaperLength)),
               PaperSize = x.GetPropertyValue<string>(nameof(Win32_PrintJob.PaperSize)),
               PaperWidth = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.PaperWidth)),
               Parameters = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Parameters)),
               PrintProcessor = x.GetPropertyValue<string>(nameof(Win32_PrintJob.PrintProcessor)),
               Priority = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.Priority)),
               Size = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.Size)),
               Status = x.GetPropertyValue<string>(nameof(Win32_PrintJob.Status)),
               StartTime = x.GetPropertyValue<DateTime>(nameof(Win32_PrintJob.StartTime)),
               StatusMask = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.StatusMask)),
               TimeSubmitted = x.GetPropertyValue<DateTime>(nameof(Win32_PrintJob.TimeSubmitted)),
               TotalPages = x.GetPropertyValue<uint>(nameof(Win32_PrintJob.TotalPages)),
               UntilTime = x.GetPropertyValue<DateTime>(nameof(Win32_PrintJob.UntilTime))
             };
    }

    public static IEnumerable<Win32_Process> ListWin32_Process(this ManagementObjectSearcher @this, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      @this.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      @this.Query = new ObjectQuery($"SELECT * from {nameof(Win32_Process)}");
      return from ManagementObject x in @this.Get()
             select new Win32_Process {
               Caption = x.GetPropertyValue<string>(nameof(Win32_Process.Caption)),
               CSName = x.GetPropertyValue<string>(nameof(Win32_Process.CSName)),
               Name = x.GetPropertyValue<string>(nameof(Win32_Process.Name)),
               ProcessId = (uint)x.GetPropertyValue(nameof(Win32_Process.ProcessId))
             };
    }

    public static IEnumerable<Win32_Share> ListWin32_Shares(this ManagementObjectSearcher mos, string nameOrIpAddress = null, ConnectionOptions connectionOptions = null) {
      mos.Scope = GetManagementScope(nameOrIpAddress, connectionOptions);
      mos.Query = new ObjectQuery($"SELECT * from {nameof(Win32_Share)}");
      return from ManagementObject x in mos.Get()
             select new Win32_Share {
               Caption = x.GetPropertyValue<string>(nameof(Win32_Share.Caption)),
               Description = x.GetPropertyValue<string>(nameof(Win32_Share.Description)),
               InstallDate = x.GetPropertyValue<DateTime>(nameof(Win32_Share.InstallDate)),
               Name = x.GetPropertyValue<string>(nameof(Win32_Share.Name)),
               Path = x.GetPropertyValue<string>(nameof(Win32_Share.Path)),
               Status = x.GetPropertyValue<string>(nameof(Win32_Share.Status)),
               Type = x.GetPropertyValue<uint>(nameof(Win32_Share.Type)),
             };
    }

    #region Mappings

    public static Win32_OperatingSystem MapToWin32_OperatingSystem(this ManagementObject mos) => new Win32_OperatingSystem {
      BootDevice = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.BootDevice)),
      BuildNumber = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.BuildNumber)),
      BuildType = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.BuildType)),
      Caption = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Caption)),
      CodeSet = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.CodeSet)),
      CountryCode = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.CountryCode)),
      CreationClassName = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.CreationClassName)),
      CSCreationClassName = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.CSCreationClassName)),
      CSDVersion = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.CSDVersion)),
      CSName = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.CSName)),
      CurrentTimeZone = mos.GetPropertyValue<short>(nameof(Win32_OperatingSystem.CurrentTimeZone)),
      DataExecutionPrevention_Available = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.DataExecutionPrevention_Available)),
      DataExecutionPrevention_32BitApplications = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.DataExecutionPrevention_32BitApplications)),
      DataExecutionPrevention_Drivers = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.DataExecutionPrevention_Drivers)),
      DataExecutionPrevention_SupportPolicy = mos.GetPropertyValue<byte>(nameof(Win32_OperatingSystem.DataExecutionPrevention_SupportPolicy)),
      Debug = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.Debug)),
      Description = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Description)),
      Distributed = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.Distributed)),
      EncryptionLevel = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.EncryptionLevel)),
      ForegroundApplicationBoost = mos.GetPropertyValue<byte>(nameof(Win32_OperatingSystem.ForegroundApplicationBoost)),
      FreePhysicalMemory = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.FreePhysicalMemory)),
      FreeSpaceInPagingFiles = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.FreeSpaceInPagingFiles)),
      FreeVirtualMemory = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.FreeVirtualMemory)),
      InstallDate = mos.GetPropertyValue<DateTime>(nameof(Win32_OperatingSystem.InstallDate)),
      LargeSystemCache = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.LargeSystemCache)),
      LastBootUpTime = mos.GetPropertyValue<DateTime>(nameof(Win32_OperatingSystem.LastBootUpTime)),
      LocalDateTime = mos.GetPropertyValue<DateTime>(nameof(Win32_OperatingSystem.LocalDateTime)),
      Locale = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Locale)),
      Manufacturer = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Manufacturer)),
      MaxNumberOfProcesses = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.MaxNumberOfProcesses)),
      MaxProcessMemorySize = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.MaxProcessMemorySize)),
      MUILanguages = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.MUILanguages)),
      Name = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Name)),
      NumberOfLicensedUsers = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.NumberOfLicensedUsers)),
      NumberOfProcesses = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.NumberOfProcesses)),
      NumberOfUsers = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.NumberOfUsers)),
      OperatingSystemSKU = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.OperatingSystemSKU)),
      Organization = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Organization)),
      OSArchitecture = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.OSArchitecture)),
      OSLanguage = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.OSLanguage)),
      OSProductSuite = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.OSProductSuite)),
      OSType = mos.GetPropertyValue<ushort>(nameof(Win32_OperatingSystem.OSType)),
      OtherTypeDescription = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.OtherTypeDescription)),
      PAEEnabled = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.PAEEnabled)),
      PlusProductID = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.PlusProductID)),
      PlusVersionNumber = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.PlusVersionNumber)),
      PortableOperatingSystem = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.PortableOperatingSystem)),
      Primary = mos.GetPropertyValue<bool>(nameof(Win32_OperatingSystem.Primary)),
      ProductType = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.ProductType)),
      RegisteredUser = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.RegisteredUser)),
      SerialNumber = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.SerialNumber)),
      ServicePackMajorVersion = mos.GetPropertyValue<ushort>(nameof(Win32_OperatingSystem.ServicePackMajorVersion)),
      ServicePackMinorVersion = mos.GetPropertyValue<ushort>(nameof(Win32_OperatingSystem.ServicePackMinorVersion)),
      SizeStoredInPagingFiles = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.SizeStoredInPagingFiles)),
      Status = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Status)),
      SuiteMask = mos.GetPropertyValue<uint>(nameof(Win32_OperatingSystem.SuiteMask)),
      SystemDevice = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.SystemDevice)),
      SystemDirectory = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.SystemDirectory)),
      SystemDrive = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.SystemDrive)),
      TotalSwapSpaceSize = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.TotalSwapSpaceSize)),
      TotalVirtualMemorySize = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.TotalVirtualMemorySize)),
      TotalVisibleMemorySize = mos.GetPropertyValue<ulong>(nameof(Win32_OperatingSystem.TotalVisibleMemorySize)),
      Version = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.Version)),
      WindowsDirectory = mos.GetPropertyValue<string>(nameof(Win32_OperatingSystem.WindowsDirectory)),
      QuantumLength = mos.GetPropertyValue<byte>(nameof(Win32_OperatingSystem.QuantumLength)),
      QuantumType = mos.GetPropertyValue<byte>(nameof(Win32_OperatingSystem.QuantumType))
    };

    #endregion Mappings

  }
}