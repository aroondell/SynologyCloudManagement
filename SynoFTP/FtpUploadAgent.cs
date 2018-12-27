using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyFTP
{
    public class FtpUploadAgent : FtpTransferAgent
    {
        public FtpUploadAgent(string FolderPath, string FilePath): base(FolderPath, FilePath)
        {

        }

        public override void TransferFile()
        {
            FtpClient Client = SynoClient.GetFtpClient();
            string remoteFile = GetNewFileName();
            Client.UploadFile(FilePath, remoteFile, FtpExists.Overwrite, false, FtpVerify.None, progress);
        }
    }
}
