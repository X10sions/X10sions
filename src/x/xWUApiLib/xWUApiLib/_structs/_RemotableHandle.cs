﻿using System.Runtime.InteropServices;


namespace xWUApiLib {
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct _RemotableHandle {
    public int fContext;
    public __MIDL_IWinTypes_0009 u;
  }
}