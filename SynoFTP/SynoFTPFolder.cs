using FluentFTP;
using LocalStorageManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyFTP
{
    public class SynoFTPFolder
    {
        string FolderPath;
        List<FtpListItem> FolderContents;
        FtpClient Client;

        public SynoFTPFolder(string folderPath)
        {
            FolderPath = folderPath;
            Client = SynoFTPClient.GetSynoFtpClient().GetFtpClient();
            RetrieveFolderContents();
        }

        public List<FtpListItem> RetrieveFilesFromFolder()
        {
            return FolderContents.Where(a => a.Type == FtpFileSystemObjectType.File).ToList();
        }

        public List<FtpListItem> RetrieveFoldersWithinFolder()
        {
            return FolderContents.Where(a => a.Type == FtpFileSystemObjectType.Directory).ToList();
        }

        public void UploadFile(string FilePath)
        {
            FtpUploadAgent uploadAgent = new FtpUploadAgent(FolderPath, FilePath);
            uploadAgent.TransferFile();
        }

        public void DownloadFile(string FileName)
        {
            string SaveDirectory = StorageConfiguration.GetSaveDirectory();
            string sourceFile = FolderPath + '/' + FileName;
            FtpDownloadAgent downloadAgent = new FtpDownloadAgent(SaveDirectory, sourceFile);
            downloadAgent.TransferFile();
        }

        private void RetrieveFolderContents()
        {
            FolderContents = Client.GetListing(FolderPath).ToList();
        }

        private Progress<double> progress = new Progress<double>(x =>
        {
            Debug.WriteLine(x);
            if (x < 0)
            {
            }
            else
            {
            }
        });
    }
}
