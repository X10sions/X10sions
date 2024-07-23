﻿namespace RCommon;

public class SystemTimeOptions : ISystemTimeOptions {
  /// <summary>
  /// Default: <see cref="DateTimeKind.Unspecified"/>
  /// </summary>
  public DateTimeKind Kind { get; set; }

  public SystemTimeOptions() {
    Kind = DateTimeKind.Unspecified;
  }
}