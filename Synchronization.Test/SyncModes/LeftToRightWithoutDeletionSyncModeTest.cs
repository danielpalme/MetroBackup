using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palmmedia.BackUp.Synchronization.SyncItems;
using Palmmedia.BackUp.Synchronization.SyncModes;

namespace Palmmedia.BackUp.Synchronization.Test
{
    /// <summary>
    ///This is a test class for LeftToRightWithoutDeletionSyncModeTest and is intended
    ///to contain all LeftToRightWithoutDeletionSyncModeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LeftToRightWithoutDeletionSyncModeTest : AbstractSyncTest
    {
        private ISyncMode target;

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CreateFiles();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DeleteFiles();
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.target = new LeftToRightWithoutDeletionSyncMode();
        }

        /// <summary>
        ///A test for DirectoryFoundOnReferenceDirectory
        ///</summary>
        [TestMethod()]
        public void DirectoryFoundOnReferenceDirectoryTest()
        {
            var syncItem = this.target.DirectoryFoundOnReferenceDirectory(directory_test1_1, directory_test1_2);
            Assert.IsNull(syncItem, "No synchronization required.");

            syncItem = this.target.DirectoryFoundOnReferenceDirectory(directory_test2_1, directory_test2_2);
            Assert.IsInstanceOfType(syncItem, typeof(CreateDirectorySyncItem), "Directory should be created.");
        }

        /// <summary>
        ///A test for DirectoryFoundOnTargetDirectory
        ///</summary>
        [TestMethod()]
        public void DirectoryFoundOnTargetDirectoryTest()
        {
            var syncItem = this.target.DirectoryFoundOnTargetDirectory(directory_test1_1, directory_test1_2);
            Assert.IsNull(syncItem, "No synchronization required.");

            syncItem = this.target.DirectoryFoundOnTargetDirectory(directory_test3_1, directory_test3_2);
            Assert.IsNull(syncItem, "No synchronization required, since nothing is deleted.");
        }

        /// <summary>
        ///A test for FileFoundOnReferenceDirectory
        ///</summary>
        [TestMethod()]
        public void FileFoundOnReferenceDirectoryTest()
        {
            var syncItem = this.target.FileFoundOnReferenceDirectory(file_only1_1, file_only1_2);
            Assert.IsInstanceOfType(syncItem, typeof(CopyFileSyncItem), "File should be copied.");

            syncItem = this.target.FileFoundOnReferenceDirectory(file_samedate_1, file_samedate_2);
            Assert.IsNull(syncItem, "No synchronization required, since files have same date.");

            syncItem = this.target.FileFoundOnReferenceDirectory(file_1newerthan2_1, file_1newerthan2_2);
            Assert.IsInstanceOfType(syncItem, typeof(CopyFileSyncItem), "File should be copied, since file on reference directory is newer.");

            syncItem = this.target.FileFoundOnReferenceDirectory(file_2newerthan1_1, file_2newerthan1_2);
            Assert.IsNull(syncItem, "No synchronization required, since file on target directory is newer.");
        }

        /// <summary>
        ///A test for FileFoundOnTargetDirectory
        ///</summary>
        [TestMethod()]
        public void FileFoundOnTargetDirectoryTest()
        {
            var syncItem = this.target.FileFoundOnTargetDirectory(file_only2_1, file_only2_2);
            Assert.IsNull(syncItem, "No synchronization required, since nothing is deleted.");

            syncItem = this.target.FileFoundOnTargetDirectory(file_samedate_1, file_samedate_2);
            Assert.IsNull(syncItem, "No synchronization required, since files have same date.");

            syncItem = this.target.FileFoundOnTargetDirectory(file_1newerthan2_1, file_1newerthan2_2);
            Assert.IsNull(syncItem, "No synchronization required, since search on reference directory handles this file.");

            syncItem = this.target.FileFoundOnTargetDirectory(file_2newerthan1_1, file_2newerthan1_2);
            Assert.IsNull(syncItem, "No synchronization required, since search on reference directory handles this file.");
        }
    }
}
