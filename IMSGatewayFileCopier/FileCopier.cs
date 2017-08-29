using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace IMSGatewayFileCopier
{
    class FileCopier
    {
        public FileCopier() { }

        public static void CopyFile(string sourceFile, string sourceDirectory, string destinationDirectory, bool newFile = false)
        {
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
            if (!fileCopied && !fileSkipped) Console.WriteLine(DateTime.Now + " - Unable to Copy " + Path.GetFileName(sourceFile));
            if (fileCopied) Console.WriteLine(DateTime.Now + " - Copied " + Path.GetFileName(sourceFile));
            if (fileSkipped) Console.WriteLine(DateTime.Now + " - Skipped " + Path.GetFileName(sourceFile));
        }

        public static void CopyAllFiles(string sourceDirectory, string destinationDirectory)
        {
            DateTime start = DateTime.Now;
            string[] sourceFiles = Directory.GetFiles(sourceDirectory);
            Array.Sort(sourceFiles, (a, b) => string.Compare(a.Substring(a.LastIndexOf(@"\") + 15), b.Substring(b.LastIndexOf(@"\") + 15)));
            for (int i = 0; i < sourceFiles.Length; i++)
            {
                string sourceFile = sourceFiles[i];
                CopyFile(sourceFile, sourceDirectory, destinationDirectory);
                Thread.Sleep(2);
            }
            DateTime finish = DateTime.Now;
            TimeSpan timeLapsed = finish.Subtract(start);
            Console.WriteLine(finish + " - Finished copying {0} files", sourceFiles.Length);
            Console.WriteLine(finish + " - TimeLapsed: {0}", timeLapsed);
        }
    }
}
