using Gremlins.Monitoring;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Gremlins.Demo.Models.GlobalVariables;

namespace Gremlins.Demo.Examples
{
    public static class SqlMonitoringExamples
    {
        public static async Task GetActiveConnectionsAsync()
        {
            var sqlMonitor = new SqlMonitoring();

            using (var conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();

                await sqlMonitor.WriteStatsToConsoleAsync();

                conn.Close();
                conn.Dispose();

                await sqlMonitor.WriteStatsToConsoleAsync();
            }
        }
    }
}
