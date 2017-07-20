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
        static void Main(string[] args)
        {
            Console.WriteLine("High Speed File Copier \n\r");
            ThreadPool.QueueUserWorkItem(_ => FileCopier.CopyAllFiles(dir.sourceDirectory, dir.destinationDirectory));
            new FileWatcher(dir.sourceDirectory, dir.destinationDirectory);
            
            // Hold thread indefinitely. End program with Red X.
            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        //[PermissionSet(SecurityAction.Demand, Name ="FullTrust")]
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
