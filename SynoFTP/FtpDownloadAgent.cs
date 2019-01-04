using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyFTP
{
    class FtpDownloadAgent : FtpTransferAgent
    {

        public FtpDownloadAgent(string TargetPath, string SourcePath) : base(TargetPath, SourcePath)
        {
        }

        public override void TransferFile()
        {
            FtpClient Client = SynoClient.GetFtpClient();
            string localSave = GetNewFileName();
            Client.DownloadFile(localSave, SourcePath, false, FtpVerify.None, progress);
        }
    }
}
