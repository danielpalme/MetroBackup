using System;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Shows a view displaying a preview of the synchronization.
    /// </summary>
    internal class PreviewAction : WorkflowAction, IDependsOn<PreviewViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="PreviewViewModel"/>
        /// </summary>
        private PreviewViewModel previewViewModel;

        #endregion

        /// <summary>
        /// Occurs when synchronization should be performed.
        /// </summary>
        public event Action<SyncPreview> Synchronize;

        /// <summary>
        /// Occurs when synchronization should be performed but no files are selected for synchronization.
        /// </summary>
        public event Action SynchronizeNoFiles;

        /// <summary>
        /// Occurs when synchronization is canceled.
        /// </summary>
        public event Action Canceled;

        /// <summary>
        /// Injects the specified preview view model.
        /// </summary>
        /// <param name="previewViewModel">The preview view model.</param>
        public void Inject(PreviewViewModel previewViewModel)
        {
            this.previewViewModel = previewViewModel;
            this.previewViewModel.SynchronizeCommand.ExecuteAction += this.ProcessResult;
            this.previewViewModel.CancelCommand.ExecuteAction += this.OnCanceled;
        }

        /// <summary>
        /// Displays the given <see cref="SyncPreview"/>.
        /// </summary>
        /// <param name="syncPreview">The <see cref="SyncPreview"/>.</param>
        public void Process(SyncPreview syncPreview)
        {
            this.previewViewModel.SyncPreview = syncPreview;
            this.previewViewModel.SelectedFileList = 0;
            this.MainViewModel.CurrentSyncView = this.previewViewModel;
        }

        /// <summary>
        /// Called when synchronization should be performed.
        /// </summary>
        /// <param name="syncPreview">The sync preview.</param>
        protected virtual void OnSynchronize(SyncPreview syncPreview)
        {
            if (this.Synchronize != null)
            {
                this.Synchronize(syncPreview);
            }
        }

        /// <summary>
        /// Called when synchronization should be performed but no files are selected for synchronization.
        /// </summary>
        protected virtual void OnSynchronizeNoFiles()
        {
            if (this.SynchronizeNoFiles != null)
            {
                this.SynchronizeNoFiles();
            }
        }

        /// <summary>
        /// Called when synchronization is canceled.
        /// </summary>
        protected virtual void OnCanceled()
        {
            if (this.Canceled != null)
            {
                this.Canceled();
            }
        }

        /// <summary>
        /// Determines which command should be performed based on the file selection.
        /// </summary>
        private void ProcessResult()
        {
            var syncPreview = this.previewViewModel.SyncPreview;

            if (syncPreview.CountOfActiveCopyFiles == 0
                && syncPreview.CountOfActiveCreateDirectories == 0
                && syncPreview.CountOfActiveDeleteDirectories == 0
                && syncPreview.CountOfActiveDeleteFiles == 0)
            {
                this.OnSynchronizeNoFiles();
            }
            else
            {
                this.OnSynchronize(syncPreview);
            }
        }
    }
}
