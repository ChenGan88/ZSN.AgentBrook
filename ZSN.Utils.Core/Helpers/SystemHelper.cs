using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ZSN.Utils.Core.Helpers
{
    /// <summary>
    /// 操作系统 的摘要说明。
    /// </summary>
    public class SystemHelper
    {
        #region 单位转换进制

        private const int KbDiv = 1024;
        private const int MbDiv = 1024 * 1024;
        private const int GbDiv = 1024 * 1024 * 1024;

        #endregion

        #region 获取磁盘占用率

        public static double GetUsedDiskPercent()
        {
            float usedSize = GetUsedDiskSize();
            float totalSize = GetTotalSize();
            double percent = (double)usedSize / totalSize;
            return Convert.ToDouble(percent * 100);
        }

        /// <summary>
        /// 所在盘使用量
        /// </summary>
        /// <returns></returns>
        public static float GetUsedDiskSize()
        {
            var currentDrive = GetCurrentDrive();
            float UsedDiskSize = currentDrive.TotalSize - currentDrive.TotalFreeSpace;
            return UsedDiskSize / MbDiv;
        }

        /// <summary>
        /// 所在盘总量
        /// </summary>
        /// <returns></returns>
        public static float GetTotalSize()
        {
            var currentDrive = GetCurrentDrive();
            float TotalSize = currentDrive.TotalSize / MbDiv;
            return TotalSize;
        }

        /// <summary>
        /// 获取当前执行的盘符信息
        /// </summary>
        /// <returns></returns>
        public static DriveInfo GetCurrentDrive()
        {
            string path = Environment.CurrentDirectory.Substring(0, 3);
            return DriveInfo.GetDrives().FirstOrDefault(p => p.Name.Equals(path));
        }
        #endregion

        #region 获取当前活动网络MAC地址

        /// <summary>
        /// 获取本机MAC地址
        /// </summary>
        /// <param name="separator"></param>
        public static Dictionary<string, string> GetActiveMacAddress(string separator = "-")
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            var macAddress = new Dictionary<string, string>();

            if (nics == null || nics.Length < 1)
            {
                return macAddress;
            }

            foreach (NetworkInterface adapter in nics.Where(c =>
             c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up))
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                var unicastAddresses = properties.UnicastAddresses;
                if (unicastAddresses.Any(t => t.Address.AddressFamily == AddressFamily.InterNetwork))
                {
                    var address = adapter.GetPhysicalAddress();
                    string mac;
                    if (string.IsNullOrEmpty(separator))
                    {
                        mac = address.ToString();
                    }
                    else
                    {
                        mac = string.Join(separator, address.GetAddressBytes().Select(b => b.ToString("X2")));
                    }
                    macAddress.Add(adapter.Name, mac);
                }
            }
            return macAddress;
        }

        #endregion

        #region 获取CPU使用率

        #region AIP声明
        [DllImport("IpHlpApi.dll")]
        extern static public uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        [DllImport("User32")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);
        #endregion

        public static float GetCpuUsedRate()
        {
            try
            {
                PerformanceCounter pcCpuLoad;
                pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total")
                {
                    MachineName = "."
                };
                pcCpuLoad.NextValue();
                Thread.Sleep(1500);
                float CpuLoad = pcCpuLoad.NextValue();
                return CpuLoad;
            }
            catch
            {
            }
            return 0;
        }

        #endregion

        #region 获取内存使用率

        #region 可用内存

        /// <summary>
        /// 获取可用内存
        /// </summary>
        //internal static long? GetMemoryAvailable()
        //{

        //    long availablebytes = 0;
        //    var managementClassOs = new ManagementClass("Win32_OperatingSystem");
        //    foreach (var managementBaseObject in managementClassOs.GetInstances())
        //        if (managementBaseObject["FreePhysicalMemory"] != null)
        //            availablebytes = 1024 * long.Parse(managementBaseObject["FreePhysicalMemory"].ToString());
        //    return availablebytes / MbDiv;

        //}

        #endregion

        #endregion
    }
}
