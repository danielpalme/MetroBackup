
namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// The main viewmodel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The currently displayed sync view.
        /// </summary>
        private ViewModelBase currentSyncView;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current sync view.
        /// </summary>
        /// <value>The current sync view.</value>
        public ViewModelBase CurrentSyncView
        {
            get
            {
                return this.currentSyncView;
            }

            set
            {
                this.currentSyncView = value;
                this.OnPropertyChanged("CurrentSyncView");
            }
        }

        #endregion
    }
}
