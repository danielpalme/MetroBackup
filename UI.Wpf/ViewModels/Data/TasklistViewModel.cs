using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels.Data
{
    /// <summary>
    /// Represents a collection of <see cref="SyncTaskViewModel"/>.
    /// </summary>
    public class TasklistViewModel : ObservableCollection<SyncTaskViewModel>
    {
        /// <summary>
        /// The name of the task list.
        /// </summary>
        private string name;

        /// <summary>
        /// The last sync date.
        /// </summary>
        private DateTime? lastSyncDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="TasklistViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the task list.</param>
        public TasklistViewModel(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TasklistViewModel"/> class.
        /// </summary>
        /// <param name="syncTasks">The sync tasks.</param>
        /// <param name="name">The name of the task list.</param>
        public TasklistViewModel(IEnumerable<SyncTaskViewModel> syncTasks, string name)
            : base(syncTasks)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the task list.
        /// </summary>
        /// <value>The name of the task list.</value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        /// <summary>
        /// Gets or sets the last sync date.
        /// </summary>
        /// <value>The last sync date.</value>
        public DateTime? LastSyncDate
        {
            get
            {
                return this.lastSyncDate;
            }

            set
            {
                this.lastSyncDate = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("LastSyncDate"));
            }
        }
    }
}
