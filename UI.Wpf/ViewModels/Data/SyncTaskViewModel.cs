using System;
using System.Windows.Input;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.Synchronization.SyncModes;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.Interaction;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels.Data
{
    /// <summary>
    /// <see cref="System.ComponentModel.INotifyPropertyChanged"/> wrapper for <see cref="SyncTask"/>.
    /// </summary>
    public class SyncTaskViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="SyncTask"/>.
        /// </summary>
        private readonly SyncTask syncTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncTaskViewModel"/> class.
        /// </summary>
        /// <param name="syncTask">The sync task.</param>
        public SyncTaskViewModel(SyncTask syncTask)
        {
            if (syncTask == null)
            {
                throw new ArgumentNullException("syncTask");
            }

            this.DialogHandler = new FormDialog();
            this.ChangeReferenceDirectoryCommand = new RelayCommand(o => this.ChangeReferenceDirectory());
            this.ChangeTargetDirectoryCommand = new RelayCommand(o => this.ChangeTargetDirectory());

            this.syncTask = syncTask;
            syncTask.ReferenceDirectoryChanged += (s, e) => this.OnPropertyChanged("ReferenceDirectory");
            syncTask.TargetDirectoryChanged += (s, e) => this.OnPropertyChanged("TargetDirectory");
            syncTask.StatusChanged += (s, e) => this.OnPropertyChanged("Status");
            syncTask.LastSyncDateChanged += (s, e) => this.OnPropertyChanged("LastSyncDate");
        }

        /// <summary>
        /// Gets the sync task.
        /// </summary>
        /// <value>The sync task.</value>
        public SyncTask SyncTask
        {
            get
            {
                return this.syncTask;
            }
        }

        /// <summary>
        /// Gets or sets the name of the sync task.
        /// </summary>
        /// <value>The name of the sync task.</value>
        public string Name
        {
            get
            {
                return this.syncTask.Name;
            }

            set
            {
                this.syncTask.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get
            {
                return this.syncTask.IsActive;
            }

            set
            {
                this.syncTask.IsActive = value;
                this.OnPropertyChanged("IsActive");
            }
        }

        /// <summary>
        /// Gets or sets the type of the sync mode.
        /// </summary>
        /// <value>The type of the sync mode.</value>
        public SyncModeType SyncModeType
        {
            get
            {
                return this.syncTask.SyncModeType;
            }

            set
            {
                this.syncTask.SyncModeType = value;
                this.OnPropertyChanged("SyncModeType");
            }
        }

        /// <summary>
        /// Gets or sets the reference directory.
        /// </summary>
        /// <value>The reference directory.</value>
        public string ReferenceDirectory
        {
            get
            {
                return this.syncTask.ReferenceDirectory;
            }

            set
            {
                this.syncTask.ReferenceDirectory = value;
                this.OnPropertyChanged("ReferenceDirectory");
            }
        }

        /// <summary>
        /// Gets or sets the target directory.
        /// </summary>
        /// <value>The target directory.</value>
        public string TargetDirectory
        {
            get
            {
                return this.syncTask.TargetDirectory;
            }

            set
            {
                this.syncTask.TargetDirectory = value;
                this.OnPropertyChanged("TargetDirectory");
            }
        }

        /// <summary>
        /// Gets the current status.
        /// </summary>
        /// <value>The current status.</value>
        public string Status
        {
            get
            {
                return this.syncTask.Status;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this an error has occured.
        /// </summary>
        /// <value><c>true</c> if error has occured; otherwise, <c>false</c>.</value>
        public bool Error
        {
            get
            {
                return this.syncTask.Error;
            }
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter
        {
            get
            {
                return this.syncTask.Filter;
            }

            set
            {
                this.syncTask.Filter = value;
                this.OnPropertyChanged("Filter");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether synchronization should be performed in subdirectories.
        /// </summary>
        /// <value><c>true</c> if synchronization should be performed in subdirectories; otherwise, <c>false</c>.</value>
        public bool Recursive
        {
            get
            {
                return this.syncTask.Recursive;
            }

            set
            {
                this.syncTask.Recursive = value;
                this.OnPropertyChanged("Recursive");
            }
        }

        /// <summary>
        /// Gets the last sync date.
        /// </summary>
        /// <value>The last sync date.</value>
        public DateTime? LastSyncDate
        {
            get
            {
                return this.syncTask.LastSyncDate;
            }
        }

        /// <summary>
        /// Gets the change reference directory command.
        /// </summary>
        /// <value>The change reference directory command.</value>
        public ICommand ChangeReferenceDirectoryCommand { get; private set; }

        /// <summary>
        /// Gets the change target directory command.
        /// </summary>
        /// <value>The change target directory command.</value>
        public ICommand ChangeTargetDirectoryCommand { get; private set; }

        /// <summary>
        /// Gets or sets the dialog handler.
        /// </summary>
        /// <value>The dialog handler.</value>
        internal FormDialog DialogHandler { get; set; }

        /// <summary>
        /// Changes the reference directory.
        /// </summary>
        private void ChangeReferenceDirectory()
        {
            this.ReferenceDirectory = this.DialogHandler.GetFolder(this.ReferenceDirectory);
        }

        /// <summary>
        /// Changes the target directory.
        /// </summary>
        private void ChangeTargetDirectory()
        {
            this.TargetDirectory = this.DialogHandler.GetFolder(this.TargetDirectory);
        }
    }
}
