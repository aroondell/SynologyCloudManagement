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
        public DynamicButton(string TagValue)
        {
            string[] commandParts = TagValue.Split('-');
            if (commandParts[0] == "UploadRecording")
            {
                MergeRecordingsAndUploadFile(commandParts[1]);
            }
        }

        private void MergeRecordingsAndUploadFile(string dateString)
        {
            USBStorage storage = new USBStorage();
            DateTime date = Convert.ToDateTime(dateString);
            string fileName = storage.MergeVoiceRecordingsAndReturnNewFilePath(date);
            SynoFTPClient client = SynoFTPClient.GetSynoFtpClient();
            client.LoginSecurely();
            string mainFolderName = ConfigurationManager.AppSettings["MainFolderName"].ToString();
            SynoFTPFolder folder = new SynoFTPFolder("/" + mainFolderName);
            folder.UploadFile(fileName);
            client.Logoff();
            storage.DeleteFile(fileName);
        }
    }
}
