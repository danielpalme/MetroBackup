using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization.SyncModes
{
    /// <summary>
    /// Interface that all sync modes have to implement.
    /// </summary>
    public interface ISyncMode
    {
        /// <summary>
        /// Gets the name of the sync mode.
        /// </summary>
        /// <value>The name of the sync mode.</value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the reference directory should be created.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the reference directory should be created; otherwise, <c>false</c>.
        /// </value>
        bool CreateReferenceDirectory { get; }

        /// <summary>
        /// Gets a value indicating whether the target directory should be created.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the target directory should be created; otherwise, <c>false</c>.
        /// </value>
        bool CreateTargetDirectory { get; }

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a file that has been found on the reference directory.
        /// </summary>
        /// <param name="fileOnReferenceFolder">The file on reference folder.</param>
        /// <param name="fileOnTargetFolder">The file on target folder.</param>
        /// <returns>The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.</returns>
        SyncItemBase FileFoundOnReferenceDirectory(string fileOnReferenceFolder, string fileOnTargetFolder);

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a directory that has been found on the reference directory.
        /// </summary>
        /// <param name="directoryInReferenceFolder">The directory in reference folder.</param>
        /// <param name="directoryInTargetFolder">The directory in target folder.</param>
        /// <returns>The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.</returns>
        SyncItemBase DirectoryFoundOnReferenceDirectory(string directoryInReferenceFolder, string directoryInTargetFolder);

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a file that has been found on the target directory.
        /// </summary>
        /// <param name="fileOnReferenceFolder">The file on reference folder.</param>
        /// <param name="fileOnTargetFolder">The file on target folder.</param>
        /// <returns>The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.</returns>
        SyncItemBase FileFoundOnTargetDirectory(string fileOnReferenceFolder, string fileOnTargetFolder);

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a directory that has been found on the target directory.
        /// </summary>
        /// <param name="directoryInReferenceFolder">The directory in reference folder.</param>
        /// <param name="directoryInTargetFolder">The directory in target folder.</param>
        /// <returns>The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.</returns>
        SyncItemBase DirectoryFoundOnTargetDirectory(string directoryInReferenceFolder, string directoryInTargetFolder);
    }
}
