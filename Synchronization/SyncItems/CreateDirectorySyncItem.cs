using System;
using System.Globalization;
using System.IO;
using Palmmedia.BackUp.Synchronization.Properties;

namespace Palmmedia.BackUp.Synchronization.SyncItems
{
    /// <summary>
    /// Creates a directory.
    /// </summary>
    public class CreateDirectorySyncItem : SyncItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDirectorySyncItem"/> class.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        public CreateDirectorySyncItem(string targetPath)
            : base(targetPath)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} ({1}): {2}",
                Resources.LoggerCreateDirectorySyncItem,
                this.SyncResult.ToString(),
                this.TargetPath);
        }

        /// <summary>
        /// Executes the synchronization.
        /// </summary>
        /// <returns>The error or <c>null</c> if no error has occured.</returns>
        protected override string Execute()
        {
            try
            {
                if (!Directory.Exists(this.TargetPath))
                {
                    Directory.CreateDirectory(this.TargetPath);
                }

                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return Resources.ExceptionUnauthorizedAccess;
            }
            catch (DirectoryNotFoundException)
            {
                return Resources.ExceptionDirectoryNotFound;
            }
            catch (PathTooLongException)
            {
                return Resources.ExceptionPathTooLong;
            }
            catch (IOException)
            {
                return Resources.ExceptionIO;
            }
        }
    }
}
