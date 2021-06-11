using System;

namespace Common.Attributes {

  [AttributeUsage(AttributeTargets.Field)]
  public class ComputerInfoAttribute : Attribute {
    public string IpAddress { get; set; }
    public string Name { get; set; }

    public ComputerInfoAttribute(string ipAddress, string name) {
      IpAddress = ipAddress;
      Name = name;
    }

    public string PhysicalPath() => $@"\\{IpAddress ?? Name}\";

  }
}