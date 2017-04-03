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
    public class SyncPreviewTest
    {
        private static readonly string directory_testtest = "C:\\test";

        private SyncPreview target;

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.target = new SyncPreview();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest_AddOneElement_SyncTaskPreviewBySyncTaskContainsOneNestedSyncItem()
        {
            SyncTask syncTask = new SyncTask("Test");
            CreateDirectorySyncItem syncItem = new CreateDirectorySyncItem("test");

            this.target.Add(syncTask, syncItem);

            Assert.AreEqual(syncItem, target[syncTask].CreateDirectorySyncItems.ElementAt(0), "SyncItem does not match.");
            Assert.AreEqual(1, target[syncTask].CreateDirectorySyncItems.Count(), "CreateDirectorySyncItems should contain 1 element.");
            Assert.AreEqual(1, target.SyncTaskPreviewBySyncTask.Count, "SyncTaskPreviewBySyncTask should contain one element.");
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest_AddTwoElement2_SyncTaskPreviewBySyncTaskContainsTwoNestedSyncItem()
        {
            SyncTask syncTask = new SyncTask("Test");
            CreateDirectorySyncItem syncItem1 = new CreateDirectorySyncItem("test1");
            CreateDirectorySyncItem syncItem2 = new CreateDirectorySyncItem("test2");

            this.target.Add(syncTask, syncItem1);
            this.target.Add(syncTask, syncItem2);

            Assert.AreEqual(2, target[syncTask].CreateDirectorySyncItems.Count(), "CreateDirectorySyncItems should contain 2 elements.");
            Assert.AreEqual(1, target.SyncTaskPreviewBySyncTask.Count, "SyncTaskPreviewBySyncTask should contain one element.");
            Assert.AreEqual(2, target.CountOfActiveCreateDirectories, "CountOfActiveCreateDirectories should contain 2 elements.");
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithoutFilter_SyncTaskNotRemoved()
        {
            SyncTask syncTask = new SyncTask("Test");
            CreateDirectorySyncItem syncItem = new CreateDirectorySyncItem(directory_testtest);

            this.target.Add(syncTask, syncItem);

            this.target.RemoveUnnecessarySyncItems();

            Assert.AreEqual(1, target.SyncTaskPreviewBySyncTask.Count, "SyncTaskPreviewBySyncTask should contain one element.");
        }

        /// <summary>
        ///A test for RemoveUnnecessarySyncItems
        ///</summary>
        [TestMethod()]
        public void RemoveUnnecessarySyncItemsTest_CalledWithFilter_SyncTaskRemoved()
        {
            SyncTask syncTask = new SyncTask("Test");
            syncTask.Filter = "txt";
            CreateDirectorySyncItem syncItem = new CreateDirectorySyncItem(directory_testtest);

            this.target.Add(syncTask, syncItem);

            this.target.RemoveUnnecessarySyncItems();

            Assert.AreEqual(0, target.SyncTaskPreviewBySyncTask.Count, "SyncTaskPreviewBySyncTask should not contain an element.");
        }
    }
}
