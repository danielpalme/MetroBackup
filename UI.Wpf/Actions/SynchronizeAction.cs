using System;
using System.Threading.Tasks;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Performs the synchronization and shows a view displaying the progress.
    /// </summary>
    internal class SynchronizeAction : WorkflowAction, IDependsOn<SynchronizeViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="ISynchronizer"/>.
        /// </summary>
        private readonly ISynchronizer synchronizer;

        /// <summary>
        /// The <see cref="SynchronizeViewModel"/>.
        /// </summary>
        private SynchronizeViewModel synchronizeViewModel;

        /// <summary>
        /// The number of total completed file operations.
        /// </summary>
        private int totalCompletedOperations;

        /// <summary>
        /// The number of total expected file operations.
        /// </summary>
        private int totalExpectedOperations;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeAction"/> class.
        /// </summary>
        /// <param name="synchronizer">The <see cref="ISynchronizer"/>.</param>
        public SynchronizeAction(ISynchronizer synchronizer)
        {
            if (synchronizer == null)
            {
                throw new ArgumentNullException("synchronizer");
            }

            this.synchronizer = synchronizer;
            this.synchronizer.Synchronizing += this.ShowProgress;
        }

        /// <summary>
        /// Occurs when the result should be displayed.
        /// </summary>
        public event Action<SyncReport> Result;

        /// <summary>
        /// Injects the specified synchronize view model.
        /// </summary>
        /// <param name="synchronizeViewModel">The synchronize view model.</param>
        public void Inject(SynchronizeViewModel synchronizeViewModel)
        {
            this.synchronizeViewModel = synchronizeViewModel;
            this.synchronizeViewModel.CancelCommand.ExecuteAction += this.synchronizer.Cancel;
        }

        /// <summary>
        /// Performs the synchronization and shows the view.
        /// </summary>
        /// <param name="syncPreview">The sync preview.</param>
        public void Process(SyncPreview syncPreview)
        {
            this.totalCompletedOperations = 0;
            this.totalExpectedOperations =
                syncPreview.CountOfActiveCopyFiles
                + syncPreview.CountOfActiveCreateDirectories
                + syncPreview.CountOfActiveDeleteDirectories
                + syncPreview.CountOfActiveDeleteFiles;

            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                var taskbarManager = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarManager.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
            }

            var syncReportTask = Task<SyncReport>.Factory.StartNew(
                () => this.synchronizer.Synchronize(syncPreview));

            syncReportTask.ContinueWith(
                syncReport => this.OnResult(syncReport.Result),
                TaskScheduler.FromCurrentSynchronizationContext());

            this.MainViewModel.CurrentSyncView = this.synchronizeViewModel;
        }

        /// <summary>
        /// Called when the result should be displayed.
        /// </summary>
        /// <param name="syncReport">The sync report.</param>
        protected virtual void OnResult(SyncReport syncReport)
        {
            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                var taskbarManager = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarManager.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
            }

            if (this.Result != null)
            {
                this.Result(syncReport);
            }
        }

        /// <summary>
        /// Shows the progress of the synchronization.
        /// </summary>
        /// <param name="file">The current file.</param>
        private void ShowProgress(string file)
        {
            this.synchronizeViewModel.CurrentFile = file;

            this.totalCompletedOperations++;
            this.synchronizeViewModel.Progress = 100 * ((double)this.totalCompletedOperations / this.totalExpectedOperations);

            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                var taskbarManager = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                taskbarManager.SetProgressValue(this.totalCompletedOperations, this.totalExpectedOperations);
            }
        }
    }
}
