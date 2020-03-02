using System;
using System.Runtime.InteropServices;
using System.Text;


namespace SPOZIS1lab
{
    class Program
    {
        [DllImport("user32")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(SystemParameters uiAction, int uiParam, out IntPtr pvParam, int fWinIni);

        [DllImport("user32")]
        public static extern int GetSysColor(int nIndex);

        [StructLayout(LayoutKind.Sequential)]
        public class SystemTime
        {
            public ushort year;
            public ushort month;
            public ushort weekday;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
            public ushort millisecond;
        }

        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime([In, Out] SystemTime st);


        public enum SystemMetric
        {
            SM_CMOUSEBUTTONS = 43,
            SM_MOUSEWHEELPRESENT = 75,

            SM_CXFULLSCREEN =16,
            SM_SLOWMACHINE=73,
            SM_STARTER=88,
            SM_CLEANBOOT=67

        }
        public enum SystemParameters
        {

            SPI_GETBEEP = 1,
            SPI_GETDRAGFULLWINDOWS = 38

        }

       

        static void Main()
        {

            Environment.SpecialFolder win = Environment.SpecialFolder.Windows;
            string pathToWinFolder = Environment.GetFolderPath(win);
            string strPath = System.Environment.GetEnvironmentVariable("TEMP");
            string tempPath = System.IO.Path.GetTempPath();
           
            //1 Имя пк имя юзера
            Console.WriteLine("UserName: {0}", Environment.UserName);
            Console.WriteLine("Machine: {0}", Environment.MachineName);
            //2 Системные каталоги
            Console.WriteLine("System Directory: {0}", Environment.SystemDirectory);
            Console.WriteLine("Windows Directory: {0}", pathToWinFolder);
            Console.WriteLine("Temp directory: {0}", tempPath);
            Console.WriteLine("Environment variable Directory: {0}", strPath);
            
            //3 Версия ОС
            Console.WriteLine("OS Version: {0}", Environment.OSVersion);
            //4 Системные метрики
            Console.WriteLine("Number of monitors:{0}", GetSystemMetrics(SystemMetric.SM_MOUSEWHEELPRESENT));
            Console.WriteLine("Number of mouse buttons:{0}", GetSystemMetrics(SystemMetric.SM_CMOUSEBUTTONS));
            //5 Системные параметры
            Console.WriteLine("Beeper status: {0}", SystemParametersInfo(SystemParameters.SPI_GETBEEP, 0,  out IntPtr Getter, 0));
            Console.WriteLine("DragFullWindows status: {0}", SystemParametersInfo(SystemParameters.SPI_GETDRAGFULLWINDOWS, 0, out IntPtr Getter2 , 0));
            //6 Системные цвета
            Console.WriteLine("Scrollbar Color in decimal format: {0}", GetSysColor(0));
            //7 Системное время
            SystemTime st = new SystemTime();
            GetLocalTime(st);
            Console.WriteLine("System time now: {0}:{1}:{2}.{3}", st.hour, st.minute, st.second, st.millisecond);
            Console.WriteLine("-----------------------Дополнительные функции-----------------------");
            Console.WriteLine("Screen width: {0}px", GetSystemMetrics(SystemMetric.SM_CXFULLSCREEN));
            Console.WriteLine("Slow machine par: {0}", GetSystemMetrics(SystemMetric.SM_SLOWMACHINE));
            Console.WriteLine("Is OS ver. is 'starter edition': {0}", GetSystemMetrics(SystemMetric.SM_STARTER));
            Console.WriteLine("Type of BOOT (0 - Normal, 1 - Secure Mod, 2 - Secure Mod with Network: {0}", GetSystemMetrics(SystemMetric.SM_CLEANBOOT));
            Console.ReadKey();
        }
    }
}
