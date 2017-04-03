using System;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Shows a view displaying the synchronization results.
    /// </summary>
    internal class ResultAction : WorkflowAction, IDependsOn<StartViewModel>, IDependsOn<ResultViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="StartViewModel"/>.
        /// </summary>
        private StartViewModel startViewModel;

        /// <summary>
        /// The <see cref="ResultViewModel"/>.
        /// </summary>
        private ResultViewModel resultViewModel;

        #endregion

        /// <summary>
        /// Occurs when next view should be displayed.
        /// </summary>
        public event Action Ok;

        /// <summary>
        /// Injects the specified start view model.
        /// </summary>
        /// <param name="startViewModel">The start view model.</param>
        public void Inject(StartViewModel startViewModel)
        {
            this.startViewModel = startViewModel;
        }

        /// <summary>
        /// Injects the specified result view model.
        /// </summary>
        /// <param name="resultViewModel">The result view model.</param>
        public void Inject(ResultViewModel resultViewModel)
        {
            this.resultViewModel = resultViewModel;
            this.resultViewModel.OkCommand.ExecuteAction += this.OnOk;
        }

        /// <summary>
        /// Displays the given <see cref="SyncReport"/>.
        /// </summary>
        /// <param name="syncReport">The <see cref="SyncReport"/>.</param>
        public void Process(SyncReport syncReport)
        {
            this.resultViewModel.SyncReport = syncReport;
            this.resultViewModel.SelectedFileList = 0;
            this.startViewModel.SelectedTasklist.LastSyncDate = DateTime.Now;
            this.MainViewModel.CurrentSyncView = this.resultViewModel;
        }

        /// <summary>
        /// Called when next view should be displayed.
        /// </summary>
        protected virtual void OnOk()
        {
            if (this.Ok != null)
            {
                this.Ok();
            }
        }
    }
}
