using System;

namespace Common.Attributes {
  [AttributeUsage(AttributeTargets.Field)]
  public class PrinterInfoAttribute : Attribute {
    public string ComputerName { get; set; }
    public string Name { get; set; }

    public PrinterInfoAttribute(string computerName, string name) {
      ComputerName = computerName;
      Name = name;
    }

    public string PhysicalPath() => $@"\\{ComputerName}\{Name}\";
  }
}
