using System.Threading;
using static Gremlins.Utilities.Enums;

namespace Gremlins.Models.System
{
    /// <summary>
    /// ThreadContainer holds a thread and the CPU information inteded for it to be assigned a specific
    /// CPU core or LogicalProcessor.
    /// </summary>
    public class ThreadContainer
    {
        /// <summary>
        /// The Thread that will be assigned to a Core/Logical Processor.
        /// </summary>
        public Thread Thread { get; set; } = null;

        /// <summary>
        /// The CPU this Thread is assigned to.
        /// </summary>
        public int CpuNumber { get; set; } = 0;

        /// <summary>
        /// The physical Core/Logical Processor this Thread is assigned to.
        /// </summary>
        public int CpuCoreNumber { get; set; } = 0;

        /// <summary>
        /// The physical Core/Logical Processor this Thread is assigned to.
        /// </summary>
        public int CpuLogicalProcessorNumber { get; set; } = 0;

        /// <summary>
        /// The physical Core count per CPU.
        /// </summary>
        public int CoresPerCpu { get; set; } = 0;

        /// <summary>
        /// The Logical Processors per CPU as seen by the OS.
        /// </summary>
        public int LogicalProcessorsPerCpu { get; set; } = 0;

        /// <summary>
        /// The ThreadStatus helps quickly identify what work state is for the Thread stored here.
        /// </summary>
        public ThreadStatus ThreadStatus { get; set; } = ThreadStatus.NoThread;
    }
}
