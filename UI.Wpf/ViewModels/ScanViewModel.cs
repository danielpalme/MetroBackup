using Palmmedia.BackUp.UI.Wpf.Common;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// ViewModel presenting the scan process.
    /// </summary>
    public class ScanViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The current directory.
        /// </summary>
        private string currentDirectory;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanViewModel"/> class.
        /// </summary>
        public ScanViewModel()
        {
            this.CancelCommand = new FlowCommand();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the current directory.
        /// </summary>
        /// <value>The current directory.</value>
        public string CurrentDirectory
        {
            get
            {
                return this.currentDirectory;
            }

            set
            {
                this.currentDirectory = value;
                this.OnPropertyChanged("CurrentDirectory");
            }
        }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public IFlowCommand CancelCommand { get; private set; }

        #endregion
    }
}
