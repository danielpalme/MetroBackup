using System.Windows;
using System.Windows.Controls;

namespace Palmmedia.BackUp.UI.Wpf.Controls
{
    /// <summary>
    /// <see cref="ContentControl"/> thats gets animated when loaded/unloaded.
    /// </summary>
    public class MetroContentControl : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetroContentControl"/> class.
        /// </summary>
        public MetroContentControl()
        {
            this.DefaultStyleKey = typeof(MetroContentControl);

            this.Loaded += this.MetroContentControl_Loaded;
            this.Unloaded += this.MetroContentControl_Unloaded;
        }

        /// <summary>
        /// Handles the Loaded event of the MetroContentControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MetroContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AfterLoaded", true);
        }

        /// <summary>
        /// Handles the Unloaded event of the MetroContentControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MetroContentControl_Unloaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AfterUnLoaded", false);
        }
    }
}
