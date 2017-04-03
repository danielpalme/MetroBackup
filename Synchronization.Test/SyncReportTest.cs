using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization.Test
{


    /// <summary>
    ///This is a test class for SyncReportTest and is intended
    ///to contain all SyncReportTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SyncReportTest
    {
        private SyncReport target;

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.target = new SyncReport();
        }

        /// <summary>
        ///A test for AddFailedSyncItem
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Expected exception.")]
        public void AddFailedSyncItem_CalledWithSyncItemBase_ThrowsInvalidOperationException()
        {
            SyncItemBase syncItem = null;

            this.target.AddFailedSyncItem(syncItem);
        }

        /// <summary>
        ///A test for AddFailedSyncItem
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithCopyFileSyncItem_FailedCopyFilesContainsItem()
        {
            CopyFileSyncItem syncItem = new CopyFileSyncItem(" ", " ");

            this.target.AddFailedSyncItem(syncItem);

            var failedCopyFiles = this.target.FailedCopyFiles;
            Assert.IsTrue(failedCopyFiles.Count() == 1, "SyncReport contains wrong number of FailedCopyFiles.");
            Assert.ReferenceEquals(syncItem, failedCopyFiles.ElementAt(0));
        }

        /// <summary>
        ///A test for AddFailedSyncItem
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithDeleteFileSyncItem_FailedDeleteFilesContainsItem()
        {
            DeleteFileSyncItem syncItem = new DeleteFileSyncItem(" ");

            this.target.AddFailedSyncItem(syncItem);

            var failedDeleteFiles = this.target.FailedDeleteFiles;
            Assert.IsTrue(failedDeleteFiles.Count() == 1, "SyncReport contains wrong number of FailedDeleteFiles.");
            Assert.ReferenceEquals(syncItem, failedDeleteFiles.ElementAt(0));
        }

        /// <summary>
        ///A test for AddFailedSyncItem
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithDeleteDirectorySyncItem_FailedDeleteDirectoriesContainsItem()
        {
            DeleteDirectorySyncItem syncItem = new DeleteDirectorySyncItem(" ");

            this.target.AddFailedSyncItem(syncItem);

            var failedDeleteDirectories = this.target.FailedDeleteDirectories;
            Assert.IsTrue(failedDeleteDirectories.Count() == 1, "SyncReport contains wrong number of FailedDeleteDirectories.");
            Assert.ReferenceEquals(syncItem, failedDeleteDirectories.ElementAt(0));
        }

        /// <summary>
        ///A test for AddFailedSyncItem
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithCreateDirectorySyncItem_FailedCreateDirectoriesContainsItem()
        {
            CreateDirectorySyncItem syncItem = new CreateDirectorySyncItem(" ");

            this.target.AddFailedSyncItem(syncItem);

            var failedCreateDirectories = this.target.FailedCreateDirectories;
            Assert.IsTrue(failedCreateDirectories.Count() == 1, "SyncReport contains wrong number of FailedCreateDirectories.");
            Assert.ReferenceEquals(syncItem, failedCreateDirectories.ElementAt(0));
        }
    }
}
