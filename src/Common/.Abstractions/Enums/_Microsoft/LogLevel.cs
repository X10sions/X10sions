using System;
namespace Common.Enums {
  [Flags]
  public enum LogLevel {
    All = 0x0,
    Trace = 0x1,
    Debug = 0x2,
    Information = 0x4,
    Warning = 0x8,
    Error = 0x10,
    CriticalOrFatal = 0x20,
    Off = 0x40
  }
}
