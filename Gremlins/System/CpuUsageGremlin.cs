using Gremlins.Models;
using Gremlins.Models.System;
using Gremlins.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Gremlins.Utilities.Enums;
using static Gremlins.Utilities.ApplicationHelpers;

namespace Gremlins.System
{
    /// <summary>
    /// This Gremlin is used to create heavy CPU usage. Useful for hardening applications that might suffer errors
    /// when the CPU is taxed.
    /// </summary>
    public class CpuUsageGremlin
    {
        private static int _cpuCount => GlobalVariables.System.CpuCount;
        private static int _coresPerCpu => GlobalVariables.System.CoresPerCpu;
        private static int _logicalProcessorsPerCpu => GlobalVariables.System.LogicalProcessorsPerCpu;

        /// <summary>
        /// Array containing all the threads to be assigned to a Cpue Core and thread information for record keeping.
        /// </summary>
        public static ThreadContainer[] CpuCoreThreadContainers = null;

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
            CpuCoreThreadContainers = new ThreadContainer[_cpuCount * Math.Max(_coresPerCpu, _logicalProcessorsPerCpu)];
            await CreateCpuCoreThreadContainers(threadPriority);
            await StartCpuCoreThreads();
        }

        /// <summary>
        /// Creates a Thread per CPU core/logical processor.
        /// </summary>
        /// <param name="threadPriority"></param>
        public Task CreateCpuCoreThreadContainers(ThreadPriority threadPriority = ThreadPriority.Lowest)
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
                    threadContainer.Thread.IsBackground = true;

                    CpuCoreThreadContainers[index] = threadContainer;
                    index++;
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Starts all the threads in the CpuCoreThreadContainer.
        /// </summary>
        /// <returns></returns>
        public Task StartCpuCoreThreads()
        {
            for (int i = 0; i < CpuCoreThreadContainers.Length; i++)
            {
                CpuCoreThreadContainers[i].TerminateSelf = false;
                CpuCoreThreadContainers[i].Thread?.Start(i);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops all the threads in the CpuCoreThreadContainer.
        /// </summary>
        /// <returns></returns>
        public Task StopCpuCoreThreads()
        {
            for (int i = 0; i < CpuCoreThreadContainers.Length; i++)
            {
                CpuCoreThreadContainers[i].TerminateSelf = true;
                CpuCoreThreadContainers[i].ThreadStatus = ThreadStatus.Idle;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Worker sets the affinity to its assigned CPU Logical Processor and then engages in work to get 100%
        /// utilization.
        /// </summary>
        /// <param name="ThreadNumber"></param>
        public async void ThreadWorker(object ThreadNumber)
        {
            var threadContainer = CpuCoreThreadContainers[(int)ThreadNumber];
            await SetThreadAffinity(NativeMethods.GetCurrentThread(),
                threadContainer.CpuNumber,
                threadContainer.CpuLogicalProcessorNumber,
                threadContainer.LogicalProcessorsPerCpu);

            threadContainer.ThreadStatus = ThreadStatus.Processing;

            if (Monitor.TryEnter(threadContainer.FuncLock))
            {
                while (!threadContainer.TerminateSelf)
                {
                    if (threadContainer.ThrottleTime > 0)
                    {
                        await AsyncWork(threadContainer.ThrottleTime);
                    }
                    else if (threadContainer.AsyncFuncWork != default)
                    {
                        await threadContainer.AsyncFuncWork(ThreadNumber);
                    }
                }

                Monitor.Exit(threadContainer.FuncLock);
            }
        }

        private async Task AsyncWork(int throttleTime)
        {
            await Task.Delay(throttleTime);
        }       
    }
}
