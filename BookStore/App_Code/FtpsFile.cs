using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BookStore.App_Code;

namespace BookStore.App_Code
{
    class FtpsFile
    {
        /// <summary>
        /// first c
        /// </summary>

        public static string CreateFTPDirectory(string DirectoryName, string OrderNumber)
        {
            try
            {
                string path = ConstantMessage.CONST_FTPUrl + DirectoryName + "/";

                //check is already exists

                bool IsExists = DirectoryExists(path);

                if (!IsExists)
                {
                    WebRequest request = WebRequest.Create(path);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);
                    using (var resp = (FtpWebResponse)request.GetResponse())
                    {
                        IsExists = true;
                    }
                }
                if (IsExists)
                {

                    //now create another 
                    path = path + OrderNumber + "/";

                    //now check is OrderNumber directory exists or not
                    bool IsOrderFileExists = DirectoryExists(path);

                    if (IsOrderFileExists)
                    {
                        //to do delete the Directory and then create again 
                        DeleteFTPDirectory(path);
                    }

                    //now create another Directory named   Order
                    WebRequest request2 = WebRequest.Create(path);
                    request2.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request2.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);

                    using (var resp2 = (FtpWebResponse)request2.GetResponse())
                    {
                        return path;
                    }
                }


                return null;

            }
            catch (Exception ex)
            {

                string msg = "Error:" + ex.Message;
                return msg;

            }

        }


        //directory = @"ftp://ftp.example.com/Rubicon/";
        public static bool DirectoryExists(string directoryPath)   //directory
        {

            bool directoryExists;


            // directory = ConstantMessage.CONST_FTPUrl + directory + "/";

            var request = (FtpWebRequest)WebRequest.Create(directoryPath);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);

            //added 26-09-16
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            //

            try
            {
                using (request.GetResponse())
                {
                    directoryExists = true;
                }
            }
            catch (WebException ex)
            {
                string msg = ex.Message;  //msg=The remote server returned an error: (550) File unavailable (e.g., file not found, no access).
                directoryExists = false;
            }



            return directoryExists;
        }


        #region Delete Inside File and Directory
        /// <summary>
        /// Delete FTP Directory 
        /// </summary>

        public static void delete(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                var ftpRequest = (FtpWebRequest)WebRequest.Create(deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.ToString()); 
                try
                {
                    // deleteDirectory(deleteFile);
                }
                catch { }


            }
            return;
        }
        public static void DeleteFTPDirectory(string directoryPath)
        {
            try
            {
                //Check files inside 
                var direcotryChildren = directoryListSimple(directoryPath);
                if (direcotryChildren.Any() && (!string.IsNullOrWhiteSpace(direcotryChildren[0])))
                {
                    foreach (var child in direcotryChildren)
                    {
                        delete(directoryPath + "/" + child);
                    }
                }

                /* Create an FTP Request */
                var ftpRequest = (FtpWebRequest)WebRequest.Create(directoryPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;


            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString()); 
                throw ex;
            }
            return;
        }



        /* List Directory Contents File/Folder Name Only */
        public static string[] directoryListSimple(string directory)
        {
            try
            {
                /* Create an FTP Request */
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                var ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] directoryListDetailed(string directory)
        {
            try
            {
                /* Create an FTP Request */
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                var ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }
        #endregion

        public static bool UploadFileToFTP(string FileLocalPath, string DistinationPath, string fileName)
        {
            try
            {


                //string Filename = Path.GetFileName(FileLocalPath);
                string CONST_FTPUrl = DistinationPath + "/" + fileName;   // @"ftp://204.11.58.210/Test File Upload/" + fileName;

                int bufferSize = 2048;

                int total_bytes = 0;

                FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(CONST_FTPUrl);
                ftpClient.Credentials = new System.Net.NetworkCredential(ConstantMessage.CONST_UserName, ConstantMessage.CONST_Pass);
                ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                ftpClient.UseBinary = true;
                ftpClient.KeepAlive = true;

                System.IO.FileInfo fi = new System.IO.FileInfo(FileLocalPath);

                //Edited
                ftpClient.ContentLength = fi.Length;
                byte[] buffer = new byte[4097];
                int bytes = 0;
                total_bytes = (int)fi.Length;
                System.IO.FileStream fs = fi.OpenRead();
                System.IO.Stream rs = ftpClient.GetRequestStream();
                while (total_bytes > 0)
                {
                    bytes = fs.Read(buffer, 0, buffer.Length);
                    rs.Write(buffer, 0, bytes);
                    total_bytes = total_bytes - bytes;
                }
                //fs.Flush();
                fs.Close();
                rs.Close();
                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                var value = uploadResponse.StatusDescription;
                uploadResponse.Close();

                return true;
            }
            catch (Exception)
            {
                //throw ex;
                return false;
            }
        }

        //check is given directory already exists
        //public string directoryExists(string directory, string mainDirectory)
        //{
        //    try
        //    {
        //        var list = this.GetFileList(mainDirectory);
        //        if (list != null)
        //        {
        //            if (list.Contains(directory))
        //                return "true";
        //            else
        //                return "false";
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception)
        //    {
        //       // Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}

        //public string[] GetFileList(string path)
        //{
        //    var ftpPath = ConstantMessage.CONST_FTPUrl + "/" + path;
        //    var ftpUser = ConstantMessage.CONST_UserName;
        //    var ftpPass = ConstantMessage.CONST_Pass;
        //    var result = new StringBuilder();
        //    try
        //    {
        //        var strLink = ftpPath;
        //        var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(strLink));
        //        reqFtp.UseBinary = true;
        //        reqFtp.Credentials = new NetworkCredential(ftpUser, ftpPass);
        //        reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
        //        reqFtp.Proxy = null;
        //        reqFtp.KeepAlive = false;
        //        reqFtp.UsePassive = true;
        //        using (var response = reqFtp.GetResponse())
        //        {
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var line = reader.ReadLine();
        //                while (line != null)
        //                {
        //                    result.Append(line);
        //                    result.Append("\n");
        //                    line = reader.ReadLine();
        //                }
        //                result.Remove(result.ToString().LastIndexOf('\n'), 1);
        //            }
        //        }
        //        return result.ToString().Split('\n');
        //    }
        //    catch (Exception)
        //    {
        //      //  Console.WriteLine("FTP ERROR: ", ex.Message);
        //        return null;
        //    }

        //    //finally
        //    //{
        //    //    ftpRequest = null;
        //    //}
        //}
    }
}
