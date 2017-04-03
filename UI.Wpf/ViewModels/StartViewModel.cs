using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Palmmedia.BackUp.UI.Wpf.Common;
using Palmmedia.BackUp.UI.Wpf.Interaction;
using Palmmedia.BackUp.UI.Wpf.ViewModels.Data;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels
{
    /// <summary>
    /// ViewModel presenting the start view.
    /// </summary>
    public class StartViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The path to the file containing the saved task lists.
        /// </summary>
        private static readonly string taskFilePath = InitTaskFilePath();

        /// <summary>
        /// The selected task list.
        /// </summary>
        private TasklistViewModel selectedTasklist;

        /// <summary>
        /// Counter used to generate names for new joblists.
        /// </summary>
        private int jobListCounter = 0;

        /// <summary>
        /// Counter used to generate names for new jobs.
        /// </summary>
        private int jobCounter = 0;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StartViewModel"/> class.
        /// </summary>
        public StartViewModel()
        {
            this.Tasklists = new ObservableCollection<TasklistViewModel>();
            this.DialogHandler = new FormDialog();

            this.SynchronizeCommand = new FlowCommand(o => this.selectedTasklist != null);
            this.AddJoblistCommand = new RelayCommand(o => this.AddJobList());
            this.RemoveJoblistCommand = new RelayCommand(o => this.RemoveJobList((TasklistViewModel)o));
            this.AddJobCommand = new RelayCommand(o => this.AddJob(), o => this.SelectedTasklist != null);
            this.RemoveJobCommand = new RelayCommand(o => this.RemoveJob((SyncTaskViewModel)o));
            this.ImportCommand = new RelayCommand(o => this.LoadJobList());
            this.ExportCommand = new RelayCommand(o => this.SaveJobList());
        }

        #region Properties

        /// <summary>
        /// Gets the tasklists.
        /// </summary>
        /// <value>The tasklists.</value>
        public ObservableCollection<TasklistViewModel> Tasklists { get; private set; }

        /// <summary>
        /// Gets or sets the selected tasklist.
        /// </summary>
        /// <value>The selected tasklist.</value>
        public TasklistViewModel SelectedTasklist
        {
            get
            {
                return this.selectedTasklist;
            }

            set
            {
                this.selectedTasklist = value;
                this.OnPropertyChanged("SelectedTasklist");
            }
        }

        /// <summary>
        /// Gets the synchronize command.
        /// </summary>
        /// <value>The synchronize command.</value>
        public IFlowCommand SynchronizeCommand { get; private set; }

        /// <summary>
        /// Gets the add joblist command.
        /// </summary>
        /// <value>The add joblist command.</value>
        public ICommand AddJoblistCommand { get; private set; }

        /// <summary>
        /// Gets the remove joblist command.
        /// </summary>
        /// <value>The remove joblist command.</value>
        public ICommand RemoveJoblistCommand { get; private set; }

        /// <summary>
        /// Gets the add job command.
        /// </summary>
        /// <value>The add job command.</value>
        public ICommand AddJobCommand { get; private set; }

        /// <summary>
        /// Gets the remove job command.
        /// </summary>
        /// <value>The remove job command.</value>
        public ICommand RemoveJobCommand { get; private set; }

        /// <summary>
        /// Gets the import command.
        /// </summary>
        /// <value>The import command.</value>
        public ICommand ImportCommand { get; private set; }

        /// <summary>
        /// Gets the export command.
        /// </summary>
        /// <value>The export command.</value>
        public ICommand ExportCommand { get; private set; }

        /// <summary>
        /// Gets or sets the dialog handler.
        /// </summary>
        /// <value>The dialog handler.</value>
        internal IDialog DialogHandler { get; set; }

        #endregion

        /// <summary>
        /// Loads the default job list.
        /// </summary>
        internal void LoadDefaultJobList()
        {
            if (File.Exists(taskFilePath))
            {
                this.ImportJobList(taskFilePath);
            }
        }

        /// <summary>
        /// Saves the default job list.
        /// </summary>
        internal void SaveDefaultJobList()
        {
            this.ExportJobList(taskFilePath);
        }

        /// <summary>
        /// Inits the path to the file containing the saved task lists.
        /// </summary>
        /// <returns>The path to the file containing the saved task lists.</returns>
        private static string InitTaskFilePath()
        {
            string path = System.Windows.Forms.Application.UserAppDataPath;
            return Path.Combine(path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar)), "Tasks.xml");
        }

        /// <summary>
        /// Saves the job list.
        /// </summary>
        private void SaveJobList()
        {
            string file = this.DialogHandler.GetFileNameForSaving(FormDialog.XMLFILTER);

            if (!string.IsNullOrEmpty(file))
            {
                this.ExportJobList(file);
            }
        }

        /// <summary>
        /// Exports the job list.
        /// </summary>
        /// <param name="path">The path to the job list.</param>
        private void ExportJobList(string path)
        {
            try
            {
                this.Tasklists.ToXml().Save(path);
            }
            catch (Exception)
            {
                this.DialogHandler.ShowMessage(Properties.Resources.ErrorSave);
            }
        }

        /// <summary>
        /// Loads the job list.
        /// </summary>
        private void LoadJobList()
        {
            string file = this.DialogHandler.GetFileNameForOpening(FormDialog.XMLFILTER);

            if (string.IsNullOrEmpty(file) || !File.Exists(file))
            {
                return;
            }

            var existingJoblists = this.Tasklists.ToArray();
            bool replaceExistingJoblists = this.DialogHandler.ShowQuestion(Properties.Resources.ReplaceCurrentTasks, Properties.Resources.Replace) == System.Windows.MessageBoxResult.Yes;

            try
            {
                if (replaceExistingJoblists)
                {
                    this.Tasklists.Clear();
                }

                this.ImportJobList(file);
            }
            catch (Exception)
            {
                if (replaceExistingJoblists)
                {
                    foreach (var joblist in existingJoblists)
                    {
                        this.Tasklists.Add(joblist);
                    }
                }

                this.DialogHandler.ShowMessage(Properties.Resources.ErrorLoad);
            }
        }

        /// <summary>
        /// Imports the job list.
        /// </summary>
        /// <param name="path">The path to the job list.</param>
        private void ImportJobList(string path)
        {
            foreach (var tasklist in XDocument.Load(path).FromXml())
            {
                this.Tasklists.Add(tasklist);
            }

            if (this.Tasklists.Count > 0)
            {
                this.SelectedTasklist = this.Tasklists[0];
            }
        }

        /// <summary>
        /// Adds the job list.
        /// </summary>
        private void AddJobList()
        {
            var taskListViewModel = new TasklistViewModel(string.Format(CultureInfo.InvariantCulture, "{0} {1}", Properties.Resources.DefaultJoblistTitle, ++this.jobListCounter));
            this.Tasklists.Add(taskListViewModel);
            this.SelectedTasklist = taskListViewModel;
        }

        /// <summary>
        /// Removes the given job list.
        /// </summary>
        /// <param name="tasklist">The tasklist.</param>
        private void RemoveJobList(TasklistViewModel tasklist)
        {
            if (tasklist == this.SelectedTasklist)
            {
                this.SelectedTasklist = null;
            }

            this.Tasklists.Remove(tasklist);

            if (this.SelectedTasklist == null && this.Tasklists.Count > 0)
            {
                this.SelectedTasklist = this.Tasklists[0];
            }
        }

        /// <summary>
        /// Adds the job to the selected task list.
        /// </summary>
        private void AddJob()
        {
            this.SelectedTasklist.Add(new SyncTaskViewModel(new Synchronization.SyncTask(string.Format(CultureInfo.InvariantCulture, "{0} {1}", Properties.Resources.DefaultJobTitle, ++this.jobCounter))));
        }

        /// <summary>
        /// Removes the given job.
        /// </summary>
        /// <param name="syncTask">The sync task.</param>
        private void RemoveJob(SyncTaskViewModel syncTask)
        {
            this.SelectedTasklist.Remove(syncTask);
        }
    }
}
