using System.IO;
using Palmmedia.BackUp.SharedResources;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization.SyncModes
{
    /// <summary>
    /// <see cref="ISyncMode"/> that performs synchronization from left to right without deleting any files.
    /// </summary>
    public class LeftToRightWithoutDeletionSyncMode : ISyncMode
    {
        /// <summary>
        /// Gets the name of the sync mode.
        /// </summary>
        /// <value>The name of the sync mode.</value>
        public string Name
        {
            get
            {
                return Common.SyncModeType_LeftToRightWithoutDeletion;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the reference directory should be created.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the reference directory should be created; otherwise, <c>false</c>.
        /// </value>
        public bool CreateReferenceDirectory
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the target directory should be created.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the target directory should be created; otherwise, <c>false</c>.
        /// </value>
        public bool CreateTargetDirectory
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a file that has been found on the reference directory.
        /// </summary>
        /// <param name="fileOnReferenceFolder">The file on reference folder.</param>
        /// <param name="fileOnTargetFolder">The file on target folder.</param>
        /// <returns>
        /// The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.
        /// </returns>
        public SyncItemBase FileFoundOnReferenceDirectory(string fileOnReferenceFolder, string fileOnTargetFolder)
        {
            if (!File.Exists(fileOnTargetFolder)
                || File.GetLastWriteTime(fileOnReferenceFolder) > File.GetLastWriteTime(fileOnTargetFolder).AddSeconds(1))
            {
                return new CopyFileSyncItem(fileOnReferenceFolder, fileOnTargetFolder);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a directory that has been found on the reference directory.
        /// </summary>
        /// <param name="directoryInReferenceFolder">The directory in reference folder.</param>
        /// <param name="directoryInTargetFolder">The directory in target folder.</param>
        /// <returns>
        /// The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.
        /// </returns>
        public SyncItemBase DirectoryFoundOnReferenceDirectory(string directoryInReferenceFolder, string directoryInTargetFolder)
        {
            if (!Directory.Exists(directoryInTargetFolder))
            {
                return new CreateDirectorySyncItem(directoryInTargetFolder);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a file that has been found on the target directory.
        /// </summary>
        /// <param name="fileOnReferenceFolder">The file on reference folder.</param>
        /// <param name="fileOnTargetFolder">The file on target folder.</param>
        /// <returns>
        /// The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.
        /// </returns>
        public SyncItemBase FileFoundOnTargetDirectory(string fileOnReferenceFolder, string fileOnTargetFolder)
        {
            return null;
        }

        /// <summary>
        /// Generates a <see cref="SyncItemBase"/> for a directory that has been found on the target directory.
        /// </summary>
        /// <param name="directoryInReferenceFolder">The directory in reference folder.</param>
        /// <param name="directoryInTargetFolder">The directory in target folder.</param>
        /// <returns>
        /// The <see cref="SyncItemBase"/> or <c>null</c> if no action is required.
        /// </returns>
        public SyncItemBase DirectoryFoundOnTargetDirectory(string directoryInReferenceFolder, string directoryInTargetFolder)
        {
            return null;
        }
    }
}
