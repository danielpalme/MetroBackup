using System.IO;

namespace Palmmedia.BackUp.Synchronization.Test
{
    internal static class FileManager
    {
        public static string GetTestDataDirectory()
        {
            var baseDirectory = new DirectoryInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;
            return Path.Combine(baseDirectory, "Synchronization.Test");
        }
    }
}
