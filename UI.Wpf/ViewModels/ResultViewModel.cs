using System.Collections.Generic;
using System.Windows;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.UI.Wpf.Common;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// ViewModel presenting the sychronization result.
    /// </summary>
    public class ResultViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The sync report.
        /// </summary>
        private SyncReport syncReport;

        /// <summary>
        /// the selected file list.
        /// </summary>
        private int selectedFileList;

        /// <summary>
        /// The column width.
        /// </summary>
        private double columnWidth;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultViewModel"/> class.
        /// </summary>
        public ResultViewModel()
        {
            this.OkCommand = new FlowCommand();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the sync report.
        /// </summary>
        /// <value>The sync report.</value>
        public SyncReport SyncReport
        {
            get
            {
                return this.syncReport;
            }

            set
            {
                this.syncReport = value;
                this.OnPropertyChanged("SyncReport");
                this.OnPropertyChanged("ErrorsVisibility");
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
                if (this.SelectedFileList == 0)
                {
                    return this.SyncReport.FailedCopyFiles;
                }
                else if (this.SelectedFileList == 1)
                {
                    return this.SyncReport.FailedDeleteFiles;
                }
                else if (this.SelectedFileList == 2)
                {
                    return this.SyncReport.FailedCreateDirectories;
                }
                else
                {
                    return this.SyncReport.FailedDeleteDirectories;
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
        /// Gets the errors visibility.
        /// </summary>
        /// <value>The errors visibility.</value>
        public Visibility ErrorsVisibility
        {
            get
            {
                return this.SyncReport.HasErrors ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the ok command.
        /// </summary>
        /// <value>The ok command.</value>
        public IFlowCommand OkCommand { get; private set; }

        #endregion
    }
}
