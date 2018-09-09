using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static HouseofCat.Library.Miscellaneous;

namespace Gremlins.Monitoring
{
    /// <summary>
    /// Class that helps monitor Tcp Socket usage.
    /// </summary>
    public class TcpMonitoring
    {
        /// <summary>
        /// The storage location of the Ado.Net Performance Counters.
        /// </summary>
        public PerformanceCounter[] TcpPerformanceCounters { get; set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public TcpMonitoring()
        {
            SetupTcpPerformanceCounters().GetAwaiter().GetResult();
        }

        private async Task SetupTcpPerformanceCounters()
        {
            var instanceName = await GetInstanceName();


        }
    }
}
