using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace IMSGatewayFileCopier
{
    class FileCompressor
    {
        public FileCompressor() { }

        public static void CopyAndCompressFile(string sourceFile, string sourceDirectory, string destinationDirectory, bool newFile = false)
        {
            string zipFile = sourceFile.Substring(sourceFile.LastIndexOf(@"\") + 1, 25);
            string zipPath = destinationDirectory + @"\" + zipFile + ".zip";
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry entry = archive.GetEntry(Path.GetFileName(sourceFile));
                if (entry == null) archive.CreateEntryFromFile(sourceFile, Path.GetFileName(sourceFile));
            }
            Console.WriteLine(DateTime.Now + " - Copied {0}", Path.GetFileName(sourceFile));
        }

        public static void CopyAndCompressAllFiles(string sourceDirectory, string destinationDirectory)
        {
            string[][] filesToCompress = GetSortedFilesToCompress(sourceDirectory);
            long totalFilesCompressed = 0;
            foreach(string[] fileGroup in filesToCompress)
            {
                string zipFile = fileGroup[0].Substring(fileGroup[0].LastIndexOf(@"\") + 1, 25);
                string zipPath = destinationDirectory + @"\" + zipFile + ".zip";

                Console.WriteLine(DateTime.Now + " - Compressing {0} ...", zipFile);
                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    foreach (string file in fileGroup)
                    {
                        ZipArchiveEntry entry = archive.GetEntry(Path.GetFileName(file));
                        if (entry == null) archive.CreateEntryFromFile(file, Path.GetFileName(file));
                    }
                }
                totalFilesCompressed += fileGroup.Length;
                Console.WriteLine(DateTime.Now + " - Completed compressing {0} files", fileGroup.Length);
            }
            Console.WriteLine(DateTime.Now + " - Finished copying and compressing {0} files", totalFilesCompressed);
        }

        private static string[][] GetSortedFilesToCompress(string sourceDirectory)
        {
            string[] files = Directory.GetFiles(sourceDirectory);

            List<List<string>> sortedFiles = new List<List<string>>();
            List<string> uniqueDaqHrs = new List<string>();
            foreach (string file in files)
            {
                string daqHr = file.Substring(file.LastIndexOf(@"\") + 1, 25);
                int daqHrIndex = uniqueDaqHrs.IndexOf(daqHr);
                if (daqHrIndex == -1)
                {
                    uniqueDaqHrs.Add(daqHr);
                    daqHrIndex = uniqueDaqHrs.IndexOf(daqHr);
                    sortedFiles.Add(new List<string>());
                }
                sortedFiles.ElementAt(daqHrIndex).Add(file);
            }
            string[][] sortedArray = new string[uniqueDaqHrs.Count][];
            for (int i = 0; i < uniqueDaqHrs.Count; i++)
            {
                sortedArray[i] = sortedFiles.ElementAt(i).ToArray();
            }
            return sortedArray;
        }
    }
}
