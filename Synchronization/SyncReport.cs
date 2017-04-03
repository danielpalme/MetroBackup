using System;
using System.Collections.Generic;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Contains information about the file operations that have to be performed during sychronization.
    /// </summary>
    public class SyncReport
    {
        /// <summary>
        /// The files failed to copy.
        /// </summary>
        private readonly List<CopyFileSyncItem> failedCopyFiles;

        /// <summary>
        /// The files failed to delete.
        /// </summary>
        private readonly List<DeleteFileSyncItem> failedDeleteFiles;

        /// <summary>
        /// The directories failed to create.
        /// </summary>
        private readonly List<CreateDirectorySyncItem> failedCreateDirectories;

        /// <summary>
        /// The directories failed to delete.
        /// </summary>
        private readonly List<DeleteDirectorySyncItem> failedDeleteDirectories;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncReport"/> class.
        /// </summary>
        public SyncReport()
        {
            this.failedCopyFiles = new List<CopyFileSyncItem>();
            this.failedDeleteFiles = new List<DeleteFileSyncItem>();
            this.failedCreateDirectories = new List<CreateDirectorySyncItem>();
            this.failedDeleteDirectories = new List<DeleteDirectorySyncItem>();
        }

        /// <summary>
        /// Gets the number of successfully copied files.
        /// </summary>
        /// <value>The number of successfully copied files.</value>
        public int SuccessCopyFiles { get; internal set; }

        /// <summary>
        /// Gets the number of files that have to be copied.
        /// </summary>
        /// <value>The number of files that have to be copied.</value>
        public int TotalCopyFiles { get; internal set; }

        /// <summary>
        /// Gets the number of successfully deleted files.
        /// </summary>
        /// <value>The number of successfully deleted files.</value>
        public int SuccessDeleteFiles { get; internal set; }

        /// <summary>
        /// Gets the number of files that have to be deleted.
        /// </summary>
        /// <value>The number of files that have to be deleted.</value>
        public int TotalDeleteFiles { get; internal set; }

        /// <summary>
        /// Gets the number of successfully created directories.
        /// </summary>
        /// <value>The number of successfully created directories.</value>
        public int SuccessCreateDirectories { get; internal set; }

        /// <summary>
        /// Gets the number of directories that have to be created.
        /// </summary>
        /// <value>The number of directories that have to be created.</value>
        public int TotalCreateDirectories { get; internal set; }

        /// <summary>
        /// Gets the number of successfully deleted directories.
        /// </summary>
        /// <value>The number of successfully deleted directories.</value>
        public int SuccessDeleteDirectories { get; internal set; }

        /// <summary>
        /// Gets the number of directories that have to be deleted.
        /// </summary>
        /// <value>The number of directories that have to be deleted.</value>
        public int TotalDeleteDirectories { get; internal set; }

        /// <summary>
        /// Gets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public string LogFilePath { get; internal set; }

        /// <summary>
        /// Gets the files failed to copy.
        /// </summary>
        /// <value>The files failed to copy.</value>
        public IEnumerable<CopyFileSyncItem> FailedCopyFiles
        {
            get
            {
                return this.failedCopyFiles;
            }
        }

        /// <summary>
        /// Gets the files failed to delete.
        /// </summary>
        /// <value>The files failed to delete.</value>
        public IEnumerable<DeleteFileSyncItem> FailedDeleteFiles
        {
            get
            {
                return this.failedDeleteFiles;
            }
        }

        /// <summary>
        /// Gets the directories failed to create.
        /// </summary>
        /// <value>The directories failed to create.</value>
        public IEnumerable<CreateDirectorySyncItem> FailedCreateDirectories
        {
            get
            {
                return this.failedCreateDirectories;
            }
        }

        /// <summary>
        /// Gets the directories failed to delete.
        /// </summary>
        /// <value>The directories failed to delete.</value>
        public IEnumerable<DeleteDirectorySyncItem> FailedDeleteDirectories
        {
            get
            {
                return this.failedDeleteDirectories;
            }
        }

        /// <summary>
        /// Gets a value indicating whether errors have occured during synchronization.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if errors have occured during synchronization; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get
            {
                return this.failedCopyFiles.Count > 0
                || this.failedDeleteFiles.Count > 0
                || this.failedCreateDirectories.Count > 0
                || this.failedDeleteDirectories.Count > 0;
            }
        }

        /// <summary>
        /// Adds a <see cref="SyncItemBase"/> which failed.
        /// </summary>
        /// <param name="syncItem">The sync item.</param>
        internal void AddFailedSyncItem(SyncItemBase syncItem)
        {
            CopyFileSyncItem copyFileSyncItem = syncItem as CopyFileSyncItem;

            if (copyFileSyncItem != null)
            {
                this.failedCopyFiles.Add(copyFileSyncItem);
                return;
            }

            DeleteFileSyncItem deleteFileSyncItem = syncItem as DeleteFileSyncItem;

            if (deleteFileSyncItem != null)
            {
                this.failedDeleteFiles.Add(deleteFileSyncItem);
                return;
            }

            CreateDirectorySyncItem createDirectorySyncItem = syncItem as CreateDirectorySyncItem;

            if (createDirectorySyncItem != null)
            {
                this.failedCreateDirectories.Add(createDirectorySyncItem);
                return;
            }

            DeleteDirectorySyncItem deleteDirectorySyncItem = syncItem as DeleteDirectorySyncItem;

            if (deleteDirectorySyncItem != null)
            {
                this.failedDeleteDirectories.Add(deleteDirectorySyncItem);
                return;
            }

            throw new ArgumentException("SyncItem has invalid type.", "syncItem");
        }
    }
}
