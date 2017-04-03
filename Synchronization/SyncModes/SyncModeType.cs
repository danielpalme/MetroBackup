
namespace Palmmedia.BackUp.Synchronization.SyncModes
{
    /// <summary>
    /// Enum containing the sync modes.
    /// </summary>
    public enum SyncModeType
    {
        /// <summary>
        /// Mirror Reference Directory.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Mirror Reference Directory (without deleting files).
        /// </summary>
        LeftToRightWithoutDeletion,

        /// <summary>
        /// Synchronize in both directions.
        /// </summary>
        BothWays
    }
}