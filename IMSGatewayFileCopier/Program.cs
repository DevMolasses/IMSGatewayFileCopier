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
        static bool fileWatcherEnabled = false;
        static bool allFilesCopied = false;

        static void Main(string[] args)
        {
            Log.NewEntry("BeganIMSFileCopier");
            while (true)
            {
                if (!Directory.Exists(dir.destinationDirectory))
                {
                    Log.NewEntry("Can't find Destination Directory", dir.destinationDirectory);
                    fileWatcher.DisableWatcher();
                    fileWatcherEnabled = false;
                    allFilesCopied = false;
                    try
                    {
                        Directory.CreateDirectory(dir.destinationDirectory);
                        Log.NewEntry("Destination Directory created", dir.destinationDirectory);
                    }
                    catch (Exception)
                    {
                        Log.NewEntry("Destination Directory could not be created", dir.destinationDirectory);
                        Thread.Sleep(5000);
                        continue;
                    }
                }
                if (!allFilesCopied)
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
