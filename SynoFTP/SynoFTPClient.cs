using FluentFTP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SynologyFTP
{
    public class SynoFTPClient
    {
        FtpClient Client;
        public static SynoFTPClient Instance;
        private FTPAuthentication Authentication;
        private SynoFTPClient()
        {
            string FTPUrl = ConfigurationManager.AppSettings["FTPUrl"].ToString();
            Client = new FtpClient(FTPUrl);
            Authentication = new FTPAuthentication(this);
        }

        public static SynoFTPClient GetSynoFtpClient()
        {
            if (Instance == null)
            {
                Instance = new SynoFTPClient();
            }
            return Instance;
        }

        public void Login()
        {
            Authentication.FtpLogin();
        }

        public void LoginSecurely()
        {
            Authentication.FtpsLogin();
        }

        public void Logoff()
        {
            Client.Disconnect();
        }

        public FtpClient GetFtpClient()
        {
            return Client;
        }
    }
}
