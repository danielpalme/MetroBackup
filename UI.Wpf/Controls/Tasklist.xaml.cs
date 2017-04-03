using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Palmmedia.BackUp.UI.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Tasklist : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tasklist"/> class.
        /// </summary>
        public Tasklist()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the GotFocus event of the InitialTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void InitialTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListBox))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            var listBox = dep as ListBox;

            if (listBox != null)
            {
                listBox.SelectedValue = this.DataContext;
            }

#if DEBUG
            if (listBox == null)
            {
                System.Diagnostics.Trace.WriteLine("Expected that this control is inside a ListBox.");
            }
#endif
        }
    }
}
