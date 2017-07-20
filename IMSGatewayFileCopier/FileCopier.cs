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

        public static void CopyFile(string sourceFile, string sourceDirectory, string destinationDirectory)
        {
            if (!Directory.Exists(destinationDirectory))
                try { Directory.CreateDirectory(destinationDirectory); }
                catch { Console.WriteLine("Can't create destination directory"); }

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
                    if (!File.Exists(destinationFile))
                    {
                        File.Copy(sourceFile, destinationFile);
                        fileCopied = true;
                        break;
                    }
                    else fileSkipped = true;
                }
                catch
                {
                    Thread.Sleep(500); //Sleep for half a second before trying again
                }
            }
            if (!fileCopied && !fileSkipped) Console.WriteLine("Unable to Copy " + Path.GetFileName(sourceFile));
        }

        public static void CopyAllFiles(string sourceDirectory, string destinationDirectory)
        {
            string[] sourceFiles = Directory.GetFiles(sourceDirectory);

            for (int i = 0; i < sourceFiles.Length; i++)
            {
                string sourceFile = sourceFiles[i];
                CopyFile(sourceFile, sourceDirectory, destinationDirectory);
            }
            Console.WriteLine("Finished copying all files");
        }
    }
}
