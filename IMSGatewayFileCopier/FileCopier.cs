using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace IMSGatewayFileCopier
{
    class FileCopier
    {
        public FileCopier() { }

        public static int CopyFile(string sourceFile, string sourceDirectory, string destinationDirectory, bool newFile = false)
        {
            string destinationFile = sourceFile.Replace(sourceDirectory, destinationDirectory);

            int tries = 0;
            int allowedTries = 100;
            bool fileCopied = false;
            bool fileSkipped = false;
            while (tries <= allowedTries)
            {
                tries++;
                try
                {
                    string destinationExt = Path.GetFileName(sourceFile).Substring(0, 25);
                    destinationDirectory = destinationDirectory + @"\" + destinationExt;
                    string destinationFile = sourceFile.Replace(sourceDirectory, destinationDirectory);

                    if (!Directory.Exists(destinationDirectory)) Directory.CreateDirectory(destinationDirectory);

                    if (!newFile)
                    {
                        if (!File.Exists(destinationFile))
                        {
                            File.Copy(sourceFile, destinationFile);
                            fileCopied = true;
                            break;
                        }
                        else
                        {
                            fileSkipped = true;
                            break;
                        }
                    }
                    else
                    {
                        File.Copy(sourceFile, destinationFile);
                        fileCopied = true;
                        break;
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(DateTime.Now + " - File has been overwritten...");
                    Console.WriteLine(e.FileName);
                    break;
                }
                catch (PathTooLongException e)
                {
                    Console.WriteLine(DateTime.Now + " - Path too long error...");
                    Console.WriteLine(e.Data.ToString());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(DateTime.Now + " - Problem copying File");
                    Console.WriteLine(e.ToString());
                    Thread.Sleep(500); //Sleep for half a second before trying again
                }
            }
            if (!fileCopied && !fileSkipped)
            {
                Console.WriteLine(DateTime.Now + " - Unable to Copy " + Path.GetFileName(sourceFile));
                return 0;
            }
            if (fileCopied) {
                Console.WriteLine(DateTime.Now + " - Copied " + Path.GetFileName(sourceFile));
                return 1;
            }
            if (fileSkipped)
            {
                Console.WriteLine(DateTime.Now + " - Skipped " + Path.GetFileName(sourceFile));
                return 2;
            }
            return 0;
        }

        public static void CopyAllFiles(string sourceDirectory, string destinationDirectory)
        {
            DateTime start = DateTime.Now;
            long filesCopied = 0, filesSkipped = 0, filesErrored = 0;
            string[] sourceFiles = Directory.GetFiles(sourceDirectory);
            Log.NewEntry("Starting to copy all files", "Total Files - " + sourceFiles.Length);
            Array.Sort(sourceFiles, (a, b) => string.Compare(a.Substring(a.LastIndexOf(@"\") + 15), b.Substring(b.LastIndexOf(@"\") + 15)));
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < sourceFiles.Length; i++)
            {
                timer.Restart();
                string sourceFile = sourceFiles[i];
                int copied = CopyFile(sourceFile, sourceDirectory, destinationDirectory);
                switch (copied)
                {
                    case 0:
                        filesErrored++;
                        break;
                    case 1:
                        filesCopied++;
                        break;
                    case 2:
                        filesSkipped++;
                        break;
                }
                while (timer.ElapsedMilliseconds < 25) { Thread.Sleep(1); }
            }
            DateTime finish = DateTime.Now;
            TimeSpan timeLapsed = finish.Subtract(start);
            Log.NewEntry("Finished copying all files", 
                         "Files Copied - " + filesCopied, 
                         "Files Skipped - " + filesSkipped, 
                         "Files Errored Out - " + filesErrored,
                         "Time Lapsed - " + timeLapsed);
        }
    }
}
