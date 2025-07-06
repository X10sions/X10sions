namespace CIMv2;
public class Win32_PrinterConfiguration {
  // https://msdn.microsoft.com/en-us/library/aa394364(v=vs.85).aspx
  public string Caption { get; set; }
  public string Description { get; set; }
  // Public Property SettingID As String
  // Public Property BitsPerPel As UInt32
  // Public Property Collate As Boolean
  // Public Property Color As UInt32
  // Public Property Copies As UInt32
  public string DeviceName { get; set; }
  // Public Property DisplayFlags As UInt32
  // Public Property DisplayFrequency As UInt32
  // Public Property DitherType As UInt32
  // Public Property DriverVersion As UInt32
  // Public Property Duplex As Boolean
  public string FormName { get; set; }
  // Public Property HorizontalResolution As UInt32
  // Public Property ICMIntent As UInt32
  // Public Property ICMMethod As UInt32
  // Public Property LogPixels As UInt32
  // Public Property MediaType As UInt32
  public string Name { get; set; }
  // Public Property Orientation As UInt32
  // Public Property PaperLength As UInt32
  public string PaperSize { get; set; }

}