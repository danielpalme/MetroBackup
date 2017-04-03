using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Palmmedia.BackUp.Synchronization.FileSearch
{
    /// <summary>
    /// Determines the target directory from a directory link in Windows.
    /// </summary>
    public static class ReparsePoint
    {
        #region Constants

        private const int INVALID_HANDLE_VALUE = -1;
        private const int OPEN_EXISTING = 3;
        private const int FILE_FLAG_OPEN_REPARSE_POINT = 0x200000;
        private const int FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;
        private const int FSCTL_GET_REPARSE_POINT = 0x900A8;

        /// <summary>
        /// If the path "REPARSE_GUID_DATA_BUFFER.SubstituteName" begins with this prefix, it is not interpreted by the virtual file system.
        /// </summary>
        private const string NonInterpretedPathPrefix = "\\??\\";

        #endregion

        /// <summary>
        /// Gets the target directory from a directory link in Windows.
        /// </summary>
        /// <param name="directoryInfo">The directory info of the directory link</param>
        /// <returns>The target directory, if link exists, otherwise the FullName</returns>
        /// <exception cref="ReparsePointException">Thrown if an error occurs</exception>
        public static string GetTargetDirectory(FileSystemInfo directoryInfo)
        {
            try
            {
                string targetDir = directoryInfo.FullName;

                // Is it a directory link?
                if ((directoryInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                {
                    // Open the directory link:
                    IntPtr hFile = SafeNativeMethods.CreateFile(
                        directoryInfo.FullName,
                        0,
                        0,
                        IntPtr.Zero,
                        OPEN_EXISTING,
                        FILE_FLAG_BACKUP_SEMANTICS | FILE_FLAG_OPEN_REPARSE_POINT,
                        IntPtr.Zero);

                    if (hFile.ToInt32() != INVALID_HANDLE_VALUE)
                    {
                        // Allocate a buffer for the reparse point data:
                        int outBufferSize = Marshal.SizeOf(typeof(REPARSE_GUID_DATA_BUFFER));
                        IntPtr outBuffer = Marshal.AllocHGlobal(outBufferSize);

                        try
                        {
                            // Read the reparse point data:
                            int bytesReturned;
                            int readOK = SafeNativeMethods.DeviceIoControl(
                                hFile,
                                FSCTL_GET_REPARSE_POINT,
                                IntPtr.Zero,
                                0,
                                outBuffer,
                                outBufferSize,
                                out bytesReturned,
                                IntPtr.Zero);

                            if (readOK != 0)
                            {
                                // Get the target directory from the reparse point data:
                                REPARSE_GUID_DATA_BUFFER rgdBuffer = (REPARSE_GUID_DATA_BUFFER)Marshal.PtrToStructure(outBuffer, typeof(REPARSE_GUID_DATA_BUFFER));
                                targetDir = Encoding.Unicode.GetString(
                                    rgdBuffer.PathBuffer,
                                    rgdBuffer.SubstituteNameOffset,
                                    rgdBuffer.SubstituteNameLength);

                                if (targetDir.StartsWith(NonInterpretedPathPrefix, StringComparison.OrdinalIgnoreCase))
                                {
                                    targetDir = targetDir.Substring(NonInterpretedPathPrefix.Length);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ReparsePointException("Failed to access ReparsePoint.", ex);
                        }
                        finally
                        {
                            // Free the buffer for the reparse point data:
                            Marshal.FreeHGlobal(outBuffer);

                            // Close the directory link:
                            SafeNativeMethods.CloseHandle(hFile);
                        }
                    }
                }

                return targetDir;
            }
            catch (Exception ex)
            {
                throw new ReparsePointException("Failed to access ReparsePoint.", ex);
            }
        }

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        private struct REPARSE_GUID_DATA_BUFFER
        {
            public uint ReparseTag;
            public ushort ReparseDataLength;
            public ushort Reserved;
            public ushort SubstituteNameOffset;
            public ushort SubstituteNameLength;
            public ushort PrintNameOffset;
            public ushort PrintNameLength;

            /// <summary>
            /// Contains the SubstituteName and the PrintName.
            /// The SubstituteName is the path of the target directory.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
            public byte[] PathBuffer;
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates the access to unmanaged code
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            int dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);
    }
}