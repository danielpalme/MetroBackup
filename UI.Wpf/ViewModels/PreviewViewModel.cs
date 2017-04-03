using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// ViewModel presenting the synchronization preview.
    /// </summary>
    public class PreviewViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The sync preview.
        /// </summary>
        private SyncPreview syncPreview;

        /// <summary>
        /// The selected sync task.
        /// </summary>
        private SyncTask selectectedSyncTask;

        /// <summary>
        /// The selected file list.
        /// </summary>
        private int selectedFileList;

        /// <summary>
        /// The column width.
        /// </summary>
        private double columnWidth;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewViewModel"/> class.
        /// </summary>
        public PreviewViewModel()
        {
            this.SynchronizeCommand = new FlowCommand();
            this.CancelCommand = new FlowCommand();
            this.SelectAllCommand = new RelayCommand(o => this.SelectAll());
            this.UnselectAllCommand = new RelayCommand(o => this.UnselectAll());
        }

        #region Properties

        /// <summary>
        /// Gets or sets the sync preview.
        /// </summary>
        /// <value>The sync preview.</value>
        public SyncPreview SyncPreview
        {
            get
            {
                return this.syncPreview;
            }

            set
            {
                this.syncPreview = value;
                this.SelectectedSyncTask = this.syncPreview.SyncTaskPreviewBySyncTask.Keys.FirstOrDefault();
                this.OnPropertyChanged("SyncPreview");
                this.OnPropertyChanged("SyncTasks");
            }
        }

        /// <summary>
        /// Gets the sync tasks.
        /// </summary>
        /// <value>The sync tasks.</value>
        public IEnumerable<SyncTask> SyncTasks
        {
            get
            {
                return this.syncPreview.SyncTaskPreviewBySyncTask.Keys;
            }
        }

        /// <summary>
        /// Gets or sets the selectected sync task.
        /// </summary>
        /// <value>The selectected sync task.</value>
        public SyncTask SelectectedSyncTask
        {
            get
            {
                return this.selectectedSyncTask;
            }

            set
            {
                this.selectectedSyncTask = value;
                this.OnPropertyChanged("SelectectedSyncTask");
                this.OnPropertyChanged("SelectedFiles");
            }
        }

        /// <summary>
        /// Gets or sets the selected file list.
        /// </summary>
        /// <value>The selected file list.</value>
        public int SelectedFileList
        {
            get
            {
                return this.selectedFileList;
            }

            set
            {
                this.selectedFileList = value;
                this.OnPropertyChanged("SelectedFileList");
                this.OnPropertyChanged("SelectedFiles");

                // Resize columns, there is no cleaner way to do this
                this.ColumnWidth = 100;
                this.ColumnWidth = -1;
            }
        }

        /// <summary>
        /// Gets the selected files.
        /// </summary>
        /// <value>The selected files.</value>
        public IEnumerable<Palmmedia.BackUp.Synchronization.SyncItems.SyncItemBase> SelectedFiles
        {
            get
            {
                var syncTaskPreview = this.syncPreview.SyncTaskPreviewBySyncTask[this.SelectectedSyncTask];
                if (this.SelectedFileList == 0)
                {
                    return syncTaskPreview.CopyFileSyncItems;
                }
                else if (this.SelectedFileList == 1)
                {
                    return syncTaskPreview.DeleteFileSyncItems;
                }
                else if (this.SelectedFileList == 2)
                {
                    return syncTaskPreview.CreateDirectorySyncItems;
                }
                else
                {
                    return syncTaskPreview.DeleteDirectorySyncItems;
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width of the column.</value>
        public double ColumnWidth
        {
            get
            {
                return this.columnWidth;
            }

            set
            {
                this.columnWidth = value;
                this.OnPropertyChanged("ColumnWidth");
            }
        }

        /// <summary>
        /// Gets the synchronize command.
        /// </summary>
        /// <value>The synchronize command.</value>
        public IFlowCommand SynchronizeCommand { get; private set; }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public IFlowCommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets the select all command.
        /// </summary>
        /// <value>The select all command.</value>
        public ICommand SelectAllCommand { get; private set; }

        /// <summary>
        /// Gets the unselect all command.
        /// </summary>
        /// <value>The unselect all command.</value>
        public ICommand UnselectAllCommand { get; private set; }

        #endregion

        /// <summary>
        /// Selects all currently displayed items.
        /// </summary>
        private void SelectAll()
        {
            foreach (var syncItem in this.SelectedFiles)
            {
                syncItem.IsActive = true;
            }
        }

        /// <summary>
        /// Unselects all currently displayed items.
        /// </summary>
        private void UnselectAll()
        {
            foreach (var syncItem in this.SelectedFiles)
            {
                syncItem.IsActive = false;
            }
        }
    }
}
