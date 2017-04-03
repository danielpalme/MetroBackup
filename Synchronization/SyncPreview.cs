using System;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Contains information about the file operations that have to be performed during sychronization.
    /// </summary>
    public class SyncPreview
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncPreview"/> class.
        /// </summary>
        public SyncPreview()
        {
            this.SyncTaskPreviewBySyncTask = new Dictionary<SyncTask, SyncTaskPreview>();
        }

        /// <summary>
        /// Gets the <see cref="SyncTaskPreview"/> by <see cref="SyncTask"/>.
        /// </summary>
        /// <value>The <see cref="SyncTaskPreview"/> by <see cref="SyncTask"/>.</value>
        public IDictionary<SyncTask, SyncTaskPreview> SyncTaskPreviewBySyncTask { get; private set; }

        /// <summary>
        /// Gets the count of active copy files.
        /// </summary>
        /// <value>The count of active copy files.</value>
        public int CountOfActiveCopyFiles
        {
            get
            {
                return this.SyncTaskPreviewBySyncTask.Sum(t => t.Value.CopyFileSyncItems.Count(i => i.IsActive));
            }
        }

        /// <summary>
        /// Gets the count of active delete files.
        /// </summary>
        /// <value>The count of active delete files.</value>
        public int CountOfActiveDeleteFiles
        {
            get
            {
                return this.SyncTaskPreviewBySyncTask.Sum(t => t.Value.DeleteFileSyncItems.Count(i => i.IsActive));
            }
        }

        /// <summary>
        /// Gets the count of active create directories.
        /// </summary>
        /// <value>The count of active create directories.</value>
        public int CountOfActiveCreateDirectories
        {
            get
            {
                return this.SyncTaskPreviewBySyncTask.Sum(t => t.Value.CreateDirectorySyncItems.Count(i => i.IsActive));
            }
        }

        /// <summary>
        /// Gets the count of active delete directories.
        /// </summary>
        /// <value>The count of active delete directories.</value>
        public int CountOfActiveDeleteDirectories
        {
            get
            {
                return this.SyncTaskPreviewBySyncTask.Sum(t => t.Value.DeleteDirectorySyncItems.Count(i => i.IsActive));
            }
        }

        /// <summary>
        /// Gets the <see cref="SyncTaskPreview"/> by the given <see cref="SyncTask"/>.
        /// </summary>
        /// <param name="syncTask">The <see cref="SyncTaskPreview"/></param>
        public SyncTaskPreview this[SyncTask syncTask]
        {
            get
            {
                return this.SyncTaskPreviewBySyncTask[syncTask];
            }
        }

        /// <summary>
        /// Adds the specified sync task.
        /// </summary>
        /// <param name="syncTask">The sync task.</param>
        /// <param name="syncItem">The sync item.</param>
        internal void Add(SyncTask syncTask, SyncItemBase syncItem)
        {
            if (syncTask == null)
            {
                throw new ArgumentNullException("syncTask");
            }

            if (syncItem == null)
            {
                throw new ArgumentNullException("syncItem");
            }

            SyncTaskPreview syncTaskPreview;
            if (this.SyncTaskPreviewBySyncTask.TryGetValue(syncTask, out syncTaskPreview))
            {
                syncTaskPreview.Add(syncItem);
            }
            else
            {
                syncTaskPreview = new SyncTaskPreview();
                syncTaskPreview.Add(syncItem);
                this.SyncTaskPreviewBySyncTask[syncTask] = syncTaskPreview;
            }
        }

        /// <summary>
        /// Removes the unnecessary sync items.
        /// </summary>
        internal void RemoveUnnecessarySyncItems()
        {
            var tasksToRemove = new List<SyncTask>();

            foreach (var task in this.SyncTaskPreviewBySyncTask)
            {
                if (string.IsNullOrEmpty(task.Key.Filter))
                {
                    continue;
                }

                SyncTaskPreview syncTaskPreview = task.Value;
                syncTaskPreview.RemoveUnnecessarySyncItems();

                if (syncTaskPreview.Count == 0)
                {
                    tasksToRemove.Add(task.Key);
                }
            }

            foreach (var syncTask in tasksToRemove)
            {
                this.SyncTaskPreviewBySyncTask.Remove(syncTask);
            }
        }
    }
}
