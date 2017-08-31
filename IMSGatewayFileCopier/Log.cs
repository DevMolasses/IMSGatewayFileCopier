using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSGatewayFileCopier
{
    class Log
    {
        public Log() { }

        private static string logDir = "./logs";

        public static void NewEntry(params string[] messages)
        {
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);

            using (StreamWriter sw = File.AppendText(GetLogFilePath()))
            {
                sw.Write("\r\nLog Entry: ");
                sw.WriteLine(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss.000"));
                sw.WriteLine(" :");
                foreach(string message in messages)
                {
                    sw.WriteLine(" :{0}", message);
                }
                sw.WriteLine("--------------------------------------------");
            }
            foreach (string message in messages)
            {
                Console.WriteLine(DateTime.Now + " - " + message);
            }
        }

        private static string GetLogFilePath()
        {
            DateTime now = DateTime.Now;
            StringBuilder logFileName = new StringBuilder(logDir);
            logFileName.Append("/Log_");
            logFileName.Append(now.Year);
            logFileName.Append(Month_MMM(now));
            logFileName.Append(".txt");
            return logFileName.ToString();
        }

        private static string Month_MMM(DateTime now)
        {
            int m = now.Month;
            switch (m)
            {
                case 1:
                    return "JAN";
                case 2:
                    return "FEB";
                case 3:
                    return "MAR";
                case 4:
                    return "APR";
                case 5:
                    return "MAY";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AUG";
                case 9:
                    return "SEP";
                case 10:
                    return "OCT";
                case 11:
                    return "NOV";
                case 12:
                    return "DEC";
                default:
                    return "";
            }
        }
    }
}
