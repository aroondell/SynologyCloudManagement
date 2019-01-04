using LocalStorageManager;
using SynologyFTP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyWindows
{
    public class DynamicButton
    {
        readonly string mainFolderName = ConfigurationManager.AppSettings["MainFolderName"].ToString();
        SynoFTPFolder folder;
        SynoFTPClient client;

        public bool StartProcessingClick(string TagValue)
        {
            folder = new SynoFTPFolder("/" + mainFolderName);
            client = SynoFTPClient.GetSynoFtpClient();
            client.LoginSecurely();
            string[] stringSeparators = new string[] { "--" };
            string[] commandParts = TagValue.Split(stringSeparators, StringSplitOptions.None);
            bool result = true;
            if (commandParts[0] == "UploadRecording")
            {
                result = MergeRecordingsAndUploadFile(commandParts[1]);
            }
            else if (commandParts[0] == "DownloadFile")
            {
                result = DownloadFileToSavePath(commandParts[1]);
            }
            return result;
        }

        private bool MergeRecordingsAndUploadFile(string dateString)
        {
            USBStorage storage = new USBStorage();
            DateTime date = Convert.ToDateTime(dateString);
            string fileName = storage.MergeVoiceRecordingsAndReturnNewFilePath(date);
            folder.UploadFile(fileName);
            client.Logoff();
            bool finished = storage.DeleteFileAndLabelPartsAsProcessed(fileName);
            return finished;
        }

        private bool DownloadFileToSavePath(string fileName)
        {
            folder.DownloadFile(fileName);
            client.Logoff();
            return true;
        }
    }
}
