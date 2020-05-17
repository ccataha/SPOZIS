using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Security;
namespace WindowsFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
  

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

    public class MemoryStatus
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatus lpBuffer);
        private uint dwLength;
        public uint MemoryLoad;
        public ulong TotalPhys;
        public ulong AvailPhys;
        public ulong TotalPageFile;
        public ulong AvailPageFile;
        public ulong TotalVirtual;
        public ulong AvailVirtual;
        public ulong AvailExtendedVirtual;
        private static volatile MemoryStatus singleton;
        private static readonly object syncroot = new object();
        public static MemoryStatus CreateInstance()
        {
            if (singleton == null)
                lock (syncroot)
            if (singleton == null)
                singleton = new MemoryStatus();
         return singleton;
        }

        [SecurityCritical]

        private MemoryStatus()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatus));
            GlobalMemoryStatusEx(this);
        }
    }
}