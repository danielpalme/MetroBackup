using System;

namespace Palmmedia.BackUp.Synchronization.SyncModes
{
    /// <summary>
    /// Factory class for <see cref="ISyncMode"/>.
    /// </summary>
    internal class SyncModeFactory
    {
        /// <summary>
        /// Creates the corresponding <see cref="ISyncMode"/> to the given sync mode type.
        /// </summary>
        /// <param name="mode">The sync mode.</param>
        /// <returns>The <see cref="ISyncMode"/> instance.</returns>
        public static ISyncMode Create(SyncModeType mode)
        {
            if (mode == SyncModeType.LeftToRight)
            {
                return new LeftToRightSyncMode();
            }
            else if (mode == SyncModeType.LeftToRightWithoutDeletion)
            {
                return new LeftToRightWithoutDeletionSyncMode();
            }
            else if (mode == SyncModeType.BothWays)
            {
                return new BothWaysSyncMode();
            }

            throw new InvalidOperationException("Sync mode is not supported.");
        }
    }
}
