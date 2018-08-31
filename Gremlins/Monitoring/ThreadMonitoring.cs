using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gremlins.Monitoring
{
    /// <summary>
    /// Class that helps monitor Thread usage.
    /// </summary>
    public static class ThreadMonitoring
    {
        public static Task<int> GetActiveThreadCountInThreadPoolAsync()
        {
            ThreadPool.GetMaxThreads(out int maxThreads, out int completionPortThreads);
            ThreadPool.GetAvailableThreads(out int availableThreads, out completionPortThreads);

            return Task.FromResult(maxThreads - availableThreads);
        }

        public static Task<int> GetCurrentProcessThreadCountAsync()
        {
            return Task.FromResult(Process.GetCurrentProcess().Threads.Count);
        }

        public static Task<int> GetActiveThreadsCountAsync()
        {
            ProcessThreadCollection threads = Process.GetCurrentProcess().Threads;
            var activeCount = 0;

            return Task.FromResult(activeCount);
        }
    }
}
