using System;
using System.IO;
using System.Threading;

namespace IMSGatewayFileCopier
{
    class FileWatcher : FileSystemWatcher
    {
        private FileWatcher fileWatcher { get; set; }
        private FileWatcher() { }
        /// <summary>
        /// Establishes a SystemFileWatcher for the purpose of copying files from the
        /// source directory to the destination directory as soon as the files are
        /// created in the source directory
        /// </summary>
        /// <param name="src">Source Directory</param>
        /// <param name="dest">Destination Directory</param>
        public FileWatcher(string src, string dest)
        {
            fileWatcher = new FileWatcher();
            try
            {
                fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                    NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //fileWatcher.Filter = "*.txt";
                fileWatcher.Created += new FileSystemEventHandler((sender, e) => OnCreated(sender, e, src, dest));
                fileWatcher.Renamed += new RenamedEventHandler((sender, e) => OnRenamed(sender, e, src, dest));
                fileWatcher.IncludeSubdirectories = false;
                fileWatcher.Path = src;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(DateTime.Now + " - " + e);
                throw;
            }
        }

        /// <summary>
        /// Enables the FileWatcher to raise events and logs that it has done so
        /// </summary>
        public void EnableWatcher()
        {
            fileWatcher.EnableRaisingEvents = true;
            Console.WriteLine(DateTime.Now + " - Enabled file watcher");
        }

        /// <summary>
        /// Disables the FileWatcher from raising events and logs that it has done so
        /// </summary>
        public void DisableWatcher()
        {
            fileWatcher.EnableRaisingEvents = false;
            Console.WriteLine(DateTime.Now + " - Disabled file watcher");
        }

        /// <summary>
        /// Copies all files the FileWatcher catches as Renamed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="src">Source Directory</param>
        /// <param name="dest">Destination Directory</param>
        private void OnRenamed(object sender, RenamedEventArgs e, string src, string dest)
        {
            Console.WriteLine(DateTime.Now + " - File Renamed");
            ThreadPool.QueueUserWorkItem(_ => FileCopier.CopyFileToFtp(e.FullPath, src, dest));
        }

        /// <summary>
        /// Copies all files the FileWatcher catches as Created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="src">Source Directory</param>
        /// <param name="dest">Destination Directory</param>
        private void OnCreated(object sender, FileSystemEventArgs e, string src, string dest)
        {
            Console.WriteLine(DateTime.Now + " - File Created");
            ThreadPool.QueueUserWorkItem(_ => FileCopier.CopyFileToFtp(e.FullPath, src, dest));
        }
    }
}
