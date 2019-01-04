using SynologyFTP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyWindows
{
    public class SynoMainList
    {
        public static List<SynoFileSummary> GetFileSummariesFromMainFolder()
        {
            SynoFTPClient Client = SynoFTPClient.GetSynoFtpClient();            
            Client.LoginSecurely();
            string mainFolderName = ConfigurationManager.AppSettings["MainFolderName"].ToString();
            Client.LoadTargetFolder(mainFolderName);
            List<SynoFileSummary> fileSummaries = Client.GetFilesFromFolder();
            Client.Logoff();
            return fileSummaries;
        }
    }
}
