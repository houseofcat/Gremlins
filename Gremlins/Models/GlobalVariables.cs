using Gremlins.Models.System;

namespace Gremlins.Models
{
    /// <summary>
    /// Class to hold variables that will be useful across the Library.
    /// </summary>
    public class GlobalVariables
    {
        /// <summary>
        /// Holds data/variables specific to the computer system.
        /// </summary>
        public static SystemVariables System { get; set; } = new SystemVariables();
    }
}
