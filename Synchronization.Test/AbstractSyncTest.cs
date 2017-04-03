using System.IO;
using System.Threading;

namespace Palmmedia.BackUp.Synchronization.Test
{
    public abstract class AbstractSyncTest
    {
        protected static readonly string directory_base = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder");

        protected static readonly string file_1newerthan2_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\1newerthan2.txt");
        protected static readonly string file_1newerthan2_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\1newerthan2.txt");

        protected static readonly string file_2newerthan1_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\2newerthan1.txt");
        protected static readonly string file_2newerthan1_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\2newerthan1.txt");

        protected static readonly string file_samedate_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\samedate.txt");
        protected static readonly string file_samedate_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\samedate.txt");

        protected static readonly string file_only1_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\only1.txt");
        protected static readonly string file_only1_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\only1.txt");

        protected static readonly string file_only2_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\only2.txt");
        protected static readonly string file_only2_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\only2.txt");

        protected static readonly string directory_test1_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\test1");
        protected static readonly string directory_test1_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\test1");

        protected static readonly string directory_test2_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\test2");
        protected static readonly string directory_test2_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\test2");

        protected static readonly string directory_test3_1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\1\\test3");
        protected static readonly string directory_test3_2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\2\\test3");

        protected static readonly string directory_test = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\test");
        protected static readonly string directory_testtest = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\test\\test");
        protected static readonly string file1 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\test\\1.txt");
        protected static readonly string file2 = Path.Combine(FileManager.GetTestDataDirectory(), "TestFolder\\test\\2.txt");

        protected static void CreateFiles()
        {
            Directory.CreateDirectory(directory_base);
            Directory.CreateDirectory(directory_test1_1);
            Directory.CreateDirectory(directory_test1_2);
            Directory.CreateDirectory(directory_test2_1);
            Directory.CreateDirectory(directory_test3_2);
            Directory.CreateDirectory(directory_test);
            Directory.CreateDirectory(directory_testtest);

            File.WriteAllText(file_1newerthan2_2, string.Empty);
            File.WriteAllText(file_2newerthan1_1, string.Empty);
            File.WriteAllText(file_samedate_1, string.Empty);
            File.WriteAllText(file_samedate_2, string.Empty);
            File.WriteAllText(file_only1_1, string.Empty);
            File.WriteAllText(file_only2_2, string.Empty);
            File.WriteAllText(file1, string.Empty);
            File.WriteAllText(file2, string.Empty);
            Thread.Sleep(1500);
            File.WriteAllText(file_1newerthan2_1, string.Empty);
            File.WriteAllText(file_2newerthan1_2, string.Empty);
        }

        protected static void DeleteFiles()
        {
            Directory.Delete(directory_base, true);
        }
    }
}
