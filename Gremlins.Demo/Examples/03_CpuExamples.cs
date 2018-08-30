using Gremlins.System;
using System.Threading.Tasks;

namespace Gremlins.Demo.Examples
{
    public static class CpuGremlinExamples
    {
        public static async Task UseAllCpuCoresAsync()
        {
            CpuUsageGremlin cpuUsageGremlin = new CpuUsageGremlin();

            await cpuUsageGremlin.UseAllCpuCoresAsync();
        }
    }
}
