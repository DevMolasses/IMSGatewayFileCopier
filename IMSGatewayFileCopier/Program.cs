using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace IMSGatewayFileCopier
{
    class Program
    {
        private static Directories dir = new Directories();
        static FileWatcher fileWatcher = new FileWatcher(dir.sourceDirectory, dir.destinationDirectory);
        FileCopier fileCopier = new FileCopier();
        static bool fileWatcherEnabled = false;
        static bool allFilesCopied = false;

        static void Main(string[] args)
        {
            while (true)
            {
                if (!Directory.Exists(dir.destinationDirectory))
                {
                    Console.WriteLine(DateTime.Now + " - Can't find Destination Directory");
                    fileWatcher.DisableWatcher();
                    fileWatcherEnabled = false;
                    allFilesCopied = false;
                    try
                    {
                        Directory.CreateDirectory(dir.destinationDirectory);
                        Console.WriteLine(DateTime.Now + " - Destination Directory created");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(DateTime.Now + " - Destination Directory could not be created");
                        Thread.Sleep(5000);
                        continue;
                    }
                }
                if(!allFilesCopied)
                {
                    ThreadPool.QueueUserWorkItem(_ => FileCopier.CopyAllFiles(dir.sourceDirectory, dir.destinationDirectory));
                    allFilesCopied = true;
                }
                if (!fileWatcherEnabled)
                {
                    fileWatcher.EnableWatcher();
                    fileWatcherEnabled = true;
                }
                Thread.Sleep(100);
            }
        }
    }
}
