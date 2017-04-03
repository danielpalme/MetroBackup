using System;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Shows a view indicating that no files have been selected for synchronization.
    /// </summary>
    internal class SynchronizeNoFilesAction : WorkflowAction, IDependsOn<SynchronizeNoFilesViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="SynchronizeNoFilesViewModel"/>.
        /// </summary>
        private SynchronizeNoFilesViewModel synchronizeNoFilesViewModel;

        #endregion

        /// <summary>
        /// Occurs when next view should be displayed.
        /// </summary>
        public event Action Ok;

        /// <summary>
        /// Injects the specified synchronize no files view model.
        /// </summary>
        /// <param name="synchronizeNoFilesViewModel">The synchronize no files view model.</param>
        public void Inject(SynchronizeNoFilesViewModel synchronizeNoFilesViewModel)
        {
            this.synchronizeNoFilesViewModel = synchronizeNoFilesViewModel;
            this.synchronizeNoFilesViewModel.OkCommand.ExecuteAction += this.OnOk;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void Process()
        {
            this.MainViewModel.CurrentSyncView = this.synchronizeNoFilesViewModel;
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
