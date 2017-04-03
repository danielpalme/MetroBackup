using System;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Shows a view indicating that no files have been found for synchronization.
    /// </summary>
    internal class PreviewNoFilesAction : WorkflowAction, IDependsOn<PreviewNoFilesViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="PreviewNoFilesViewModel"/>.
        /// </summary>
        private PreviewNoFilesViewModel previewNoFilesViewModel;

        #endregion

        /// <summary>
        /// Occurs when next view should be displayed.
        /// </summary>
        public event Action Ok;

        /// <summary>
        /// Injects the specified preview no files view model.
        /// </summary>
        /// <param name="previewNoFilesViewModel">The preview no files view model.</param>
        public void Inject(PreviewNoFilesViewModel previewNoFilesViewModel)
        {
            this.previewNoFilesViewModel = previewNoFilesViewModel;
            this.previewNoFilesViewModel.OkCommand.ExecuteAction += this.OnOk;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void Process()
        {
            this.MainViewModel.CurrentSyncView = this.previewNoFilesViewModel;
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
