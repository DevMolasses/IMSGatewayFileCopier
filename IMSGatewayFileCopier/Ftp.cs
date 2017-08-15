using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IMSGatewayFileCopier
{
    class Ftp
    {
        private static Private credentials = new Private();

        public Ftp() { }

        internal static bool FtpFileExists(string ftpFile)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpFile);
            request.Credentials = new NetworkCredential(credentials.username, credentials.password);
            request.Proxy = new WebProxy();
            //request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        internal static bool FtpDirectoryExists(string ftpDirectory)
        {
            string parent = ftpDirectory.Substring(0, ftpDirectory.TrimEnd('/').LastIndexOf('/') + 1);
            string child = ftpDirectory.Substring(ftpDirectory.TrimEnd('/').LastIndexOf('/') + 1).TrimEnd('/');
            var request = (FtpWebRequest)WebRequest.Create(parent);
            request.Credentials = new NetworkCredential(credentials.username, credentials.password);
            request.Proxy = new WebProxy();
            //request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (response == null) return false;
                string data = new StreamReader(response.GetResponseStream(), true).ReadToEnd();
                response.Close();
                return data.IndexOf(child, StringComparison.InvariantCultureIgnoreCase) >= 0;
            }
            catch (WebException)
            {
                return false;
            }
        }

        internal static void FtpCreateDirectory(string ftpDirectory)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpDirectory);
            request.Credentials = new NetworkCredential(credentials.username, credentials.password);
            request.Proxy = new WebProxy();
            //request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException)
            {
                throw;
            }
        }

        internal static void FtpFileCopy(string sourceFile, string ftpFile)
        {
            try
            {
                // Get the object used to communicate with the server.  
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFile);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                //request.KeepAlive = false;

                // This example assumes the FTP site uses anonymous logon.  
                request.Credentials = new NetworkCredential(credentials.username, credentials.password);

                request.Proxy = new WebProxy();

                // Copy the contents of the file to the request stream.  
                StreamReader sourceStream = new StreamReader(sourceFile);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                response.Close();

            }
            catch (Exception)
            {
                Console.WriteLine(DateTime.Now + " - Unable to Copy " + Path.GetFileName(sourceFile));
            }
        }
    }
}
