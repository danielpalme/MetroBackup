using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Contains information about the file operations that have to be performed during sychronization within a specific <see cref="SyncTask"/>.
    /// </summary>
    public class SyncTaskPreview
    {
        /// <summary>
        /// The path separators.
        /// </summary>
        private static readonly char[] PathSeperators = new char[] { Path.PathSeparator };

        /// <summary>
        /// The files to copy.
        /// </summary>
        private readonly List<CopyFileSyncItem> copyFileSyncItems;

        /// <summary>
        /// The files to delete.
        /// </summary>
        private readonly List<DeleteFileSyncItem> deleteFileSyncItems;

        /// <summary>
        /// The directories to create.
        /// </summary>
        private readonly List<CreateDirectorySyncItem> createDirectorySyncItems;

        /// <summary>
        /// The directories to delete.
        /// </summary>
        private readonly List<DeleteDirectorySyncItem> deleteDirectorySyncItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncTaskPreview"/> class.
        /// </summary>
        public SyncTaskPreview()
        {
            this.copyFileSyncItems = new List<CopyFileSyncItem>();
            this.deleteFileSyncItems = new List<DeleteFileSyncItem>();
            this.createDirectorySyncItems = new List<CreateDirectorySyncItem>();
            this.deleteDirectorySyncItems = new List<DeleteDirectorySyncItem>();
        }

        /// <summary>
        /// Gets the number of total file operations.
        /// </summary>
        /// <value>The number of total file operations.</value>
        public int Count
        {
            get
            {
                return this.copyFileSyncItems.Count
                    + this.deleteFileSyncItems.Count
                    + this.createDirectorySyncItems.Count
                    + this.deleteDirectorySyncItems.Count;
            }
        }

        /// <summary>
        /// Gets the files that have to be copied.
        /// </summary>
        /// <value>The files that have to be copied.</value>
        public IEnumerable<CopyFileSyncItem> CopyFileSyncItems
        {
            get
            {
                return this.copyFileSyncItems;
            }
        }

        /// <summary>
        /// Gets the files that have to be deleted.
        /// </summary>
        /// <value>The files that have to be deleted</value>
        public IEnumerable<DeleteFileSyncItem> DeleteFileSyncItems
        {
            get
            {
                return this.deleteFileSyncItems;
            }
        }

        /// <summary>
        /// Gets the directories that have to be created.
        /// </summary>
        /// <value>The directories that have to be created</value>
        public IEnumerable<CreateDirectorySyncItem> CreateDirectorySyncItems
        {
            get
            {
                return this.createDirectorySyncItems;
            }
        }

        /// <summary>
        /// Gets the directories that have to be deleted.
        /// </summary>
        /// <value>The directories that have to be deleted.</value>
        public IEnumerable<DeleteDirectorySyncItem> DeleteDirectorySyncItems
        {
            get
            {
                return this.deleteDirectorySyncItems;
            }
        }

        /// <summary>
        /// Adds the specified sync item.
        /// </summary>
        /// <param name="syncItem">The sync item.</param>
        internal void Add(SyncItemBase syncItem)
        {
            CopyFileSyncItem copyFileSyncItem = syncItem as CopyFileSyncItem;

            if (copyFileSyncItem != null)
            {
                this.copyFileSyncItems.Add(copyFileSyncItem);
                return;
            }

            DeleteFileSyncItem deleteFileSyncItem = syncItem as DeleteFileSyncItem;

            if (deleteFileSyncItem != null)
            {
                this.deleteFileSyncItems.Add(deleteFileSyncItem);
                return;
            }

            CreateDirectorySyncItem createDirectorySyncItem = syncItem as CreateDirectorySyncItem;

            if (createDirectorySyncItem != null)
            {
                this.createDirectorySyncItems.Add(createDirectorySyncItem);
                return;
            }

            DeleteDirectorySyncItem deleteDirectorySyncItem = syncItem as DeleteDirectorySyncItem;

            if (deleteDirectorySyncItem != null)
            {
                this.deleteDirectorySyncItems.Add(deleteDirectorySyncItem);
                return;
            }

            throw new ArgumentException("SyncItem has invalid type.", "syncItem");
        }

        /// <summary>
        /// Removes the unnecessary sync items.
        /// </summary>
        internal void RemoveUnnecessarySyncItems()
        {
            var unnecessaryCreateDirectories = this.createDirectorySyncItems
                .Where(d => !this.DoesSyncFileExistInDirectory(d.TargetPath)).ToArray();

            foreach (var syncItem in unnecessaryCreateDirectories)
            {
                this.createDirectorySyncItems.Remove(syncItem);
            }

            // Only delete directories that are empty after synchronization
            foreach (var syncItem in this.deleteDirectorySyncItems.OrderByDescending(i => i.TargetPath.Split(PathSeperators).Length).ToArray())
            {
                if (this.DirectoryEmptyAfterSynchronization(syncItem.TargetPath))
                {
                    this.deleteDirectorySyncItems.Remove(syncItem);
                }
            }
        }

        /// <summary>
        /// Determines whether a file will be copied to the given directory.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns><c>true</c> if any file will be copied to the given directory.</returns>
        private bool DoesSyncFileExistInDirectory(string path)
        {
            path = path.TrimEnd(PathSeperators);

            return this.copyFileSyncItems.Any(i => Path.GetDirectoryName(i.TargetPath).StartsWith(path, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether the given directory will be empty after synchronization.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns><c>true</c> if directory will be empty after synchronization.</returns>
        private bool DirectoryEmptyAfterSynchronization(string path)
        {
            path = path.TrimEnd(PathSeperators);

            int countFiles = this.deleteFileSyncItems.Where(syncFile => Path.GetDirectoryName(syncFile.TargetPath).StartsWith(path, StringComparison.OrdinalIgnoreCase)).Count();
            int countDirectories = this.deleteDirectorySyncItems.Where(syncFile => syncFile.TargetPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)).Count() - 1;

            try
            {
                return countFiles == Directory.GetFiles(path).Length && countDirectories == Directory.GetDirectories(path).Length;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
