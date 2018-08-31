using System.Reflection;
using System.Threading.Tasks;

namespace Gremlins.Utilities
{
    /// <summary>
    /// Class for doing miscellaneous application and system calls.
    /// </summary>
    public static class ApplicationHelpers
    {
        /// <summary>
        /// Uses NativeMethods to get the PerformanceCounter Instance name.
        /// </summary>
        /// <returns></returns>
        public static Task<string> GetInstanceName()
        {
            var instanceName = Assembly.GetEntryAssembly().GetName().Name;
            var pid = NativeMethods.GetCurrentProcessId().ToString();

            return Task.FromResult($"{instanceName}[{pid}]");
        }
    }
}
