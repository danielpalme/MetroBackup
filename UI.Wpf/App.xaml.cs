using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.Synchronization.Logging;
using Palmmedia.BackUp.UI.Wpf.Actions;
using Palmmedia.BackUp.UI.Wpf.ViewModels;

namespace Palmmedia.BackUp.UI.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));

        /// <summary>
        /// The <see cref="StartViewModel"/>.
        /// </summary>
        private StartViewModel startViewModel;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.startViewModel != null)
                {
                    this.startViewModel.Dispose();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
               typeof(FrameworkElement),
               new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Thread.CurrentThread.Name = "MainThread";

            base.OnStartup(e);

            // Init logger
            log4net.Config.XmlConfigurator.Configure();

            // Register handler for unhandled exceptions
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

            // Build
            var mainViewModel = new MainViewModel();

            this.startViewModel = new StartViewModel();
            var scanViewModel = new ScanViewModel();
            var previewViewModel = new PreviewViewModel();
            var previewNoFilesViewModel = new PreviewNoFilesViewModel();
            var synchronizeViewModel = new SynchronizeViewModel();
            var synchronizeNoFilesViewModel = new SynchronizeNoFilesViewModel();
            var resultViewModel = new ResultViewModel();

            var synchronizer = new Synchronizer(new FileLogger());

            var startAction = new StartAction();
            var scanAction = new ScanAction(synchronizer);
            var previewAction = new PreviewAction();
            var previewNoFilesAction = new PreviewNoFilesAction();
            var synchronizeNoFilesAction = new SynchronizeNoFilesAction();
            var synchronizeAction = new SynchronizeAction(synchronizer);
            var resultAction = new ResultAction();

            var mainWindow = new MainWindow();

            // Bind
            startAction.Synchronize += scanAction.Process;

            scanAction.Preview += previewAction.Process;
            scanAction.PreviewNoFiles += previewNoFilesAction.Process;
            scanAction.Canceled += startAction.Process;

            previewAction.Synchronize += synchronizeAction.Process;
            previewAction.SynchronizeNoFiles += synchronizeNoFilesAction.Process;
            previewAction.Canceled += startAction.Process;

            previewNoFilesAction.Ok += startAction.Process;

            synchronizeAction.Result += resultAction.Process;

            synchronizeNoFilesAction.Ok += startAction.Process;

            resultAction.Ok += startAction.Process;

            // Inject
            startAction.Inject(mainViewModel);
            startAction.Inject(this.startViewModel);
            scanAction.Inject(mainViewModel);
            scanAction.Inject(scanViewModel);
            previewAction.Inject(mainViewModel);
            previewAction.Inject(previewViewModel);
            previewNoFilesAction.Inject(mainViewModel);
            previewNoFilesAction.Inject(previewNoFilesViewModel);
            synchronizeNoFilesAction.Inject(mainViewModel);
            synchronizeNoFilesAction.Inject(synchronizeNoFilesViewModel);
            synchronizeAction.Inject(mainViewModel);
            synchronizeAction.Inject(synchronizeViewModel);
            resultAction.Inject(mainViewModel);
            resultAction.Inject(this.startViewModel);
            resultAction.Inject(resultViewModel);

            mainWindow.Inject(mainViewModel);

            // Run
            this.startViewModel.LoadDefaultJobList();

            this.MainWindow = mainWindow;
            startAction.Process();
            this.MainWindow.Show();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            this.startViewModel.SaveDefaultJobList();
            base.OnExit(e);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        private static void HandleException(Exception ex)
        {
            logger.Error("Unhandled exception occured.", ex);
            MessageBox.Show(string.Format(CultureInfo.InvariantCulture, Palmmedia.BackUp.UI.Wpf.Properties.Resources.ApplicationError, ex.Message));
        }

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the Application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }
    }
}
