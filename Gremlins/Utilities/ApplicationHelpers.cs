using System;
using System.Reflection;
using System.Threading.Tasks;
using static Gremlins.Utilities.HardwareHelpers;

namespace Gremlins.Utilities
{
    /// <summary>
    /// Class for doing miscellaneous application and system calls.
    /// </summary>
    public static class ApplicationHelpers
    {
        /// <summary>
        /// Uses NativeMethods to get the PerformanceCounter Instance name.
        /// </summary>
        /// <returns></returns>
        public static Task<string> GetInstanceName()
        {
            var instanceName = Assembly.GetEntryAssembly().GetName().Name;
            var pid = NativeMethods.GetCurrentProcessId().ToString();

            return Task.FromResult($"{instanceName}[{pid}]");
        }

        /// <summary>
        /// Uses NativeMethods to set threads affinity to a CPU logical processor. Hardware counts start at 0.
        /// <para>
        /// Example setting value to logical processor 2, single CPU machine, with 4 physical cores and 8 logical processors.
        /// </para>
        /// <para>
        /// (threadPointer, 0, 1, 8)
        /// </para>
        /// </summary>
        /// <param name="threadPointer"></param>
        /// <param name="cpuNumber"></param>
        /// <param name="logicalProcessorNumber"></param>
        /// <param name="logicalProcessorCount"></param>
        /// <returns></returns>
        public static Task SetThreadAffinity(IntPtr threadPointer, int cpuNumber, int logicalProcessorNumber, int logicalProcessorCount)
        {
            var affinityMask = CalculateCoreAffinity(cpuNumber, logicalProcessorNumber, logicalProcessorCount);

            NativeMethods.SetThreadAffinityMask(threadPointer, new IntPtr(affinityMask));

            return Task.CompletedTask;
        }
    }
}
