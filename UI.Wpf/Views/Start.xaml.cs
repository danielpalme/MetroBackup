using System.Collections.Specialized;
using System.Windows.Controls;

namespace Palmmedia.BackUp.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Start"/> class.
        /// </summary>
        public Start()
        {
            this.InitializeComponent();

            ((INotifyCollectionChanged)this.TasklistsListBox.Items).CollectionChanged += this.TasklistsListBox_CollectionChanged;
            ((INotifyCollectionChanged)this.SyncTasksListBox.Items).CollectionChanged += this.SyncTasksListBox_CollectionChanged;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SyncTasksListBox.HasItems)
            {
                this.SyncTasksListBox.ScrollIntoView(this.SyncTasksListBox.Items[0]);
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the TasklistsListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void TasklistsListBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.TasklistsListBox.ScrollIntoView(e.NewItems[0]);
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the SyncTasksListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void SyncTasksListBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.SyncTasksListBox.ScrollIntoView(e.NewItems[0]);
            }
        }
    }
}
