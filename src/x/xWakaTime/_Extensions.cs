using System;

namespace Kbg.NppPluginNET.PluginInfrastructure {
  public static partial class Win32 {

    public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, byte[] lParam)
      => SendMessage(hWnd, Msg, wParam, BitConverter.ToString(lParam));

    public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, byte[] wParam, byte[] lParam)
      => SendMessage(hWnd, Msg, BitConverter.ToInt32(wParam, 0), BitConverter.ToString(lParam));

    public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, byte[] wParam, int lParam)
      => SendMessage(hWnd, Msg, BitConverter.ToInt32(wParam, 0), lParam);

    public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, char[] lParam)
      => SendMessage(hWnd, Msg, wParam, new string(lParam));

  }
}
