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

            ////Console.WriteLine("High Speed File Copier \n\r");
            //EstablishConnectionsToDirectories();
            //ThreadPool.QueueUserWorkItem(_ => FileCopier.CopyAllFiles(dir.sourceDirectory, dir.destinationDirectory));
            //fileWatcher.EnableWatcher();
            //new Thread(delegate ()
            //{
            //    MaintainConnectionsToDirectories();
            //}).Start();
            //// Hold main thread indefinitely. End program with Red X.
            //while (true)
            //{
            //    Thread.Sleep(10000);
            //}
        }

        //static void EstablishConnectionsToDirectories()
        //{
        //    if(!Directory.Exists(dir.sourceDirectory))
        //    {
        //        Console.WriteLine("{0} - {1} can't be found.", DateTime.Now, dir.sourceDirectory);
        //        Console.WriteLine(DateTime.Now + " - Waiting for Source Directory to be found...");
        //        while (!Directory.Exists(dir.sourceDirectory)) Thread.Sleep(1000);
        //    }
        //    Console.WriteLine("Connected to Source Directory");
        //    if (!Directory.Exists(dir.destinationDirectory))
        //    {
        //        Console.WriteLine("{0} can't be found.", dir.sourceDirectory);
                
        //        try
        //        {
        //            Console.WriteLine("Trying To creat Destination Directory...");
        //            Directory.CreateDirectory(dir.destinationDirectory);
        //        }
        //        catch
        //        {

        //            while (!Directory.Exists(dir.destinationDirectory)) Thread.Sleep(1000);
        //        }
                
        //    }
        //    Console.WriteLine("Connected to Destination Directory");
        //}

        //private static void MaintainConnectionsToDirectories()
        //{

        //}

        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //public static void Run()
        //{
        //    Directories dir = new Directories();

        //    StartFileSystemWatcher(dir.sourceDirectory);

        //    // Wait for the user to quit the program.
        //    Console.WriteLine("Press \'q\' to quit the sample.");
        //    while (Console.Read() != 'q') ;
        //}

        //private static void StartFileSystemWatcher(string sourceDirectory)
        //{
        //    // Create a new FileSystemWatcher and set its properties.
        //    FileSystemWatcher watcher = new FileSystemWatcher();
        //    watcher.Path = sourceDirectory;
        //    /* Watch for changes in LastAccess and LastWrite times, and
        //       the renaming of files or directories. */
        //    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
        //       | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        //    // Only watch text files.
        //    watcher.Filter = "*.txt";

        //    // Add event handlers.
        //    watcher.Changed += new FileSystemEventHandler(OnChanged);
        //    watcher.Created += new FileSystemEventHandler(OnChanged);
        //    watcher.Deleted += new FileSystemEventHandler(OnChanged);
        //    watcher.Renamed += new RenamedEventHandler(OnRenamed);

        //    // Begin watching.
        //    watcher.EnableRaisingEvents = true;
        //}

        //// Define the event handlers.
        //private static void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    // Specify what is done when a file is changed, created, or deleted.
        //    Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        //}

        //private static void OnRenamed(object source, RenamedEventArgs e)
        //{
        //    // Specify what is done when a file is renamed.
        //    Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        //}
    }
}
