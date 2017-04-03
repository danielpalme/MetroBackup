using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Performs the file scan and shows a view displaying the progress.
    /// </summary>
    internal class ScanAction : WorkflowAction, IDependsOn<ScanViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="ISynchronizer"/>.
        /// </summary>
        private readonly ISynchronizer synchronizer;

        /// <summary>
        /// The <see cref="ScanViewModel"/>.
        /// </summary>
        private ScanViewModel scanViewModel;

        /// <summary>
        /// Indicates whether scanning has been canceled.
        /// </summary>
        private bool canceled;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanAction"/> class.
        /// </summary>
        /// <param name="synchronizer">The <see cref="ISynchronizer"/>.</param>
        public ScanAction(ISynchronizer synchronizer)
        {
            if (synchronizer == null)
            {
                throw new ArgumentNullException("synchronizer");
            }

            this.synchronizer = synchronizer;
            this.synchronizer.DirectoryChanged += this.SynchronizerDirectoryChanged;
        }

        /// <summary>
        /// Occurs when the synchronization preview should be displayed.
        /// </summary>
        public event Action<SyncPreview> Preview;

        /// <summary>
        /// Occurs when the synchronization preview should be displayed, but no files have been found.
        /// </summary>
        public event Action PreviewNoFiles;

        /// <summary>
        /// Occurs when synchronization is canceled.
        /// </summary>
        public event Action Canceled;

        /// <summary>
        /// Injects the specified scan view model.
        /// </summary>
        /// <param name="scanViewModel">The scan view model.</param>
        public void Inject(ScanViewModel scanViewModel)
        {
            this.scanViewModel = scanViewModel;
            this.scanViewModel.CancelCommand.ExecuteAction += this.Cancel;
        }

        /// <summary>
        /// Performs the file scan and shows the view.
        /// </summary>
        /// <param name="syncTasks">The sync tasks.</param>
        public void Process(IEnumerable<SyncTask> syncTasks)
        {
            this.canceled = false;

            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                var taskbarManager = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarManager.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Indeterminate);
            }

            var syncPreviewTask = Task<SyncPreview>.Factory.StartNew(
                () => this.synchronizer.CreatePreview(syncTasks));

            syncPreviewTask.ContinueWith(
                syncPreview => this.ProcessResult(syncPreview.Result),
                TaskScheduler.FromCurrentSynchronizationContext());

            this.MainViewModel.CurrentSyncView = this.scanViewModel;
        }

        /// <summary>
        /// Called when the synchronization preview should be displayed.
        /// </summary>
        /// <param name="syncPreview">The sync preview.</param>
        protected virtual void OnPreview(SyncPreview syncPreview)
        {
            if (this.Preview != null)
            {
                this.Preview(syncPreview);
            }
        }

        /// <summary>
        /// Called when the synchronization preview should be displayed, but no files have been found.
        /// </summary>
        protected virtual void OnPreviewNoFiles()
        {
            if (this.PreviewNoFiles != null)
            {
                this.PreviewNoFiles();
            }
        }

        /// <summary>
        /// Called when synchronization is canceled.
        /// </summary>
        protected virtual void OnCanceled()
        {
            this.synchronizer.Cancel();

            if (this.Canceled != null)
            {
                this.Canceled();
            }
        }

        /// <summary>
        /// Determines which command should be performed based on the scan result.
        /// </summary>
        /// <param name="syncPreview">The sync preview.</param>
        private void ProcessResult(SyncPreview syncPreview)
        {
            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                var taskbarManager = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarManager.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
            }

            if (this.canceled)
            {
                return;
            }

            if (syncPreview.SyncTaskPreviewBySyncTask.Count == 0)
            {
                this.OnPreviewNoFiles();
            }
            else
            {
                this.OnPreview(syncPreview);
            }
        }

        /// <summary>
        /// Executed when the <see cref="ISynchronizer"/> has changed its current directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        private void SynchronizerDirectoryChanged(string directory)
        {
            this.scanViewModel.CurrentDirectory = directory;
        }

        /// <summary>
        /// Cancels the synchronization.
        /// </summary>
        private void Cancel()
        {
            this.canceled = true;
            this.synchronizer.Cancel();
            this.OnCanceled();
        }
    }
}
