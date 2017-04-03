using System.Windows;

namespace Palmmedia.BackUp.UI.Wpf.Interaction
{
    /// <summary>
    /// Interface for handling dialogs in MVVM.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Gets a file name for opening.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The name of the file to open.</returns>
        string GetFileNameForOpening(string filter);

        /// <summary>
        /// Gets a file name for saving.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The name of the file to save.</returns>
        string GetFileNameForSaving(string filter);

        /// <summary>
        /// Gets a folder.
        /// </summary>
        /// <param name="startFolder">The start folder.</param>
        /// <returns>The folder.</returns>
        string GetFolder(string startFolder);

        /// <summary>
        /// Shows a question.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>The <see cref="MessageBoxResult"/>.</returns>
        MessageBoxResult ShowQuestion(string message, string caption);

        /// <summary>
        /// Shows a message.
        /// </summary>
        /// <param name="message">The message.</param>
        void ShowMessage(string message);
    }
}
