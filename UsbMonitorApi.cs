using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseTest
{
    public class UsbMonitorApi
    {
        [DllImport("Monitor.dll", EntryPoint = "MonitorLtdusbDevices", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static bool MonitorLtdusbDevices(StringBuilder _lpszVidPid, UInt16  _dwVid, UInt16 _dwPid, IntPtr _lpContext, IntPtr _lpContext2);
    }
}
