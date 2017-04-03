using System;
using System.Globalization;
using System.IO;
using Palmmedia.BackUp.Synchronization.Properties;

namespace Palmmedia.BackUp.Synchronization.SyncItems
{
    /// <summary>
    /// Deletes a directory.
    /// </summary>
    public class DeleteDirectorySyncItem : SyncItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDirectorySyncItem"/> class.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        public DeleteDirectorySyncItem(string targetPath)
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
                Resources.LoggerDeleteDirectorySyncItem,
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
                if (Directory.Exists(this.TargetPath))
                {
                    Directory.Delete(this.TargetPath, true);
                }

                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return Resources.ExceptionUnauthorizedAccess;
            }
            catch (DirectoryNotFoundException)
            {
                // Directory is already deleted
                return null;
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
