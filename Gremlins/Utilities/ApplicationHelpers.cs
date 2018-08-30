﻿using System;
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
        /// Uses NativeMethods to set the Cpu Core affinity of a thread.
        /// </summary>
        /// <param name="threadPointer"></param>
        /// <param name="cpuNumber"></param>
        /// <param name="cpuCoreNumber"></param>
        /// <param name="logicalProcessorCount"></param>
        /// <returns></returns>
        public static Task SetThreadAffinity(IntPtr threadPointer, int cpuNumber, int cpuCoreNumber, int logicalProcessorCount)
        {
            var affinityMask = CalculateCoreAffinity(cpuNumber, cpuCoreNumber, logicalProcessorCount);

            NativeMethods.SetThreadAffinityMask(threadPointer, new IntPtr(affinityMask));

            return Task.CompletedTask;
        }
    }
}
