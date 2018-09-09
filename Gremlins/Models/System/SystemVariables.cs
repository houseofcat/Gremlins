using static HouseofCat.Gremlins.Utilities.Helpers.HardwareHelpers;

namespace HouseofCat.Gremlins.Models.System
{
    /// <summary>
    /// This class contains variables related to the System such as hardware info.
    /// </summary>
    public static class SystemVariables
    {
        /// <summary>
        /// Number of CPUs installed.
        /// </summary>
        public static int CpuCount { get; private set; } = GetCpuCount();

        /// <summary>
        /// Number of physical Cores per CPU.
        /// </summary>
        public static int CoreCount { get; private set; } = GetCoreCount();

        /// <summary>
        /// The logical processors as seen by the OS.
        /// </summary>
        public static int LogicalProcessorCount { get; private set; } = GetTotalLogicalProcessorCount();

        /// <summary>
        /// Calculation of the physical Cores per CPU.
        /// </summary>
        public static int CoresPerCpu => CoreCount / CpuCount;

        /// <summary>
        /// Calculation of the Logical Processors per CPU.
        /// </summary>
        public static int LogicalProcessorsPerCpu => LogicalProcessorCount / CpuCount;
    }
}
