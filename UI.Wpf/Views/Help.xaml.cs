using System;
using System.Windows.Controls;
using Palmmedia.BackUp.Synchronization.SyncModes;
using Palmmedia.BackUp.UI.Wpf.ViewModels.Data;

namespace Palmmedia.BackUp.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Help"/> class.
        /// </summary>
        public Help()
        {
            this.InitializeComponent();

            var syncTask = new Synchronization.SyncTask(Properties.Help.Active)
            {
                IsActive = true,
                ReferenceDirectory = "C:\\",
                TargetDirectory = "D:\\",
                Recursive = true,
                Filter = "xml,exe,dll",
                ExcludedSubdirectories = "node_modules",
                LastSyncDate = DateTime.Now
            };

            SyncTaskViewModel taskItem = new SyncTaskViewModel(syncTask)
            {
                SyncModeType = SyncModeType.BothWays
            };

            this.DataContext = taskItem;
        }
    }
}
