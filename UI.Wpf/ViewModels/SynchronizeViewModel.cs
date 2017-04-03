using Palmmedia.BackUp.UI.Wpf.Common;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// ViewModel presenting the synchronization.
    /// </summary>
    public class SynchronizeViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The current file.
        /// </summary>
        private string currentFile;

        /// <summary>
        /// The progress.
        /// </summary>
        private double progress;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeViewModel"/> class.
        /// </summary>
        public SynchronizeViewModel()
        {
            this.CancelCommand = new FlowCommand();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the current file.
        /// </summary>
        /// <value>The current file.</value>
        public string CurrentFile
        {
            get
            {
                return this.currentFile;
            }

            set
            {
                this.currentFile = value;
                this.OnPropertyChanged("CurrentFile");
            }
        }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public double Progress
        {
            get
            {
                return this.progress;
            }

            set
            {
                this.progress = value;
                this.OnPropertyChanged("Progress");
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
