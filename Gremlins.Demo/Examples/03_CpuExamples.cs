using HouseofCat.Library.Gremlins;
using System;
using System.Threading.Tasks;
using static HouseofCat.Library.Monitoring.ThreadInfo;

namespace HouseofCat.Gremlins.Demo.Examples
{
    public static class CpuGremlinExamples
    {
        public static async Task UseAllCpuCoresAsync()
        {
            using (var cpuUsageGremlin = new CpuUsageGremlin())
            {
                await Console.Out.WriteLineAsync("\nCpu Usage Gremlin Example Beings!");

                await cpuUsageGremlin.UseAllCpuCoresAsync();

                for (int i = 0; i < 10; i++)
                { await WriteThreadUsageToConsole(cpuUsageGremlin); }

                await Console.Out.WriteLineAsync("\nStopping gremlin!");

                await cpuUsageGremlin.StopCpuCoreThreadsAsync();

                await WriteThreadUsageToConsole(cpuUsageGremlin);

                await cpuUsageGremlin.ResetGremlinAsync();
            }

            await Console.Out.WriteLineAsync("\nCpu Usage Gremlin Example Ends!");
        }

        private static async Task WriteThreadUsageToConsole(CpuUsageGremlin cpuUsageGremlin)
        {
            await Task.Delay(1000);
            await Console.Out.WriteLineAsync($"\nActive Threads In ThreadPool: {await GetActiveThreadCountInThreadPoolAsync()}");
            await Console.Out.WriteLineAsync($"Current Process Thread Count: {await GetCurrentProcessThreadCountAsync()}");
            await Console.Out.WriteLineAsync($"Active Threads Used By Gremlins: "
                + $"{await cpuUsageGremlin.GetActiveThreadCountAsync()} (of {await cpuUsageGremlin.GetThreadCountAsync()})");
        }
    }
}
