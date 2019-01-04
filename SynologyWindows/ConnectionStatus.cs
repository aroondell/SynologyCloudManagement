using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SynologyWindows
{
    public class ConnectionStatus
    {
        private BackgroundWorker StatusWorker;
        private Label Label;

        public ConnectionStatus(Label label)
        {
            Label = label;
            StatusWorker = new BackgroundWorker();
            StatusWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
            StatusWorker.RunWorkerAsync();
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending)
            {               
                SetConnectivityStatus(CheckConnectivityToServer());
                Thread.Sleep(5000);
            }
        }

        private void SetConnectivityStatus(bool connectionStatus)
        {
            string RedBackground = "#FFFD4040";
            string GreenBackground = "#FF58FD40";
            BrushConverter bc = new BrushConverter();
            if (connectionStatus)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Label.Content = "Connected";
                    Label.Background = (Brush)bc.ConvertFrom(GreenBackground);
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Label.Content = "No Connection";
                    Label.Background = (Brush)bc.ConvertFrom(RedBackground);
                });
            }
        }

        private bool CheckConnectivityToServer()
        {
            string PingAddress = ConfigurationManager.AppSettings["PingAddress"].ToString();
            PingReply pingReply;
            using (var ping = new Ping()) pingReply = ping.Send(PingAddress);
            return pingReply.Status == IPStatus.Success;
        }
    }
}
