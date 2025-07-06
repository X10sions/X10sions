namespace CIMv2;
public class Win32_Process {
  // https://msdn.microsoft.com/en-us/library/aa394372(v=vs.85).aspx

  // Public Property CreationClassName As String
  public string Caption { get; set; }
  // Public Property CommandLine As String
  // Public Property CreationDate As DateTime
  // Public Property CSCreationClassName As String
  public string CSName { get; set; }
  // Public Property Description As String
  // Public Property ExecutablePath As String
  // Public Property ExecutionState As UInt16
  // Public Property Handle As String
  // Public Property HandleCount As UInt32
  // Public Property InstallDate As DateTime
  // Public Property KernelModeTime As UInt64
  // Public Property MaximumWorkingSetSize As UInt32
  // Public Property MinimumWorkingSetSize As UInt32
  public string Name { get; set; }
  // Public Property OSCreationClassName As String
  // Public Property OSName As String
  // Public Property OtherOperationCount As UInt64
  // Public Property OtherTransferCount As UInt64
  // Public Property PageFaults As UInt32
  // Public Property PageFileUsage As UInt32
  // Public Property ParentProcessId As UInt32
  // Public Property PeakPageFileUsage As UInt32
  // Public Property PeakVirtualSize As UInt64
  // Public Property PeakWorkingSetSize As UInt32
  // Public Property Priority As UInt32?
  // Public Property PrivatePageCount As UInt64
  public uint ProcessId { get; set; }

}