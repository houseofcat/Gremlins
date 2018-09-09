using System.Threading.Tasks;
using static HouseofCat.Gremlins.Demo.Examples.CpuGremlinExamples;
using static HouseofCat.Gremlins.Demo.Examples.SqlMonitoringExamples;
using static HouseofCat.Gremlins.Demo.Examples.SqlConnectionGremlinExamples;
using System;

namespace HouseofCat.Gremlins.Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Creates a connection and shows how many active connections there are.
            //await GetActiveConnectionsAsync();

            //SqlConnectionGremlin - Messes with how many connections are open at any one time.
            //await MessWithOpenConnectionsAsync();

            //CpuGremlin - Put a high load on all the CPU logical processors.
            await UseAllCpuCoresAsync();
            await Console.In.ReadLineAsync();
        }
    }
}
