using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace App2
{

    static class api
    {
        //Класс, в котором подключаем dll, ради использования возможностей WinApi
        
        // ID процесса
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId); 

        //путь к файлу 
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetModuleFileName(IntPtr hModule, StringBuilder lpFileName, int nSize); 

        // дескриптор модуля
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandleA(string lpModuleName); 

        //объект процесса
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId); 


        //идентификатор процесса вызывающего процесса.
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetCurrentProcessId(); 


        //псевдо-дескриптор для текущего процесса.
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetCurrentProcess(); 


        //дубликат дескриптора
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(uint hSourceProcessHandle, uint hSourceHandle, uint hTargetProcessHandle, out uint lpTargetHandle,
            uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

        //закрытие дескриптора
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [Flags]
        public enum DuplicateOptions : uint
        {
            DUPLICATE_CLOSE_SOURCE = (0x00000001),// Closes the source handle. This occurs regardless of any error status returned.
            DUPLICATE_SAME_ACCESS = (0x00000002), //Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.
        }
    }




    class Program
    {
        static string longFileName;
        static string shortFileName;
        static IntPtr processHandle;
        
        static void init()
        {
            longFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName; //
            shortFileName = System.IO.Path.GetFileName(longFileName);
            processHandle = api.GetModuleHandleA(longFileName);
        }

        static void Main(string[] args)
        {
            while (true)
            {

                // Главное меню

                Console.WriteLine(
                    "1: Show all\n" +
                    "2: By name\n" +
                    "3: By full name\n" +
                    "4: By handle\n" +
                    "5: Properties of the current process\n" +
                    "6: Enumeration of all processes, threads, modules and their properties\n"
                    );

                //Выбор

                int pressedKey = Int32.Parse(Console.ReadLine()); 

                switch (pressedKey)
                {

                    //Текущий
                    case 1: 
                        init();
                        Console.WriteLine(
                            $"Name: {shortFileName}\n" +
                            $"Full name: {longFileName}\n" +
                            $"Handle: {processHandle}\n");

                        break;

                    //По короткому имени
                    case 2: 
                        Console.Write("Type a short name: ");
                        shortFileName = Console.ReadLine();

                        processHandle = (api.GetModuleHandleA(shortFileName) == (IntPtr)0) ? (IntPtr)1 : api.GetModuleHandleA(shortFileName);

                        StringBuilder longFileNameSB1 = new StringBuilder(256);
                        api.GetModuleFileName(processHandle, longFileNameSB1, longFileNameSB1.Capacity);

                        Console.WriteLine(
                            $"Full name: {longFileNameSB1}\n" +
                            $"Handle: {processHandle}\n"
                            );
                        break;

                    //По пути к файлу
                    case 3: 
                        Console.Write("Type a path to file: ");
                        longFileName = Console.ReadLine();

                        shortFileName = System.IO.Path.GetFileName(longFileName);
                        processHandle = api.GetModuleHandleA(shortFileName);

                        Console.WriteLine(
                            $"Name: {shortFileName}\n" +
                            $"Handle: {processHandle}\n"
                            );
                        break;

                    //По дескриптору
                    case 4: 
                        Console.Write("Type a handle: ");
                        IntPtr readProcessHandle = (IntPtr)Int32.Parse(Console.ReadLine());

                        StringBuilder longFileNameSB2 = new StringBuilder(256);
                        api.GetModuleFileName(readProcessHandle, longFileNameSB2, longFileNameSB2.Capacity);
                        longFileName = longFileNameSB2.ToString();

                        shortFileName = System.IO.Path.GetFileName(longFileName);

                        Console.WriteLine(
                            $"Full path: {longFileName}\n" +
                            $"File: {shortFileName}\n"
                            );
                        break;

                    //Информация о процессе
                    case 5: 
                        uint processId = api.GetCurrentProcessId();
                        uint processPseudoId = api.GetCurrentProcess();
                        uint duplicatedHandle;
                        api.DuplicateHandle(processPseudoId, processPseudoId, processPseudoId, out duplicatedHandle, 0, false, (uint)api.DuplicateOptions.DUPLICATE_SAME_ACCESS);
                        IntPtr openedHandle = api.OpenProcess(api.ProcessAccessFlags.DuplicateHandle, true, (int)processId);

                        bool dplctd = api.CloseHandle((IntPtr)duplicatedHandle);
                        bool opnd = api.CloseHandle(openedHandle);

                        Console.WriteLine(
                            $"ID: {processId}" +
                            $"Pseudo ID: {processPseudoId}" +
                            $"Handle, catching with 'DuplicateHande' function: {duplicatedHandle}\n" +
                            $"Handle, catching with 'OpenProcess' function: {openedHandle}\n" +
                            $"Close the handle (DuplicateHande): {dplctd}\n" +
                            $"Close the handle (OpenProcess): {opnd}\n"
                            );
                        break;

                    //Перечисление всего
                    case 6: 
                        foreach (Process i in Process.GetProcesses())
                        {
                            Console.WriteLine(i.ProcessName);
                            try
                            {
                                foreach (var m in i.Modules)
                                {
                                    Console.WriteLine("\t" + m.ToString().Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)[1]);

                                }
                            }
                            catch { }
                            foreach (ProcessThread thread in i.Threads)
                            {
                                Console.WriteLine("\t\t" + thread.Id);
                            }
                        }

                        break;
                        default:
                        break;
                }
            }
        }

      
    }
    }

