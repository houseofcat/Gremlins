using System;
using System.Management;

namespace Gremlins.Utilities
{
    /// <summary>
    /// Class that has methods that interact with Windows to get Hardware information.
    /// </summary>
    public class HardwareHelpers
    {
        #region Wmi Strings

        private static readonly string Wmi_Root = "root\\CIMV2";
        private static readonly string Wmi_NumberOfProcessors = "SELECT NumberOfProcessors FROM Win32_ComputerSystem";
        private static readonly string Wmi_NumberOfCores = "SELECT NumberOfCores FROM Win32_Processor";

        #endregion

        #region Cpu Helpers

        /// <summary>
        /// Gets the number of CPUs installed on the system.
        /// </summary>
        /// <returns></returns>
        public static int GetCpuCount()
        {
            var processorCount = 0;

            try
            {
                var mos = new ManagementObjectSearcher(Wmi_Root, Wmi_NumberOfProcessors);

                foreach (var mo in mos.Get())
                {
                    if (mo["NumberOfProcessors"] != null)
                    { processorCount += int.Parse(mo["NumberOfProcessors"].ToString()); }
                }
            }
            catch { processorCount = 0; }

            return processorCount;
        }

        /// <summary>
        /// Gets the number of physical Cores on each CPU.
        /// </summary>
        /// <returns></returns>
        public static int GetCoreCount()
        {
            var coreCount = 0;

            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher(Wmi_Root, Wmi_NumberOfCores);

                foreach (ManagementObject mo in mos.Get())
                {
                    if ((mo["NumberOfCores"]) != null)
                    { coreCount += int.Parse(mo["NumberOfCores"].ToString()); }
                }
            }
            catch { coreCount = 0; }

            return coreCount;
        }

        /// <summary>
        /// Gets the number of Logical Processors the OS has access to.
        /// </summary>
        /// <returns></returns>
        public static int GetTotalLogicalProcessorCount()
        {
            return Environment.ProcessorCount;
        }

        /// <summary>
        /// Used for calculating ThreadAffinity. Necessary for assigning a Core to a CPU.
        /// </summary>
        /// <param name="currentCpu"></param>
        /// <param name="currentCore"></param>
        /// <param name="logicalProcessCount"></param>
        /// <returns></returns>
        public static long CalculateCoreAffinity(int currentCpu, int currentCore, int logicalProcessCount)
        {
            var affinity = 0;

            if (currentCpu == 0)
            { affinity = currentCore; }
            else
            { affinity = currentCore + (int)Math.Pow(logicalProcessCount, currentCpu); }

            return 1L << affinity;
        }

        #endregion
    }
}
