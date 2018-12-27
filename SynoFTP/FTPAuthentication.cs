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
    class FTPAuthentication
    {
        SynoFTPClient Client;
        private readonly string username = ConfigurationManager.AppSettings["username"].ToString();
        private readonly string password = ConfigurationManager.AppSettings["password"].ToString();

        public FTPAuthentication(SynoFTPClient client)
        {
            Client = client;
        }

        public void FtpLogin()
        {
            FtpClient ftpClient = Client.GetFtpClient();
            ftpClient.Credentials = new System.Net.NetworkCredential(username, password);
            ftpClient.Connect();
        }

        public void FtpsLogin()
        {
            FtpClient ftpClient = Client.GetFtpClient();
            ftpClient.Credentials = new System.Net.NetworkCredential(username, password);
            ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
            ftpClient.SslProtocols = SslProtocols.Tls;
            ftpClient.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
            ftpClient.Connect();
        }


        private void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            // add logic to test if certificate is valid here
            e.Accept = true;
        }
    }
}
