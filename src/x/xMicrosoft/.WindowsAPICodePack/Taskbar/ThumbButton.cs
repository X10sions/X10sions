﻿using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Taskbar {
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  public struct ThumbButton {
    /// <summary>
    /// WPARAM value for a THUMBBUTTON being clicked.
    /// </summary>
    internal const int Clicked = 0x1800;

    [MarshalAs(UnmanagedType.U4)]
    internal ThumbButtonMask Mask;
    internal uint Id;
    internal uint Bitmap;
    internal IntPtr Icon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    internal string Tip;
    [MarshalAs(UnmanagedType.U4)]
    internal ThumbButtonOptions Flags;
  }
}
