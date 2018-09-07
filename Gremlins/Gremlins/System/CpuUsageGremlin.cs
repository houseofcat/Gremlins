using Gremlins.Models.System;
using Gremlins.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Gremlins.Utilities.Enums;
using static Gremlins.Utilities.Helpers.ThreadHelpers;

namespace Gremlins.System
{
    /// <summary>
    /// This Gremlin is used to create heavy CPU usage. Useful for hardening applications that might suffer errors
    /// when the CPU is taxed.
    /// </summary>
    public class CpuUsageGremlin : IDisposable
    {
        #region Private Variables

        private static int _cpuCount => SystemVariables.CpuCount;
        private static int _coresPerCpu => SystemVariables.CoresPerCpu;
        private static int _logicalProcessorsPerCpu => SystemVariables.LogicalProcessorsPerCpu;

        #endregion

        #region Public Variables

        /// <summary>
        /// Array containing all the threads to be assigned to a Cpue Core and thread information for record keeping.
        /// </summary>
        public ThreadContainer[] CpuCoreThreadContainers = null;

        #endregion

        /// <summary>
        /// Supports multiple CPUs and around 64 cores.
        /// </summary>
        /// <param name="threadPriority"></param>
        public async Task UseAllCpuCoresAsync(ThreadPriority threadPriority = ThreadPriority.Lowest)
        {
            await StartCpuCoreThreadsAsync(threadPriority);
        }

        /// <summary>
        /// Starts all the threads in the CpuCoreThreadContainer.
        /// </summary>
        public async Task StartCpuCoreThreadsAsync(ThreadPriority threadPriority = ThreadPriority.Lowest)
        {
            if (CpuCoreThreadContainers == null)
            {
                CpuCoreThreadContainers = await CreateCpuCoreThreadContainersAsync(threadPriority);
            }

            for (int i = 0; i < CpuCoreThreadContainers.Length; i++)
            {
                CpuCoreThreadContainers[i].ThreadStatus = ThreadStatus.Processing;
                CpuCoreThreadContainers[i].TerminateSelf = false;
                CpuCoreThreadContainers[i].Thread?.Start(i);
            }
        }

        /// <summary>
        /// Stops all the threads in the CpuCoreThreadContainer.
        /// </summary>
        public Task StopCpuCoreThreadsAsync()
        {
            if (CpuCoreThreadContainers != null)
            {
                for (int i = 0; i < CpuCoreThreadContainers.Length; i++)
                {
                    CpuCoreThreadContainers[i].TerminateSelf = true;
                    CpuCoreThreadContainers[i].ThreadStatus = ThreadStatus.Idle;
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the number of active threads used by this Gremlin.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetActiveThreadCountAsync()
        {
            var count = CpuCoreThreadContainers?.Where(tc => tc.ThreadStatus == ThreadStatus.Processing).Count() ?? 0;
            return Task.FromResult(count);
        }

        /// <summary>
        /// Get the number of threads used by this Gremlin.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetThreadCountAsync()
        {
            return Task.FromResult(CpuCoreThreadContainers?.Count() ?? 0);
        }

        /// <summary>
        /// Stops all active threads. Disposes them and reset Gremlin to its original state.
        /// </summary>
        /// <returns></returns>
        public async Task ResetGremlinAsync()
        {
            if (CpuCoreThreadContainers != null)
            {
                await StopCpuCoreThreadsAsync();

                for (int i = 0; i < CpuCoreThreadContainers.Length; i++)
                {
                    CpuCoreThreadContainers[i].Thread.Join();
                    CpuCoreThreadContainers[i].Thread = null;
                }

                CpuCoreThreadContainers = null;
            }
        }

        #region Helpers

        /// <summary>
        /// Creates a unique Thread per CPU core/logical processor.
        /// </summary>
        /// <param name="threadPriority"></param>
        public Task<ThreadContainer[]> CreateCpuCoreThreadContainersAsync(ThreadPriority threadPriority = ThreadPriority.Lowest)
        {
            var cpuCoreThreadContainers = new ThreadContainer[_cpuCount * Math.Max(_coresPerCpu, _logicalProcessorsPerCpu)];
            var maximumThreadsPerCpu = Math.Max(_coresPerCpu, _logicalProcessorsPerCpu);

            for (int currentCpu = 0; currentCpu < _cpuCount; currentCpu++)
            {
                for (int currentCore = 0; currentCore < maximumThreadsPerCpu; currentCore++)
                {
                    var threadContainer = new ThreadContainer
                    {
                        CpuCoreNumber = currentCore,
                        CpuLogicalProcessorNumber = currentCore,
                        CpuNumber = currentCpu,
                        CoresPerCpu = _coresPerCpu,
                        LogicalProcessorsPerCpu = _logicalProcessorsPerCpu,
                        ThreadStatus = ThreadStatus.Idle,
                        Thread = new Thread(ThreadWorker)
                        {
                            Name = $"CpuUsageGremlin #{currentCpu}-{currentCore}",
                            Priority = threadPriority,
                            IsBackground = true
                        }
                    };

                    cpuCoreThreadContainers[currentCpu * maximumThreadsPerCpu + currentCore] = threadContainer;
                }
            }

            return Task.FromResult(cpuCoreThreadContainers);
        }

        /// <summary>
        /// Worker sets the affinity to its assigned CPU Logical Processor and then engages in work to get 100%
        /// utilization.
        /// </summary>
        /// <param name="threadNumber"></param>
        private async void ThreadWorker(object threadNumber)
        {
            var threadContainer = CpuCoreThreadContainers[(int)threadNumber];
            await SetThreadAffinity(NativeMethods.GetCurrentThread(),
                threadContainer.CpuNumber,
                threadContainer.CpuLogicalProcessorNumber,
                threadContainer.LogicalProcessorsPerCpu);

            if (Monitor.TryEnter(threadContainer.FuncLock))
            {
                while (!threadContainer.TerminateSelf)
                {
                    if (threadContainer.ThrottleTime > 0)
                    { await AsyncWork(threadContainer.ThrottleTime); }
                    else if (threadContainer.AsyncFuncWork != default)
                    { await threadContainer.AsyncFuncWork(threadNumber); }
                }

                Monitor.Exit(threadContainer.FuncLock);
            }
        }

        private async Task AsyncWork(int throttleTime)
        {
            await Task.Delay(throttleTime);
        }

        private bool disposedValue = false;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual async void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    await ResetGremlinAsync();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
