using System;
using System.Linq;
using Palmmedia.BackUp.SharedResources;
using Palmmedia.BackUp.Synchronization;
using Palmmedia.BackUp.Synchronization.SyncModes;
using Palmmedia.BackUp.UI.Console.Properties;

namespace Palmmedia.BackUp.UI.Console
{
    /// <summary>
    /// The main class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Return code indicating success/failure.</returns>
        internal static int Main(string[] args)
        {
            // Show help
            if (args.Length == 0
                || args.Contains("HELP", StringComparer.OrdinalIgnoreCase)
                || args.Contains("-H", StringComparer.OrdinalIgnoreCase)
                || args.Contains("--H", StringComparer.OrdinalIgnoreCase)
                || args.Contains("/?", StringComparer.OrdinalIgnoreCase))
            {
                ShowHelp();
                return 0;
            }
            else if (args.Length > 2 && args.Length < 7)
            {
                SyncTask syncTask = new SyncTask("Console");

                try
                {
                    syncTask.SyncModeType = GetSyncMode(args[0]);
                }
                catch (ArgumentException)
                {
                    ShowWrongParameters();
                    return 1;
                }

                syncTask.ReferenceDirectory = args[1];
                syncTask.TargetDirectory = args[2];

                syncTask.Recursive = !args.Contains("/NORECURSION", StringComparer.OrdinalIgnoreCase);

                // Filter
                for (int i = 3; i < args.Length; i++)
                {
                    if (args[i].StartsWith("/FILTER:", StringComparison.OrdinalIgnoreCase))
                    {
                        syncTask.Filter = args[i].Substring(8);
                        break;
                    }
                }

                // Excluded sub directories
                for (int i = 3; i < args.Length; i++)
                {
                    if (args[i].StartsWith("/EXCLUDEDSUBDIRECTORIES:", StringComparison.OrdinalIgnoreCase))
                    {
                        syncTask.ExcludedSubdirectories = args[i].Substring(24);
                        break;
                    }
                }

                bool success = new ConsoleSynchronizer().Synchronize(syncTask);
                return success ? 0 : 1;
            }
            else
            {
                ShowWrongParameters();
                return 1;
            }
        }

        /// <summary>
        /// Gets the sync mode.
        /// </summary>
        /// <param name="mode">The mode as <see cref="string"/>.</param>
        /// <returns>The sync mode.</returns>
        private static SyncModeType GetSyncMode(string mode)
        {
            if (mode.Equals("LeftToRight", StringComparison.OrdinalIgnoreCase) || mode.Equals("LTR", StringComparison.OrdinalIgnoreCase))
            {
                return SyncModeType.LeftToRight;
            }
            else if (mode.Equals("LeftToRightWithoutDeletion", StringComparison.OrdinalIgnoreCase) || mode.Equals("LTRWD", StringComparison.OrdinalIgnoreCase))
            {
                return SyncModeType.LeftToRightWithoutDeletion;
            }
            else if (mode.Equals("BothWays", StringComparison.OrdinalIgnoreCase) || mode.Equals("BW", StringComparison.OrdinalIgnoreCase))
            {
                return SyncModeType.BothWays;
            }
            else
            {
                throw new ArgumentException("Invalid SyncMode.", "mode");
            }
        }

        /// <summary>
        /// Used if parameters are not correct
        /// </summary>
        private static void ShowWrongParameters()
        {
            System.Console.WriteLine();
            System.Console.WriteLine(Resources.WrongParameters);
            ShowHelp();
        }

        /// <summary>
        /// Shows the help of the programm
        /// </summary>
        private static void ShowHelp()
        {
            System.Console.WriteLine();
            System.Console.WriteLine(Resources.Commands + ":");
            System.Console.WriteLine("   {LTR|LEFTTORIGHT|LTRWD|LEFTTORIGHTWITHOUTDELETION|BW|BOTHWAYS}"
                                  + " \"" + Common.ReferenceDirectory
                                  + "\" \"" + Common.TargetDirectory
                                  + "\" [/NORECURSION]"
                                  + " [/FILTER:" + Resources.Extension + "[," + Resources.Extension + "]*]"
                                  + " [/EXCLUDEDSUBDIRECTORIES:" + Resources.DirectoryName + "[," + Resources.DirectoryName + "]*]");
            System.Console.WriteLine();
            System.Console.WriteLine(Resources.Explanation + ":");
            System.Console.WriteLine("   LTR, LEFTTORIGHT: " + Common.SyncModeType_LeftToRight);
            System.Console.WriteLine("   LTRWD, LEFTTORIGHTWITHOUTDELETION: " + Common.SyncModeType_LeftToRightWithoutDeletion);
            System.Console.WriteLine("   BW, BOTHWAYS: " + Common.SyncModeType_BothWays);
            System.Console.WriteLine("   NORECURSION: " + Resources.RecursiveExplanation);
            System.Console.WriteLine("   FILTER: " + Resources.FilterExplanation);
            System.Console.WriteLine("   EXCLUDEDSUBDIRECTORIES: " + Resources.ExcludedSubdirectoriesExplanation);
            System.Console.WriteLine();
            System.Console.WriteLine(Resources.DefaultValues + ":");
            System.Console.WriteLine("   NORECURSION: " + Resources.RecursiveDefaultValues);
            System.Console.WriteLine("   FILTER: " + Resources.FilterDefaultValues);
            System.Console.WriteLine("   EXCLUDEDSUBDIRECTORIES: " + Resources.ExcludedSubdirectoriesDefaultValues);
            System.Console.WriteLine();
            System.Console.WriteLine(Resources.Examples + ":");
            System.Console.WriteLine("   LEFTTORIGHT \"C:\\\" \"D:\\\"");
            System.Console.WriteLine("   BOTHWAYS \"C:\\\" \"D:\\\" /NORECURSION");
            System.Console.WriteLine("   BOTHWAYS \"C:\\\" \"D:\\\" /FILTER:xml,exe,dll /EXCLUDEDSUBDIRECTORIES:node_modules");
        }
    }
}
