﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Palmmedia.BackUp.Synchronization.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Palmmedia.BackUp.Synchronization.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reference Directory not set.
        /// </summary>
        internal static string ErrorNoReferenceDirectory {
            get {
                return ResourceManager.GetString("ErrorNoReferenceDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target Directory not set.
        /// </summary>
        internal static string ErrorNoTargetDirectory {
            get {
                return ResourceManager.GetString("ErrorNoTargetDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reference Directory could not be created.
        /// </summary>
        internal static string ErrorReferenceDirectoryCouldNotBeCreated {
            get {
                return ResourceManager.GetString("ErrorReferenceDirectoryCouldNotBeCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reference Directory does not exist.
        /// </summary>
        internal static string ErrorReferenceDirectoryDoesNotExist {
            get {
                return ResourceManager.GetString("ErrorReferenceDirectoryDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reference Directory and Target Directory must not be equal.
        /// </summary>
        internal static string ErrorReferenceDirectoryEqualsTargetDirectory {
            get {
                return ResourceManager.GetString("ErrorReferenceDirectoryEqualsTargetDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target Directory could not be created.
        /// </summary>
        internal static string ErrorTargetDirectoryCouldNotBeCreated {
            get {
                return ResourceManager.GetString("ErrorTargetDirectoryCouldNotBeCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target Directory does not exist.
        /// </summary>
        internal static string ErrorTargetDirectoryDoesNotExist {
            get {
                return ResourceManager.GetString("ErrorTargetDirectoryDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The directory does not exist.
        /// </summary>
        internal static string ExceptionDirectoryNotFound {
            get {
                return ResourceManager.GetString("ExceptionDirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File does not exist any longer.
        /// </summary>
        internal static string ExceptionFileNotFound {
            get {
                return ResourceManager.GetString("ExceptionFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IO-Error.
        /// </summary>
        internal static string ExceptionIO {
            get {
                return ResourceManager.GetString("ExceptionIO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path too long.
        /// </summary>
        internal static string ExceptionPathTooLong {
            get {
                return ResourceManager.GetString("ExceptionPathTooLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Access denied.
        /// </summary>
        internal static string ExceptionUnauthorizedAccess {
            get {
                return ResourceManager.GetString("ExceptionUnauthorizedAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copied file.
        /// </summary>
        internal static string LoggerCopyFileSyncItem {
            get {
                return ResourceManager.GetString("LoggerCopyFileSyncItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Created directory.
        /// </summary>
        internal static string LoggerCreateDirectorySyncItem {
            get {
                return ResourceManager.GetString("LoggerCreateDirectorySyncItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleted directory.
        /// </summary>
        internal static string LoggerDeleteDirectorySyncItem {
            get {
                return ResourceManager.GetString("LoggerDeleteDirectorySyncItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleted file.
        /// </summary>
        internal static string LoggerDeleteFileSyncItem {
            get {
                return ResourceManager.GetString("LoggerDeleteFileSyncItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Job &apos;{0}&apos;: Reference Directory: {1}, Target Directory: {2}, Filter: {3}, SyncMode: {4}, Include subdirectories: {5}.
        /// </summary>
        internal static string LoggerSyncTask {
            get {
                return ResourceManager.GetString("LoggerSyncTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copying files.
        /// </summary>
        internal static string SyncStatusCopyingFiles {
            get {
                return ResourceManager.GetString("SyncStatusCopyingFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating directories.
        /// </summary>
        internal static string SyncStatusCreatingDirectories {
            get {
                return ResourceManager.GetString("SyncStatusCreatingDirectories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting directories.
        /// </summary>
        internal static string SyncStatusDeletingDirectories {
            get {
                return ResourceManager.GetString("SyncStatusDeletingDirectories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting files.
        /// </summary>
        internal static string SyncStatusDeletingFiles {
            get {
                return ResourceManager.GetString("SyncStatusDeletingFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scanning directory.
        /// </summary>
        internal static string SyncStatusScanningDirectories {
            get {
                return ResourceManager.GetString("SyncStatusScanningDirectories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validating directories.
        /// </summary>
        internal static string SyncStatusValidatingDirectories {
            get {
                return ResourceManager.GetString("SyncStatusValidatingDirectories", resourceCulture);
            }
        }
    }
}