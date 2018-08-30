using Gremlins.System;
using System.Threading.Tasks;

namespace Gremlins.Demo.Examples
{
    public static class CpuGremlinExamples
    {
        public static async Task UseAllCpuCoresAsync()
        {
            CpuGremlin cpuGremlin = new CpuGremlin();

            await cpuGremlin.UseAllCpuCoresAsync();
        }
    }
}
