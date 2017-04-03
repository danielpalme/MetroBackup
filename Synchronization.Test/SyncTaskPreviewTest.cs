using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palmmedia.BackUp.Synchronization.SyncItems;

namespace Palmmedia.BackUp.Synchronization.Test
{
    /// <summary>
    ///This is a test class for SyncTaskPreviewTest and is intended
    ///to contain all SyncTaskPreviewTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SyncTaskPreviewTest : AbstractSyncTest
    {
        private SyncTaskPreview target;

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
            this.target = new SyncTaskPreview();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Expected exception.")]
        public void AddSyncItem_CalledWithSyncItemBase_ThrowsInvalidOperationException()
        {
            SyncItemBase syncItem = null;

            this.target.Add(syncItem);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddSyncItem_CalledWithCopyFileSyncItem_CopyFileSyncItemsContainsItem()
        {
            CopyFileSyncItem syncItem = new CopyFileSyncItem(" ", " ");

            this.target.Add(syncItem);

            var copyFileSyncItems = this.target.CopyFileSyncItems;
            Assert.IsTrue(copyFileSyncItems.Count() == 1, "SyncTaskPreview contains wrong number of CopyFileSyncItems.");
            Assert.AreEqual(1, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
            Assert.ReferenceEquals(syncItem, copyFileSyncItems.ElementAt(0));
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithDeleteFileSyncItem_DeleteFileSyncItemsContainsItem()
        {
            DeleteFileSyncItem syncItem = new DeleteFileSyncItem(" ");

            this.target.Add(syncItem);

            var deleteFileSyncItems = this.target.DeleteFileSyncItems;
            Assert.IsTrue(deleteFileSyncItems.Count() == 1, "SyncTaskPreview contains wrong number of DeleteFileSyncItems.");
            Assert.AreEqual(1, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
            Assert.ReferenceEquals(syncItem, deleteFileSyncItems.ElementAt(0));
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithDeleteDirectorySyncItem_DeleteDirectorySyncItemsContainsItem()
        {
            DeleteDirectorySyncItem syncItem = new DeleteDirectorySyncItem(" ");

            this.target.Add(syncItem);

            var deleteDirectorySyncItems = this.target.DeleteDirectorySyncItems;
            Assert.IsTrue(deleteDirectorySyncItems.Count() == 1, "SyncTaskPreview contains wrong number of DeleteDirectorySyncItems.");
            Assert.AreEqual(1, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
            Assert.ReferenceEquals(syncItem, deleteDirectorySyncItems.ElementAt(0));
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddFailedSyncItem_CalledWithCreateDirectorySyncItem_CreateDirectorySyncItemsContainsItem()
        {
            CreateDirectorySyncItem syncItem = new CreateDirectorySyncItem(" ");

            this.target.Add(syncItem);

            var createDirectorySyncItems = this.target.CreateDirectorySyncItems;
            Assert.IsTrue(createDirectorySyncItems.Count() == 1, "SyncTaskPreview contains wrong number of CreateDirectorySyncItems.");
            Assert.AreEqual(1, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
            Assert.ReferenceEquals(syncItem, createDirectorySyncItems.ElementAt(0));
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithEmptyCreateDirectories_DirectoriesAreRemoved()
        {
            CreateDirectorySyncItem syncItem1 = new CreateDirectorySyncItem("C:\\test");
            CreateDirectorySyncItem syncItem2 = new CreateDirectorySyncItem("C:\\test\\test");

            this.target.Add(syncItem1);
            this.target.Add(syncItem2);

            this.target.RemoveUnnecessarySyncItems();

            var createDirectorySyncItems = this.target.CreateDirectorySyncItems;
            Assert.IsTrue(createDirectorySyncItems.Count() == 0, "SyncTaskPreview contains wrong number of CreateDirectorySyncItems.");
            Assert.AreEqual(0, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithNonEmptyCreateDirectories_DirectoriesAreNotRemoved()
        {
            CreateDirectorySyncItem syncItem1 = new CreateDirectorySyncItem("C:\\test");
            CreateDirectorySyncItem syncItem2 = new CreateDirectorySyncItem("C:\\test\\test");
            CopyFileSyncItem syncItem3 = new CopyFileSyncItem("C:\\xyz\\test\\1.txt", "C:\\test\\test\\1.txt");

            this.target.Add(syncItem1);
            this.target.Add(syncItem2);
            this.target.Add(syncItem3);

            this.target.RemoveUnnecessarySyncItems();

            var createDirectorySyncItems = this.target.CreateDirectorySyncItems;
            Assert.IsTrue(createDirectorySyncItems.Count() == 2, "SyncTaskPreview contains wrong number of CreateDirectorySyncItems.");
            Assert.AreEqual(3, this.target.Count, "SyncTaskPreview contains wrong number of elements.");

            var copyFileSyncItems = this.target.CopyFileSyncItems;
            Assert.IsTrue(copyFileSyncItems.Count() == 1, "SyncTaskPreview contains wrong number of CopyFileSyncItems.");
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithNotRemovedDirectory_DirectoriesAreNotRemoved()
        {
            DeleteDirectorySyncItem syncItem1 = new DeleteDirectorySyncItem(directory_base);

            this.target.Add(syncItem1);

            this.target.RemoveUnnecessarySyncItems();

            var deleteDirectorySyncItems = this.target.DeleteDirectorySyncItems;
            Assert.IsTrue(deleteDirectorySyncItems.Count() == 1, "SyncTaskPreview contains wrong number of DeleteDirectorySyncItems.");
            Assert.AreEqual(1, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithNotRemovedFiles_DirectoriesArePartiallyRemoved()
        {
            DeleteDirectorySyncItem syncItem1 = new DeleteDirectorySyncItem(directory_test);
            DeleteDirectorySyncItem syncItem2 = new DeleteDirectorySyncItem(directory_testtest);

            this.target.Add(syncItem1);
            this.target.Add(syncItem2);

            this.target.RemoveUnnecessarySyncItems();

            var deleteDirectorySyncItems = this.target.DeleteDirectorySyncItems;
            Assert.IsTrue(deleteDirectorySyncItems.Count() == 1, "SyncTaskPreview contains wrong number of DeleteDirectorySyncItems.");
            Assert.ReferenceEquals(syncItem1, deleteDirectorySyncItems.ElementAt(0));
            Assert.AreEqual(1, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithRemovedFilesAndDirectories_DirectoriesAreRemoved()
        {
            DeleteDirectorySyncItem syncItem1 = new DeleteDirectorySyncItem(directory_test);
            DeleteDirectorySyncItem syncItem2 = new DeleteDirectorySyncItem(directory_testtest);
            DeleteFileSyncItem syncItem3 = new DeleteFileSyncItem(file1);
            DeleteFileSyncItem syncItem4 = new DeleteFileSyncItem(file2);

            this.target.Add(syncItem1);
            this.target.Add(syncItem2);
            this.target.Add(syncItem3);
            this.target.Add(syncItem4);

            this.target.RemoveUnnecessarySyncItems();

            var deleteDirectorySyncItems = this.target.DeleteDirectorySyncItems;
            Assert.IsTrue(deleteDirectorySyncItems.Count() == 0, "SyncTaskPreview contains wrong number of DeleteDirectorySyncItems.");
            Assert.AreEqual(2, this.target.Count, "SyncTaskPreview contains wrong number of elements.");
        }
    }
}
