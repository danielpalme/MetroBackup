using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Palmmedia.BackUp.Synchronization.Logging
{
    /// <summary>
    /// Logger that writes to a logfile.
    /// </summary>
    public class FileLogger : ILogger
    {
        /// <summary>
        /// The logged messages.
        /// </summary>
        private readonly List<string> messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
            this.messages = new List<string>();
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(string message)
        {
            this.messages.Add(DateTime.Now.ToString(CultureInfo.CurrentCulture) + "   " + message);
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <returns>The path to the logfile.</returns>
        public string WriteToFile()
        {
            string pathToLogFile = Path.Combine(
                Path.GetTempPath(),
                "MetroBackUp " + DateTime.Now.ToString("dd.MM.yyyy hh-mm-ss", CultureInfo.CurrentCulture) + ".log");

            try
            {
                using (var streamWriter = new StreamWriter(pathToLogFile, false, System.Text.Encoding.UTF8))
                {
                    foreach (string message in this.messages)
                    {
                        streamWriter.WriteLine(message);
                    }

                    streamWriter.Flush();
                }

                return pathToLogFile;
            }
            catch (IOException)
            {
                this.messages.Clear();
                return null;
            }
            finally
            {
                this.messages.Clear();
            }
        }
    }
}
