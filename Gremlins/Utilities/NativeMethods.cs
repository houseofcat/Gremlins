using System.Runtime.InteropServices;

namespace Gremlins.Utilities
{
    /// <summary>
    /// NativeMethods has all Masrhal/Interop code.
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// Gets PID using Kerne32.dll.
        /// </summary>
        /// <returns>ProcessId</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentProcessId();
    }
}
