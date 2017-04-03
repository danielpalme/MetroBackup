using System;
using System.Collections.Generic;

namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Interface for sychronization.
    /// </summary>
    public interface ISynchronizer
    {
        /// <summary>
        /// Occurs when the scan directory has changed.
        /// </summary>
        event Action<string> DirectoryChanged;

        /// <summary>
        /// Occurs when when an <see cref="Palmmedia.BackUp.Synchronization.SyncItems.SyncItemBase"/> is processed during synchronization.
        /// </summary>
        event Action<string> Synchronizing;

        /// <summary>
        /// Creates a preview of the sychronization
        /// </summary>
        /// <param name="syncTasks">The sync tasks.</param>
        /// <returns>The sync preview.</returns>
        SyncPreview CreatePreview(IEnumerable<SyncTask> syncTasks);

        /// <summary>
        /// Performs synchronization of the given sync preview.
        /// </summary>
        /// <param name="syncPreview">The sync preview.</param>
        /// <returns>The sync report.</returns>
        SyncReport Synchronize(SyncPreview syncPreview);

        /// <summary>
        /// Cancels the synchronization.
        /// </summary>
        void Cancel();
    }
}
