using System.Windows;
using Microsoft.Win32;

namespace Palmmedia.BackUp.UI.Wpf.Interaction
{
    /// <summary>
    /// <see cref="IDialog"/> implementation using dialogs.
    /// </summary>
    internal class FormDialog : IDialog
    {
        /// <summary>
        /// The filter for XML files.
        /// </summary>
        public const string XMLFILTER = "XML (*.xml)|*.xml";

        /// <summary>
        /// Gets a file name for opening.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The name of the file to open.</returns>
        public string GetFileNameForOpening(string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.ShowDialog();

            return openFileDialog.FileName;
        }

        /// <summary>
        /// Gets a file name for saving.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The name of the file to save.</returns>
        public string GetFileNameForSaving(string filter)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.ShowDialog();

            return saveFileDialog.FileName;
        }

        /// <summary>
        /// Gets a folder.
        /// </summary>
        /// <param name="startFolder">The start folder.</param>
        /// <returns>The folder.</returns>
        public string GetFolder(string startFolder)
        {
            using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = startFolder;
                folderBrowserDialog.ShowDialog();

                return folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Shows a question.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>The <see cref="MessageBoxResult"/>.</returns>
        public MessageBoxResult ShowQuestion(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo);
        }

        /// <summary>
        /// Shows a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
