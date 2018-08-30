using Gremlins.Models;
using Gremlins.Models.System;
using Gremlins.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Gremlins.Utilities.Enums;
using static Gremlins.Utilities.HardwareHelpers;

namespace Gremlins.System
{
    /// <summary>
    /// This Gremlin is used to create heavy CPU usage. Useful for hardening applications that might suffer errors
    /// when the CPU is taxed.
    /// </summary>
    public class CpuGremlin
    {
        private static int _cpuCount => GlobalVariables.System.CpuCount;
        private static int _coresPerCpu => GlobalVariables.System.CoresPerCpu;
        private static int _logicalProcessorsPerCpu => GlobalVariables.System.LogicalProcessorsPerCpu;

        /// <summary>
        /// Array containing all the threads and thread information for record keeping.
        /// </summary>
        public static ThreadContainer[] ThreadContainers = null;

        /// <summary>
        /// A class to hold variables that can be used through the library.
        /// </summary>
        public static GlobalVariables GlobalVariables = new GlobalVariables();

        /// <summary>
        /// Supports multiple CPUs and around 64 cores.
        /// </summary>
        /// <param name="threadPriority"></param>
        public async Task UseAllCpuCoresAsync(ThreadPriority threadPriority = ThreadPriority.Lowest)
        {
            ThreadContainers = new ThreadContainer[_cpuCount * Math.Max(_coresPerCpu, _logicalProcessorsPerCpu)];
            await CreateThreadContainers(threadPriority);
            await StartThreads();
        }

        /// <summary>
        /// Creates a Thread per CPU core/logical processor.
        /// </summary>
        /// <param name="threadPriority"></param>
        public Task CreateThreadContainers(ThreadPriority threadPriority = ThreadPriority.Lowest)
        {
            var index = 0;
            for (int currentCpu = 0; currentCpu < _cpuCount; currentCpu++)
            {
                for (int currentCore = 0; currentCore < Math.Max(_coresPerCpu, _logicalProcessorsPerCpu); currentCore++)
                {
                    var threadContainer = new ThreadContainer
                    {
                        CpuCoreNumber = currentCore,
                        CpuLogicalProcessorNumber = currentCore,
                        CpuNumber = currentCpu,
                        CoresPerCpu = _coresPerCpu,
                        LogicalProcessorsPerCpu = _logicalProcessorsPerCpu,
                        Thread = new Thread(ThreadWorker),
                        ThreadStatus = ThreadStatus.Idle
                    };

                    threadContainer.Thread.Name = $"CpuGremlin #{currentCpu}-{currentCore}";
                    threadContainer.Thread.Priority = threadPriority;

                    ThreadContainers[index] = threadContainer;
                    index++;
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Starts all the threads in the ThreadContainer.
        /// </summary>
        /// <returns></returns>
        public Task StartThreads()
        {
            for(int i = 0; i < ThreadContainers.Length; i++)
            {
                ThreadContainers[i].Thread.Start(i);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Worker sets the affinity to its assigned CPU Logical processor and then engages the core/logical processor to
        /// 100% utilization.
        /// </summary>
        /// <param name="ThreadNumber"></param>
        public void ThreadWorker(object ThreadNumber)
        {
            var threadContainer = ThreadContainers[(int)ThreadNumber];

            var affinityMask = CalculateCoreAffinity(threadContainer.CpuNumber,
                threadContainer.CpuCoreNumber,
                threadContainer.LogicalProcessorsPerCpu);

            NativeMethods.SetThreadAffinityMask(NativeMethods.GetCurrentThread(), new IntPtr(affinityMask));

            threadContainer.ThreadStatus = ThreadStatus.Processing;

            while(true) { }
        }
    }
}
