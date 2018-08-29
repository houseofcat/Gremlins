using Gremlins.Monitoring;
using Gremlins.Sql;
using System;
using System.Threading.Tasks;
using static Gremlins.Demo.Models.GlobalVariables;

namespace Gremlins.Demo.Examples
{
    public static class SqlConnectionGremlinExamples
    {
        public static async Task MessWithOpenConnectionsAsync()
        {
            await Console.Out.WriteLineAsync("Attack of the Gremlins!\n\n\tOh No! They are adding SQL Connections!");

            SqlMonitoring sqlMonitor = new SqlMonitoring();
            SqlConnectionGremlin sqlGremlin = new SqlConnectionGremlin();

            await Console.Out.WriteLineAsync("\n== Adding 99 connections...");

            try
            { await sqlGremlin.AddOpenConnectionsAsync(ConnectionString, 99); }
            catch (Exception ex)
            { await Console.Out.WriteLineAsync($"n\tException: {ex.Message}"); }

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("\n== Adding 99 more connections...");

            try
            { await sqlGremlin.AddOpenConnectionsAsync(ConnectionString, 5); }
            catch (Exception ex)
            { await Console.Out.WriteLineAsync($"\n\tException: {ex.Message}"); }

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("\n== Closing and removing all connections from memory!");

            await sqlGremlin.ResetGremlinAsync();

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("\n== Adding 99 connections...");

            try
            { await sqlGremlin.AddOpenConnectionsAsync(ConnectionString, 99); }
            catch (Exception ex)
            { await Console.Out.WriteLineAsync($"n\tException: {ex.Message}"); }

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("=\n== Closing 99 connections...");

            try
            { await sqlGremlin.CloseOpenConnectionsAsync(99); }
            catch (Exception ex)
            { await Console.Out.WriteLineAsync($"n\tException: {ex.Message}"); }

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("\n== Closing 99 more connections...");

            try
            { await sqlGremlin.CloseOpenConnectionsAsync(99); }
            catch (Exception ex)
            { await Console.Out.WriteLineAsync($"n\tException: {ex.Message}"); }

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("\n== Closing and removing all connections from memory!");

            await sqlGremlin.ResetGremlinAsync();

            await sqlMonitor.WriteStatsToConsoleAsync();

            await Console.Out.WriteLineAsync("\n\nThe Gremlins have been defeated!");
        }
    }
}
