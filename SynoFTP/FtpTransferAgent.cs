using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyFTP
{
    public abstract class FtpTransferAgent
    {
        protected SynoFTPClient SynoClient;
        protected string FolderPath;
        protected string FilePath;

        public FtpTransferAgent(string folderPath, string filePath)
        {
            SynoClient = SynoFTPClient.GetSynoFtpClient();
            FolderPath = folderPath;
            FilePath = filePath;
        }

        public abstract void TransferFile();
        protected string GetNewFileName()
        {
            string[] filePathParts = FilePath.Split('/');
            string fileName = filePathParts[filePathParts.Length - 1];
            string remoteFile = FolderPath + "/" + fileName;
            return remoteFile;
        }

        protected Progress<double> progress = new Progress<double>(x =>
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
