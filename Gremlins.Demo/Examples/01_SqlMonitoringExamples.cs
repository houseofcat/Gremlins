using HouseofCat.Library.Monitoring;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static HouseofCat.Gremlins.Demo.Models.GlobalVariables;

namespace HouseofCat.Gremlins.Demo.Examples
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
