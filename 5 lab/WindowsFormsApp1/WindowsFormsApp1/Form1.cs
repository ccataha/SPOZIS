using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e) //Got virtual memory map on C++ project
        {
            System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
            MyProc.StartInfo.FileName = @"D:\всякое\3_Курс\2_СЕМ\СПОЗИС\5\ConsoleApplication1\ConsoleApplication1.exe";
            MyProc.Start();
        }
        private void chart1_Click_1(object sender, EventArgs e) //Physical memory
        {
            MemoryStatus status = MemoryStatus.CreateInstance();
            uint MemoryLoad = status.MemoryLoad;
            ulong TotalPhys = status.TotalPhys;
            ulong AvailPhys = status.AvailPhys;
            ulong AvailExtendedVirtual = status.AvailExtendedVirtual;
            listBox1.Items.Add("Physical memory = " + TotalPhys / 1024 / 1024 + " MB");
            listBox1.Items.Add("\nAvailable memory = " + AvailPhys / 1024 / 1024 + " MB");
            List<ulong> statuses = new List<ulong>() { TotalPhys, AvailPhys, MemoryLoad, MemoryLoad };
            chart1.Series["Memory distribution"].Points.AddXY("\n\nTotal\nphysical\nmemory\n", TotalPhys);
            chart1.Series["Memory distribution"].Points.AddXY("Available memory\n", AvailPhys);
        }

        private void chart2_Click_1(object sender, EventArgs e) //Swap file
        {
            MemoryStatus status = MemoryStatus.CreateInstance();
            uint MemoryLoad = status.MemoryLoad;
            ulong TotalPageFile = status.TotalPageFile;
            ulong AvailPageFile = status.AvailPageFile;
            listBox2.Items.Add("Total swap file memory = " + TotalPageFile / 1024 / 1024 + " MB");
            listBox2.Items.Add("Available swap file memory = " + AvailPageFile / 1024 / 1024 + " MB");
            listBox2.Items.Add("Used memory by this process = " + MemoryLoad + "byte");
            List<ulong> statuses = new List<ulong>() { TotalPageFile, AvailPageFile, MemoryLoad, MemoryLoad };
            chart2.Series["Memory distribution"].Points.AddXY("\n\nTotal\nswap\nfile\n", TotalPageFile);
            chart2.Series["Memory distribution"].Points.AddXY("Swap file capacity\nAvailable in this moment\n", AvailPageFile);
        }

        private void chart3_Click_1(object sender, EventArgs e) //Virtual memory
        {
            MemoryStatus status = MemoryStatus.CreateInstance();
            uint MemoryLoad = status.MemoryLoad;
            ulong TotalVirtual = status.TotalVirtual;
            ulong AvailVirtual = status.AvailVirtual;
            listBox3.Items.Add("Total virtual memory = " + TotalVirtual / 1024 / 1024 + " MB");
            listBox3.Items.Add("Available virtual memory = " + AvailVirtual / 1024 / 1024 + " MB");
            List<ulong> statuses = new List<ulong>() { MemoryLoad, TotalVirtual, AvailVirtual, MemoryLoad };
            chart3.Series["Memory distribution"].Points.AddXY("Total\nvirtual\nmemory\n", TotalVirtual);
            chart3.Series["Memory distribution"].Points.AddXY("Capacity of available\nvirtual memory\n", AvailVirtual);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
