using System;
using System.Text;
using Palmmedia.BackUp.SharedResources;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.Synchronization.Logging;
using Palmmedia.BackUp.UI.Console.Properties;

namespace Palmmedia.BackUp.UI.Console
{
    /// <summary>
    /// Encapsulates the synchronizing process.
    /// </summary>
    internal class ConsoleSynchronizer
    {
        /// <summary>
        /// The length of the last message displayed in console.
        /// </summary>
        private int lengthOfLastMessage = 0;

        /// <summary>
        /// Used to save the current position of the cursor within the console.
        /// </summary>
        private int currentCursorPosition;

        /// <summary>
        /// Determines whether a first message has been displayed.
        /// </summary>
        private bool firstStatusReceived;

        /// <summary>
        /// Synchronizes the given <see cref="SyncTask"/>.
        /// </summary>
        /// <param name="syncTask">The <see cref="SyncTask"/>.</param>
        /// <returns><c>true</c> if synchronization was successful.</returns>
        public bool Synchronize(SyncTask syncTask)
        {
            // Print parameters
            System.Console.WriteLine();
            System.Console.WriteLine(Resources.Commands + ":");
            System.Console.WriteLine("   " + Resources.SyncMode + ": " + syncTask.SyncMode.Name);
            System.Console.WriteLine("   " + Common.ReferenceDirectory + ": " + syncTask.ReferenceDirectory);
            System.Console.WriteLine("   " + Common.TargetDirectory + ": " + syncTask.TargetDirectory);
            System.Console.WriteLine("   " + Resources.Recursion + ": " + (syncTask.Recursive ? Resources.Yes : Resources.No));
            System.Console.WriteLine("   " + Common.Filter + ": " + syncTask.Filter);
            System.Console.WriteLine();

            Synchronizer synchronizer = new Synchronizer(new FileLogger());

            // Create Preview
            synchronizer.DirectoryChanged += this.WriteConsoleMessage;

            var statusChangedEventHandler = new EventHandler<EventArgs>((s, e) => this.NewSyncPhase(syncTask));
            syncTask.StatusChanged += statusChangedEventHandler;

            var syncPreview = synchronizer.CreatePreview(new SyncTask[] { syncTask });

            synchronizer.DirectoryChanged -= this.WriteConsoleMessage;

            if (syncTask.Error)
            {
                return false;
            }

            // Synchronize
            synchronizer.Synchronizing += this.WriteConsoleMessage;

            var syncReport = synchronizer.Synchronize(syncPreview);

            synchronizer.Synchronizing -= this.WriteConsoleMessage;
            syncTask.StatusChanged -= statusChangedEventHandler;

            ShowResults(syncReport);

            return !syncReport.HasErrors;
        }

        /// <summary>
        /// Shows the results.
        /// </summary>
        /// <param name="syncReport">The sync report.</param>
        private static void ShowResults(SyncReport syncReport)
        {
            System.Console.WriteLine(Common.Statistics + ":");
            System.Console.WriteLine("   " + Common.CreateFilesSuccess + ": " + syncReport.SuccessCopyFiles + " / " + syncReport.TotalCopyFiles);
            System.Console.WriteLine("   " + Common.DeleteFilesSuccess + ": " + syncReport.SuccessDeleteFiles + " / " + syncReport.TotalDeleteFiles);
            System.Console.WriteLine("   " + Common.CreateDirectoriesSuccess + ": " + syncReport.SuccessCreateDirectories + " / " + syncReport.TotalCreateDirectories);
            System.Console.WriteLine("   " + Common.DeleteDirectoriesSuccess + ": " + syncReport.SuccessDeleteDirectories + " / " + syncReport.TotalDeleteDirectories);

            if (!string.IsNullOrEmpty(syncReport.LogFilePath))
            {
                System.Console.WriteLine();
                System.Console.WriteLine(Common.Logfile + ":");
                System.Console.WriteLine("   " + syncReport.LogFilePath);
            }
        }

        /// <summary>
        /// Executed when a new step during synchronzation occurs.
        /// </summary>
        /// <param name="syncTask">The sync task.</param>
        private void NewSyncPhase(SyncTask syncTask)
        {
            if (this.firstStatusReceived && !syncTask.Error)
            {
                this.WriteConsoleMessage(Resources.Done);
                System.Console.WriteLine();
            }

            if (syncTask.Error)
            {
                this.WriteConsoleMessage(syncTask.Status);
            }
            else if (syncTask.Status != null)
            {
                System.Console.WriteLine(syncTask.Status + ":");
                this.currentCursorPosition = 0;
            }

            this.firstStatusReceived = true;
        }

        /// <summary>
        /// Writes a message on the console. Used space is filled up with white spaces.
        /// </summary>
        /// <param name="message">The message.</param>
        private void WriteConsoleMessage(string message)
        {
            if (this.currentCursorPosition == 0)
            {
                this.currentCursorPosition = System.Console.CursorTop;
            }

            System.Console.CursorTop = this.currentCursorPosition;

            StringBuilder sb = new StringBuilder("   ");
            sb.Append(message);

            while (sb.Length < this.lengthOfLastMessage)
            {
                sb.Append(" ");
            }

            this.lengthOfLastMessage = sb.Length;

            System.Console.WriteLine(sb.ToString());
        }
    }
}
