using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf.Actions
{
    /// <summary>
    /// Abstract base class for actions involved in synchronization process.
    /// </summary>
    internal abstract class WorkflowAction : IDependsOn<MainViewModel>
    {
        /// <summary>
        /// Gets the main view model.
        /// </summary>
        /// <value>The main view model.</value>
        protected MainViewModel MainViewModel { get; private set; }

        /// <summary>
        /// Injects the specified main view model.
        /// </summary>
        /// <param name="mainViewModel">The main view model.</param>
        public void Inject(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
        }
    }
}
