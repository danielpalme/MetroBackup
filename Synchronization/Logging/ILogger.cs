
namespace Palmmedia.BackUp.Synchronization.Logging
{
    /// <summary>
    /// Interface that loggers have to implement.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Log(string message);

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <returns>The path to the logfile.</returns>
        string WriteToFile();
    }
}
