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
        protected string TargetPath;
        protected string SourcePath;

        public FtpTransferAgent(string targetPath, string sourcePath)
        {
            SynoClient = SynoFTPClient.GetSynoFtpClient();
            TargetPath = targetPath;
            SourcePath = sourcePath;
        }

        public abstract void TransferFile();
        protected string GetNewFileName()
        {
            string[] filePathParts = SourcePath.Split('/');
            string fileName = filePathParts[filePathParts.Length - 1];
            string targetFile = TargetPath + "/" + fileName;
            return targetFile;
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
