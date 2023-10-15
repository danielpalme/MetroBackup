using System.Windows.Controls;

namespace Palmmedia.BackUp.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            this.InitializeComponent();
            this.versionValueLabel.Content = this.GetType().Assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// Executed when the Homepage is clicked
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void HomepageValueLabel_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.palmmedia.de");
        }
    }
}
