namespace CIMv2;
public class Win32_Printer {
  // https://msdn.microsoft.com/en-us/library/aa394363(v=vs.85).aspx

  // Public Property Attributes As UInt32
  // Public Property Availability As UInt16
  // Public Property AvailableJobSheets As String()
  // Public Property AveragePagesPerMinute As UInt32
  // Public Property Capabilities As UInt16()
  // Public Property CapabilityDescriptions As String()
  // Public Property Caption As String
  // Public Property CharSetsSupported As String()
  // Public Property Comment As String
  // Public Property ConfigManagerErrorCode As UInt32
  // Public Property ConfigManagerUserConfig As Boolean
  // Public Property CreationClassName As String
  // Public Property CurrentCapabilities As UInt16()
  // Public Property CurrentCharSet As String
  // Public Property CurrentLanguage As UInt16
  // Public Property CurrentMimeType As String
  // Public Property CurrentNaturalLanguage As String
  // Public Property CurrentPaperType As String
  public bool Default { get; set; }
  // Public Property DefaultCapabilities As UInt16()
  // Public Property DefaultCopies As UInt32
  // Public Property DefaultLanguage As UInt16
  // Public Property DefaultMimeType As String
  // Public Property DefaultNumberUp As UInt32
  // Public Property DefaultPaperType As String
  // Public Property DefaultPriority As UInt32
  // Public Property Description As String
  // Public Property DetectedErrorState As UInt16
  // Public Property DeviceID As String
  // Public Property Direct As Boolean
  // Public Property DoCompleteFirst As Boolean
  // Public Property DriverName As String
  // Public Property EnableBIDI As Boolean
  // Public Property EnableDevQueryPrint As Boolean
  // Public Property ErrorCleared As Boolean
  // Public Property ErrorDescription As String
  // Public Property ErrorInformation As String()
  // Public Property ExtendedDetectedErrorState As UInt16
  // Public Property ExtendedPrinterStatus As UInt16
  // Public Property Hidden As Boolean
  // Public Property HorizontalResolution As UInt32
  // Public Property InstallDate As DateTime
  // Public Property JobCountSinceLastReset As UInt32
  // Public Property KeepPrintedJobs As Boolean
  // Public Property LanguagesSupported As UInt16()
  // Public Property LastErrorCode As UInt32
  // Public Property Local As Boolean
  // Public Property Location As String
  // Public Property MarkingTechnology As UInt16
  // Public Property MaxCopies As UInt32
  // Public Property MaxNumberUp As UInt32
  // Public Property MaxSizeSupported As UInt32
  // Public Property MimeTypesSupported As String()
  public string Name { get; set; }
  // Public Property NaturalLanguagesSupported As String()
  public bool Network { get; set; }
  // Public Property PaperSizesSupported As UInt16()
  // Public Property PaperTypesAvailable As String()
  // Public Property Parameters As String
  // Public Property PNPDeviceID As String
  // Public Property PortName As String
  // Public Property PowerManagementCapabilities As UInt16()
  // Public Property PowerManagementSupported As Boolean
  // Public Property PrinterPaperNames As String()
  // Public Property PrinterState As UInt32
  // Public Property PrinterStatus As UInt16
  // Public Property PrintJobDataType As String
  // Public Property PrintProcessor As String
  // Public Property Priority As UInt32
  // Public Property Published As Boolean
  // Public Property Queued As Boolean
  // Public Property RawOnly As Boolean
  // Public Property SeparatorFile As String
  // Public Property ServerName As String
  // Public Property [Shared] As Boolean
  public string ShareName { get; set; }
  // Public Property SpoolEnabled As Boolean
  // Public Property StartTime As DateTime
  public string Status { get; set; }
}