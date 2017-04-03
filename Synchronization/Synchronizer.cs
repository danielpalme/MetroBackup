using System;
using System.Collections.Generic;
using System.IO;
using Palmmedia.BackUp.Synchronization.FileSearch;
using Palmmedia.BackUp.Synchronization.Logging;
using Palmmedia.BackUp.Synchronization.Properties;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Implementation of <see cref="ISynchronizer"/>
    /// </summary>
    public class Synchronizer : ISynchronizer
    {
        /// <summary>
        /// The <see cref="FileSearcher"/>.
        /// </summary>
        private readonly FileSearcher fileSearch = new FileSearcher();

        /// <summary>
        /// The logger used for logging synchronization events.
        /// </summary>
        private readonly ILogger syncLogger;

        /// <summary>
        /// Determines whether synchronization was canceled.
        /// </summary>
        private bool canceled;

        /// <summary>
        /// Initializes a new instance of the <see cref="Synchronizer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Synchronizer(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.syncLogger = logger;
        }

        /// <summary>
        /// Occurs when the scan directory has changed.
        /// </summary>
        public event Action<string> DirectoryChanged;

        /// <summary>
        /// Occurs when when an <see cref="SyncItemBase"/> is processed during synchronization.
        /// </summary>
        public event Action<string> Synchronizing;

        /// <summary>
        /// Creates a preview of the sychronization
        /// </summary>
        /// <param name="syncTasks">The sync tasks.</param>
        /// <returns>The sync preview.</returns>
        public SyncPreview CreatePreview(IEnumerable<SyncTask> syncTasks)
        {
            if (syncTasks == null)
            {
                throw new ArgumentNullException("syncTasks");
            }

            this.canceled = false;

            SyncPreview syncPreview = new SyncPreview();

            foreach (var syncTask in syncTasks)
            {
                if (syncTask.Status != null)
                {
                    syncTask.Status = null;
                }

                syncTask.Error = false;
            }

            foreach (var syncTask in syncTasks)
            {
                if (!syncTask.IsActive)
                {
                    continue;
                }

                syncTask.Status = Resources.SyncStatusValidatingDirectories;
                if (!syncTask.ValidateDirectories())
                {
                    continue;
                }

                syncTask.Status = Resources.SyncStatusScanningDirectories;
                string filterRegex = syncTask.BuildFilterRegex();

                // Search reference directory
                Action<string> fileFoundOnReferenceDirectoryHandler = file => FileFoundOnReferenceDirectory(file, syncTask, syncPreview);
                Action<string> directoryFoundOnReferenceDirectoryHandler = directory => DirectoryFoundOnReferenceDirectory(directory, syncTask, syncPreview);

                this.fileSearch.FileFound += fileFoundOnReferenceDirectoryHandler;
                this.fileSearch.DirectoryFound += directoryFoundOnReferenceDirectoryHandler;
                this.fileSearch.DirectoryChanged += this.OnDirectoryChanged;

                this.fileSearch.Search(syncTask.ReferenceDirectory, filterRegex, string.Empty, syncTask.Recursive);

                this.fileSearch.FileFound -= fileFoundOnReferenceDirectoryHandler;
                this.fileSearch.DirectoryFound -= directoryFoundOnReferenceDirectoryHandler;

                // Search target directory
                Action<string> fileFoundOnTargetDirectoryHandler = file => FileFoundOnTargetDirectory(file, syncTask, syncPreview);
                Action<string> directoryFoundOnTargetDirectoryHandler = directory => DirectoryFoundOnTargetDirectory(directory, syncTask, syncPreview);

                this.fileSearch.FileFound += fileFoundOnTargetDirectoryHandler;
                this.fileSearch.DirectoryFound += directoryFoundOnTargetDirectoryHandler;

                this.fileSearch.Search(syncTask.TargetDirectory, filterRegex, string.Empty, syncTask.Recursive);

                this.fileSearch.FileFound -= fileFoundOnTargetDirectoryHandler;
                this.fileSearch.DirectoryFound -= directoryFoundOnTargetDirectoryHandler;
                this.fileSearch.DirectoryChanged -= this.OnDirectoryChanged;

                syncTask.Status = null;
            }

            syncPreview.RemoveUnnecessarySyncItems();

            return syncPreview;
        }

        /// <summary>
        /// Performs synchronization of the given sync preview.
        /// </summary>
        /// <param name="syncPreview">The sync preview.</param>
        /// <returns>The sync report.</returns>
        public SyncReport Synchronize(SyncPreview syncPreview)
        {
            if (syncPreview == null)
            {
                throw new ArgumentNullException("syncPreview");
            }

            this.canceled = false;

            SyncReport syncReport = new SyncReport();
            syncReport.TotalCopyFiles = syncPreview.CountOfActiveCopyFiles;
            syncReport.TotalDeleteFiles = syncPreview.CountOfActiveDeleteFiles;
            syncReport.TotalCreateDirectories = syncPreview.CountOfActiveCreateDirectories;
            syncReport.TotalDeleteDirectories = syncPreview.CountOfActiveDeleteDirectories;

            foreach (var task in syncPreview.SyncTaskPreviewBySyncTask)
            {
                if (this.canceled)
                {
                    break;
                }

                SyncTask syncTask = task.Key;
                SyncTaskPreview syncTaskPreview = task.Value;

                syncTask.Status = Resources.SyncStatusDeletingFiles;
                this.syncLogger.Log(syncTask.ToString());

                foreach (var syncItem in syncTaskPreview.DeleteFileSyncItems)
                {
                    if (this.canceled)
                    {
                        break;
                    }

                    if (!syncItem.IsActive)
                    {
                        continue;
                    }

                    this.OnSynchronizing(syncItem.TargetPath);
                    if (syncItem.Synchronize() == SyncResult.Error)
                    {
                        syncReport.AddFailedSyncItem(syncItem);
                    }
                    else
                    {
                        syncReport.SuccessDeleteFiles++;
                    }

                    this.syncLogger.Log(syncItem.ToString());
                }

                syncTask.Status = Resources.SyncStatusDeletingDirectories;

                foreach (var syncItem in syncTaskPreview.DeleteDirectorySyncItems)
                {
                    if (this.canceled)
                    {
                        break;
                    }

                    if (!syncItem.IsActive)
                    {
                        continue;
                    }

                    this.OnSynchronizing(syncItem.TargetPath);
                    if (syncItem.Synchronize() == SyncResult.Error)
                    {
                        syncReport.AddFailedSyncItem(syncItem);
                    }
                    else
                    {
                        syncReport.SuccessDeleteDirectories++;
                    }

                    this.syncLogger.Log(syncItem.ToString());
                }

                syncTask.Status = Resources.SyncStatusCreatingDirectories;

                foreach (var syncItem in syncTaskPreview.CreateDirectorySyncItems)
                {
                    if (this.canceled)
                    {
                        break;
                    }

                    if (!syncItem.IsActive)
                    {
                        continue;
                    }

                    this.OnSynchronizing(syncItem.TargetPath);
                    if (syncItem.Synchronize() == SyncResult.Error)
                    {
                        syncReport.AddFailedSyncItem(syncItem);
                    }
                    else
                    {
                        syncReport.SuccessCreateDirectories++;
                    }

                    this.syncLogger.Log(syncItem.ToString());
                }

                syncTask.Status = Resources.SyncStatusCopyingFiles;

                foreach (var syncItem in syncTaskPreview.CopyFileSyncItems)
                {
                    if (this.canceled)
                    {
                        break;
                    }

                    if (!syncItem.IsActive)
                    {
                        continue;
                    }

                    this.OnSynchronizing(syncItem.TargetPath);
                    if (syncItem.Synchronize() == SyncResult.Error)
                    {
                        syncReport.AddFailedSyncItem(syncItem);
                    }
                    else
                    {
                        syncReport.SuccessCopyFiles++;
                    }

                    this.syncLogger.Log(syncItem.ToString());
                }

                syncTask.Status = null;

                if (!this.canceled)
                {
                    syncTask.LastSyncDate = DateTime.Now;
                }
            }

            syncReport.LogFilePath = this.syncLogger.WriteToFile();
            return syncReport;
        }

        /// <summary>
        /// Cancels the synchronization.
        /// </summary>
        public void Cancel()
        {
            this.fileSearch.StopSearch();

            this.canceled = true;
        }

        /// <summary>
        /// Called when the scan directory has changed.
        /// </summary>
        /// <param name="directory">The directory.</param>
        protected virtual void OnDirectoryChanged(string directory)
        {
            if (this.DirectoryChanged != null)
            {
                this.DirectoryChanged(directory);
            }
        }

        /// <summary>
        /// Called when when an <see cref="SyncItemBase"/> is processed during synchronization.
        /// </summary>
        /// <param name="directory">The directory.</param>
        protected virtual void OnSynchronizing(string directory)
        {
            if (this.Synchronizing != null)
            {
                this.Synchronizing(directory);
            }
        }

        /// <summary>
        /// Executed when a file was found on reference directory.
        /// </summary>
        /// <param name="file">The found file.</param>
        /// <param name="syncTask">The sync task.</param>
        /// <param name="syncPreview">The sync preview.</param>
        private static void FileFoundOnReferenceDirectory(string file, SyncTask syncTask, SyncPreview syncPreview)
        {
            string relativePath = file.Substring(syncTask.ReferenceDirectory.Length);
            string targetFile = Path.Combine(syncTask.TargetDirectory, relativePath);

            SyncItemBase syncItem = syncTask.SyncMode.FileFoundOnReferenceDirectory(file, targetFile);

            if (syncItem != null)
            {
                syncPreview.Add(syncTask, syncItem);
            }
        }

        /// <summary>
        /// Executed when a directory was found on reference directory.
        /// </summary>
        /// <param name="directory">The found directory.</param>
        /// <param name="syncTask">The sync task.</param>
        /// <param name="syncPreview">The sync preview.</param>
        private static void DirectoryFoundOnReferenceDirectory(string directory, SyncTask syncTask, SyncPreview syncPreview)
        {
            string relativePath = directory.Substring(syncTask.ReferenceDirectory.Length);
            string targetDirectory = Path.Combine(syncTask.TargetDirectory, relativePath);

            SyncItemBase syncItem = syncTask.SyncMode.DirectoryFoundOnReferenceDirectory(directory, targetDirectory);

            if (syncItem != null)
            {
                syncPreview.Add(syncTask, syncItem);
            }
        }

        /// <summary>
        /// Executed when a file was found on target directory.
        /// </summary>
        /// <param name="file">The found file.</param>
        /// <param name="syncTask">The sync task.</param>
        /// <param name="syncPreview">The sync preview.</param>
        private static void FileFoundOnTargetDirectory(string file, SyncTask syncTask, SyncPreview syncPreview)
        {
            string relativePath = file.Substring(syncTask.TargetDirectory.Length);
            string targetFile = Path.Combine(syncTask.ReferenceDirectory, relativePath);

            SyncItemBase syncItem = syncTask.SyncMode.FileFoundOnTargetDirectory(targetFile, file);

            if (syncItem != null)
            {
                syncPreview.Add(syncTask, syncItem);
            }
        }

        /// <summary>
        /// Executed when a directory was found on target directory.
        /// </summary>
        /// <param name="directory">The found directory.</param>
        /// <param name="syncTask">The sync task.</param>
        /// <param name="syncPreview">The sync preview.</param>
        private static void DirectoryFoundOnTargetDirectory(string directory, SyncTask syncTask, SyncPreview syncPreview)
        {
            string relativePath = directory.Substring(syncTask.TargetDirectory.Length);
            string targetDirectory = Path.Combine(syncTask.ReferenceDirectory, relativePath);

            SyncItemBase syncItem = syncTask.SyncMode.DirectoryFoundOnTargetDirectory(targetDirectory, directory);

            if (syncItem != null)
            {
                syncPreview.Add(syncTask, syncItem);
            }
        }
    }
}
