namespace Gremlins.Utilities
{
    /// <summary>
    /// Gremlins static Enumerations.
    /// </summary>
    public static class Enums
    {
        #region Sql Enums

        /// <summary>
        /// PerformanceCounter names for Ado.Net PerformanceCounters
        /// </summary>
        public enum AdoNetPerformanceCounters
        {
            /// <summary>
            /// NumberOfActiveConnectionPools (Index Value = 0)
            /// </summary>
            NumberOfActiveConnectionPools = 0,
            /// <summary>
            /// NumberOfReclaimedConnections (Index Value = 1)
            /// </summary>
            NumberOfReclaimedConnections = 1,
            /// <summary>
            /// HardConnectsPerSecond (Index Value = 2)
            /// </summary>
            HardConnectsPerSecond = 2,
            /// <summary>
            /// HardDisconnectsPerSecond (Index Value = 3)
            /// </summary>
            HardDisconnectsPerSecond = 3,
            /// <summary>
            /// NumberOfActiveConnectionPoolGroups (Index Value = 4)
            /// </summary>
            NumberOfActiveConnectionPoolGroups = 4,
            /// <summary>
            /// NumberOfInactiveConnectionPoolGroups (Index Value = 5)
            /// </summary>
            NumberOfInactiveConnectionPoolGroups = 5,
            /// <summary>
            /// NumberOfInactiveConnectionPools (Index Value = 6)
            /// </summary>
            NumberOfInactiveConnectionPools = 6,
            /// <summary>
            /// NumberOfNonPooledConnections (Index Value = 7)
            /// </summary>
            NumberOfNonPooledConnections = 7,
            /// <summary>
            /// NumberOfPooledConnections (Index Value = 8)
            /// </summary>
            NumberOfPooledConnections = 8,
            /// <summary>
            /// NumberOfStasisConnections (Index Value = 9)
            /// </summary>
            NumberOfStasisConnections = 9,
            /// <summary>
            /// SoftConnectsPerSecond (Index Value = 10)
            /// </summary>
            SoftConnectsPerSecond = 10,
            /// <summary>
            /// SoftDisconnectsPerSecond (Index Value = 11)
            /// </summary>
            SoftDisconnectsPerSecond = 11,
            /// <summary>
            /// NumberOfActiveConnections (Index Value = 12)
            /// </summary>
            NumberOfActiveConnections = 12,
            /// <summary>
            /// NumberOfFreeConnections (Index Value = 13)
            /// </summary>
            NumberOfFreeConnections = 13
        }

        #endregion
    }
}
