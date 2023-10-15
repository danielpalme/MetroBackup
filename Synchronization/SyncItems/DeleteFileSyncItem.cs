using System;
using System.Globalization;
using System.IO;
using Palmmedia.BackUp.Synchronization.Properties;

namespace Palmmedia.BackUp.Synchronization.SyncItems
{
    /// <summary>
    /// Deletes a file.
    /// </summary>
    public class DeleteFileSyncItem : SyncItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFileSyncItem"/> class.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        public DeleteFileSyncItem(string targetPath)
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
                Resources.LoggerDeleteFileSyncItem,
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
                File.Delete(this.TargetPath);
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    FileAttributes attributes = File.GetAttributes(this.TargetPath);
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        attributes &= ~FileAttributes.ReadOnly;
                        File.SetAttributes(this.TargetPath, attributes);
                    }

                    File.Delete(this.TargetPath);

                    return null;
                }
                catch (Exception)
                {
                    return Resources.ExceptionUnauthorizedAccess;
                }
            }
            catch (DirectoryNotFoundException)
            {
                // File is already deleted
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
