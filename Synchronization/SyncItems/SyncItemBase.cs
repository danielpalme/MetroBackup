using System;
using System.ComponentModel;

namespace Palmmedia.BackUp.Synchronization.SyncItems
{
    /// <summary>
    /// Base class for all sync action.
    /// </summary>
    public abstract class SyncItemBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Determines whetherthis instance is active.
        /// </summary>
        private bool isActive = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncItemBase"/> class.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        protected SyncItemBase(string targetPath)
        {
            if (string.IsNullOrEmpty(targetPath))
            {
                throw new ArgumentException("The path must not be null or empty.", "targetPath");
            }

            this.SyncResult = SyncResult.NotSynced;
            this.TargetPath = targetPath;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get
            {
                return this.isActive;
            }

            set
            {
                this.isActive = value;
                this.OnPropertyChanged("IsActive");
            }
        }

        /// <summary>
        /// Gets or sets the sync result.
        /// </summary>
        /// <value>The sync result.</value>
        public SyncResult SyncResult { get; protected set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public string Error { get; protected set; }

        /// <summary>
        /// Gets or sets the reference path.
        /// </summary>
        /// <value>The reference path.</value>
        public string ReferencePath { get; protected set; }

        /// <summary>
        /// Gets or sets the target path.
        /// </summary>
        /// <value>The target path.</value>
        public string TargetPath { get; protected set; }

        /// <summary>
        /// Executes the synchronization.
        /// </summary>
        /// <returns>The <see cref="SyncResult"/>.</returns>
        internal SyncResult Synchronize()
        {
            this.Error = this.Execute();

            this.SyncResult = this.Error == null ? SyncResult.Successful : SyncResult.Error;

            return this.SyncResult;
        }

        /// <summary>
        /// Executes the synchronization.
        /// </summary>
        /// <returns>The error or <c>null</c> if no error has occured.</returns>
        protected abstract string Execute();

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}