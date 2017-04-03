using System.IO;
using System.Windows.Controls;

namespace Palmmedia.BackUp.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        public Result()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the LogFileLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void LogFileLabel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (File.Exists(this.LogFileLabel.Content.ToString()))
            {
                System.Diagnostics.Process.Start(this.LogFileLabel.Content.ToString());
            }
        }
    }
}
