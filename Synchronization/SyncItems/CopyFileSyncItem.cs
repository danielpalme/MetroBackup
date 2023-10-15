using System;
using System.Globalization;
using System.IO;
using Palmmedia.BackUp.Synchronization.Properties;

namespace Palmmedia.BackUp.Synchronization.SyncItems
{
    /// <summary>
    /// Copies a file.
    /// </summary>
    public class CopyFileSyncItem : SyncItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyFileSyncItem"/> class.
        /// </summary>
        /// <param name="referencePath">The reference path.</param>
        /// <param name="targetPath">The target path.</param>
        public CopyFileSyncItem(string referencePath, string targetPath)
            : base(targetPath)
        {
            if (string.IsNullOrEmpty(referencePath))
            {
                throw new ArgumentException("The path must not be null or empty.", "referencePath");
            }

            this.ReferencePath = referencePath;
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
                "{0} ({1}): {2} -> {3}",
                Resources.LoggerCopyFileSyncItem,
                this.SyncResult.ToString(),
                this.ReferencePath,
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
                File.Copy(this.ReferencePath, this.TargetPath, true);
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
                    File.Copy(this.ReferencePath, this.TargetPath, true);

                    return null;
                }
                catch (Exception)
                {
                    return Resources.ExceptionUnauthorizedAccess;
                }
            }
            catch (DirectoryNotFoundException)
            {
                return Resources.ExceptionDirectoryNotFound;
            }
            catch (PathTooLongException)
            {
                return Resources.ExceptionPathTooLong;
            }
            catch (FileNotFoundException)
            {
                return Resources.ExceptionFileNotFound;
            }
            catch (IOException)
            {
                return Resources.ExceptionIO;
            }
        }
    }
}
