using System;
using System.Globalization;
using System.IO;
using System.Text;
using Palmmedia.BackUp.Synchronization.FileSearch;
using Palmmedia.BackUp.Synchronization.Properties;
using Palmmedia.BackUp.Synchronization.SyncModes;

namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Represents a sync task.
    /// </summary>
    public class SyncTask
    {
        /// <summary>
        /// The reference directory.
        /// </summary>
        private string referenceDirectory = string.Empty;

        /// <summary>
        /// The target directory.
        /// </summary>
        private string targetDirectory = string.Empty;

        /// <summary>
        /// The sync mode type.
        /// </summary>
        private SyncModeType syncModeType;

        /// <summary>
        /// The current status.
        /// </summary>
        private string status;

        /// <summary>
        /// The last sync date.
        /// </summary>
        private DateTime? lastSyncDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncTask"/> class.
        /// </summary>
        /// <param name="name">The name of the sync task.</param>
        public SyncTask(string name)
        {
            this.SyncModeType = SyncModeType.LeftToRight;
            this.IsActive = true;
            this.Recursive = true;
            this.Name = name;
            this.Filter = string.Empty;
        }

        /// <summary>
        /// Occurs when the reference directory changed.
        /// </summary>
        public event EventHandler<EventArgs> ReferenceDirectoryChanged;

        /// <summary>
        /// Occurs when target directory changed.
        /// </summary>
        public event EventHandler<EventArgs> TargetDirectoryChanged;

        /// <summary>
        /// Occurs when status changed.
        /// </summary>
        public event EventHandler<EventArgs> StatusChanged;

        /// <summary>
        /// Occurs when last sync date changed.
        /// </summary>
        public event EventHandler<EventArgs> LastSyncDateChanged;

        /// <summary>
        /// Gets or sets the name of the sync task
        /// </summary>
        /// <value>The name of the sync task</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sync task is active.
        /// </summary>
        /// <value><c>true</c> if whether the sync task is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the reference directory.
        /// </summary>
        /// <value>The reference directory.</value>
        public string ReferenceDirectory
        {
            get
            {
                return this.referenceDirectory;
            }

            set
            {
                this.referenceDirectory = value;
                this.OnReferenceDirectoryChanged(EventArgs.Empty);
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
                return this.targetDirectory;
            }

            set
            {
                this.targetDirectory = value;
                this.OnTargetDirectoryChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether synchronization should be performed in subdirectories.
        /// </summary>
        /// <value><c>true</c> if synchronization should be performed in subdirectories; otherwise, <c>false</c>.</value>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets the sync mode.
        /// </summary>
        /// <value>The sync mode.</value>
        public ISyncMode SyncMode { get; private set; }

        /// <summary>
        /// Gets or sets the type of the sync mode.
        /// </summary>
        /// <value>The type of the sync mode.</value>
        public SyncModeType SyncModeType
        {
            get
            {
                return this.syncModeType;
            }

            set
            {
                this.syncModeType = value;
                this.SyncMode = SyncModeFactory.Create(value);
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
                this.OnLastSyncDateChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this an error has occured.
        /// </summary>
        /// <value><c>true</c> if error has occured; otherwise, <c>false</c>.</value>
        public bool Error { get; internal set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get
            {
                return this.status;
            }

            internal set
            {
                this.status = value;
                this.OnStatusChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                Resources.LoggerSyncTask,
                this.Name,
                this.ReferenceDirectory,
                this.TargetDirectory,
                this.Filter,
                this.SyncMode.ToString(),
                this.Recursive);
        }

        /// <summary>
        /// Validates the directories.
        /// </summary>
        /// <returns><c>true</c> if directories are valid; otherwise <c>false</c>.</returns>
        internal bool ValidateDirectories()
        {
            if (string.IsNullOrEmpty(this.ReferenceDirectory))
            {
                this.Error = true;
                this.Status = Resources.ErrorNoReferenceDirectory;
                return false;
            }

            if (string.IsNullOrEmpty(this.TargetDirectory))
            {
                this.Error = true;
                this.Status = Resources.ErrorNoTargetDirectory;
                return false;
            }

            // Resolve ReparsePoints
            try
            {
                this.ReferenceDirectory = ReparsePoint.GetTargetDirectory(new DirectoryInfo(this.ReferenceDirectory));
            }
            catch (Exception)
            {
            }

            try
            {
                this.TargetDirectory = ReparsePoint.GetTargetDirectory(new DirectoryInfo(this.TargetDirectory));
            }
            catch (Exception)
            {
            }

            // Normalize paths
            if (!this.ReferenceDirectory.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                this.ReferenceDirectory += Path.DirectorySeparatorChar.ToString();
            }

            if (!this.TargetDirectory.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                this.TargetDirectory += Path.DirectorySeparatorChar.ToString();
            }

            // Compare directories
            if (this.ReferenceDirectory.Equals(this.TargetDirectory, StringComparison.OrdinalIgnoreCase))
            {
                this.Error = true;
                this.Status = Resources.ErrorReferenceDirectoryEqualsTargetDirectory;
                return false;
            }

            // Create reference directory
            if (!Directory.Exists(this.ReferenceDirectory))
            {
                if (this.SyncMode.CreateReferenceDirectory)
                {
                    try
                    {
                        Directory.CreateDirectory(this.ReferenceDirectory);
                    }
                    catch (Exception)
                    {
                        this.Error = true;
                        this.Status = Resources.ErrorReferenceDirectoryCouldNotBeCreated;
                        return false;
                    }
                }
                else
                {
                    this.Error = true;
                    this.Status = Resources.ErrorReferenceDirectoryDoesNotExist;
                    return false;
                }
            }

            // Create target directory
            if (!Directory.Exists(this.TargetDirectory))
            {
                if (this.SyncMode.CreateTargetDirectory)
                {
                    try
                    {
                        Directory.CreateDirectory(this.TargetDirectory);
                    }
                    catch (Exception)
                    {
                        this.Error = true;
                        this.Status = Resources.ErrorTargetDirectoryCouldNotBeCreated;
                        return false;
                    }
                }
                else
                {
                    this.Error = true;
                    this.Status = Resources.ErrorTargetDirectoryDoesNotExist;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Builds the filter regex.
        /// </summary>
        /// <returns>The filter regex.</returns>
        internal string BuildFilterRegex()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                return ".*";
            }
            else
            {
                StringBuilder sb = new StringBuilder("\\.(");

                string[] filters = this.Filter.Replace(" ", string.Empty).Replace(".", string.Empty).Replace("*", string.Empty).Split(',');

                for (int i = 0; i < filters.Length; i++)
                {
                    sb.Append(filters[i]);
                    if (i < filters.Length - 1)
                    {
                        sb.Append("|");
                    }
                }

                sb.Append(")$");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ReferenceDirectoryChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnReferenceDirectoryChanged(EventArgs e)
        {
            if (this.ReferenceDirectoryChanged != null)
            {
                this.ReferenceDirectoryChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TargetDirectoryChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTargetDirectoryChanged(EventArgs e)
        {
            if (this.TargetDirectoryChanged != null)
            {
                this.TargetDirectoryChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:LastSyncDateChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLastSyncDateChanged(EventArgs e)
        {
            if (this.LastSyncDateChanged != null)
            {
                this.LastSyncDateChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:StatusChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnStatusChanged(EventArgs e)
        {
            if (this.StatusChanged != null)
            {
                this.StatusChanged(this, e);
            }
        }
    }
}
