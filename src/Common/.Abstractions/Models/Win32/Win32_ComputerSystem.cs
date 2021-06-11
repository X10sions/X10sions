using System;

namespace Common.Models.Win32 {
  public class Win32_ComputerSystem {
    // https://msdn.microsoft.com/en-us/library/aa394102(v=vs.85).aspx
    // Public Property AdminPasswordStatus As uint16
    // Public Property AutomaticManagedPagefile As Boolean
    // Public Property AutomaticResetBootOption As Boolean
    // Public Property AutomaticResetCapability As Boolean
    // Public Property BootOptionOnLimit As uint16
    // Public Property BootOptionOnWatchDog As uint16
    // Public Property BootROMSupported As Boolean
    // Public Property BootupState As String
    // Public Property BootStatus As uint16()
    public string Caption { get; set; }
    // Public Property ChassisBootupState As uint16
    // Public Property ChassisSKUNumber As String
    // Public Property CreationClassName As String
    // Public Property CurrentTimeZone As Int16
    // Public Property DaylightInEffect As Boolean
    // Public Property Description As String
    // Public Property DNSHostName As String
    public string Domain { get; set; }
    // Public Property DomainRole As uint16
    // Public Property EnableDaylightSavingsTime As Boolean
    // Public Property FrontPanelResetStatus As uint16
    // Public Property HypervisorPresent As Boolean
    // Public Property InfraredSupported As Boolean
    // Public Property InitialLoadInfo As String()
    public DateTime InstallDate { get; set; }
    // Public Property KeyboardPasswordStatus As uint16
    // Public Property LastLoadInfo As String
    // Public Property Manufacturer As String
    // Public Property Model As String
    public string Name { get; set; }
    // Public Property NameFormat As String
    // Public Property NetworkServerModeEnabled As Boolean
    // Public Property NumberOfLogicalProcessors As uint32
    // Public Property NumberOfProcessors As UInt32
    // Public Property OEMLogoBitmap As Byte() ' uint8()
    // Public Property OEMStringArray As String()
    // Public Property PartOfDomain As Boolean
    // Public Property PauseAfterReset As Int64
    // Public Property PCSystemType As uint16
    // Public Property PCSystemTypeEx As UInt16
    // Public Property PowerManagementCapabilities As UInt16()
    // Public Property PowerManagementSupported As Boolean
    // Public Property PowerOnPasswordStatus As uint16
    // Public Property PowerState As uint16
    // Public Property PowerSupplyState As uint16
    // Public Property PrimaryOwnerContact As String
    // Public Property PrimaryOwnerName As String
    // Public Property ResetCapability As UInt16
    // Public Property ResetCount As Int16
    // Public Property ResetLimit As Int16
    // Public Property Roles As String()
    public string Status { get; set; }
    // Public Property SupportContactDescription As String()
    // Public Property SystemFamily As String
    // Public Property SystemSKUNumber As String
    // Public Property SystemStartupDelay As UInt16
    // Public Property SystemStartupOptions As String()
    // Public Property SystemStartupSetting As Byte 'uint8
    // Public Property SystemType As String
    // Public Property ThermalState As uint16
    // Public Property TotalPhysicalMemory As uint64
    public string UserName { get; set; }
    // Public Property WakeUpType As UInt16
    public string Workgroup { get; set; }
  }
}
