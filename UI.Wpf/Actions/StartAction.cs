using System;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Shows the start screen.
    /// </summary>
    internal class StartAction : WorkflowAction, IDependsOn<StartViewModel>
    {
        #region Fields

        /// <summary>
        /// The <see cref="StartViewModel"/>.
        /// </summary>
        private StartViewModel startViewModel;

        #endregion

        /// <summary>
        /// Occurs when synchronization should be performed.
        /// </summary>
        public event Action<IEnumerable<SyncTask>> Synchronize;

        /// <summary>
        /// Injects the specified start view model.
        /// </summary>
        /// <param name="startViewModel">The start view model.</param>
        public void Inject(StartViewModel startViewModel)
        {
            this.startViewModel = startViewModel;
            this.startViewModel.SynchronizeCommand.ExecuteAction += this.OnSynchronize;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        public void Process()
        {
            this.MainViewModel.CurrentSyncView = this.startViewModel;
        }

        /// <summary>
        /// Called when synchronization should be performed.
        /// </summary>
        protected virtual void OnSynchronize()
        {
            if (this.Synchronize != null)
            {
                this.Synchronize(this.startViewModel.SelectedTasklist.Select(t => t.SyncTask));
            }
        }
    }
}
