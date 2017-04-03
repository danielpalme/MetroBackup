using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Palmmedia.BackUp.Synchronization.FileSearch
{
    /// <summary>
    /// Searches for files and directories that match a pattern.
    /// </summary>
    public class FileSearcher
    {
        /// <summary>
        /// Determines whether the search has been canceled.
        /// </summary>
        private bool canceled;

        /// <summary>
        /// Raised when a file is found.
        /// </summary>
        public event Action<string> FileFound;

        /// <summary>
        /// Raised when a directory is found.
        /// </summary>
        public event Action<string> DirectoryFound;

        /// <summary>
        /// Raised when search changes the current directory.
        /// </summary>
        public event Action<string> DirectoryChanged;

        /// <summary>
        /// Searches in a directory for files and subdirectors that match a pattern.
        /// </summary>
        /// <param name="path">The path to search in.</param>
        /// <param name="filepattern">The pattern that the filename has to match.</param>
        /// <param name="dirpattern">The pattern that the name of the directory has to match.</param>
        /// <param name="recurse">Specifies whether the search should work recursivly.</param>
        /// <returns>True if the search has not been canceled.</returns>
        public bool Search(string path, string filepattern, string dirpattern, bool recurse)
        {
            this.canceled = false;
            return this.SearchDir(path, filepattern, dirpattern, recurse);
        }

        /// <summary>
        /// Stops the search.
        /// </summary>
        public void StopSearch()
        {
            this.canceled = true;
        }

        /// <summary>
        /// Executed when a file is found.
        /// </summary>
        /// <param name="file">The found file.</param>
        protected virtual void OnFileFound(string file)
        {
            if (this.FileFound != null)
            {
                this.FileFound(file);
            }
        }

        /// <summary>
        /// Executed when a directory is found.
        /// </summary>
        /// <param name="directory">The found directory.</param>
        protected virtual void OnDirectoryFound(string directory)
        {
            if (this.DirectoryFound != null)
            {
                this.DirectoryFound(directory);
            }
        }

        /// <summary>
        /// Executed when search changes the current directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        protected virtual void OnDirectoryChanged(string directory)
        {
            if (this.DirectoryChanged != null)
            {
                this.DirectoryChanged(directory);
            }
        }

        /// <summary>
        /// Searches in a directory for files and subdirectors that match a pattern if search has not been canceled.
        /// </summary>
        /// <param name="path">The path to search in.</param>
        /// <param name="filepattern">The pattern that the filename has to match.</param>
        /// <param name="dirpattern">The pattern that the name of the directory has to match.</param>
        /// <param name="recurse">Specifies whether the search should work recursivly.</param>
        /// <returns><c>true</c> if the search has not been canceled.</returns>
        private bool SearchDir(string path, string filepattern, string dirpattern, bool recurse)
        {
            string[] files;
            string[] directories;

            if (this.canceled)
            {
                return false;
            }

            files = Directory.GetFiles(path);
            directories = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                if (this.canceled)
                {
                    return false;
                }

                if (Regex.IsMatch(file, filepattern, RegexOptions.IgnoreCase))
                {
                    this.OnFileFound(file);
                }
            }

            foreach (string directory in directories)
            {
                if (this.canceled)
                {
                    return false;
                }

                if (recurse)
                {
                    // only search directories that are not a Reparse Point to avoid loops
                    if ((File.GetAttributes(directory) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                    {
                        this.OnDirectoryChanged(directory);
                        this.SearchDir(directory, filepattern, dirpattern, recurse);
                    }
                }

                if (Regex.IsMatch(directory, dirpattern, RegexOptions.IgnoreCase) && recurse)
                {
                    this.OnDirectoryFound(directory);
                }
            }

            return true;
        }
    }
}
