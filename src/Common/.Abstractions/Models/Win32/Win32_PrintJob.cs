using System;

namespace Common.Models.Win32 {
  public class Win32_PrintJob {
    // https://msdn.microsoft.com/en-us/library/aa394370(v=vs.85).aspx

    public string Caption { get; set; }
    public string Description { get; set; }
    public DateTime InstallDate { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public DateTime ElapsedTime { get; set; }
    public string JobStatus { get; set; }
    public string Notify { get; set; }
    public string Owner { get; set; }
    public uint Priority { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime TimeSubmitted { get; set; }
    public DateTime UntilTime { get; set; }
    public string Color { get; set; }
    public string DataType { get; set; }
    public string Document { get; set; }
    public string DriverName { get; set; }
    public string HostPrintQueue { get; set; }
    public uint JobId { get; set; }
    public uint PagesPrinted { get; set; }
    public uint PaperLength { get; set; }
    public string PaperSize { get; set; }
    public uint PaperWidth { get; set; }
    public string Parameters { get; set; }
    public string PrintProcessor { get; set; }
    public uint Size { get; set; }
    public uint StatusMask { get; set; }
    public uint TotalPages { get; set; }

  }
}