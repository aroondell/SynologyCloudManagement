using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyFTP
{
    public class SynoFileSummary
    {
        public string ColumnOne { get; set; }
        public string ColumnTwo { get; set; }
        public string ColumnThree { get; set; }

        public string ColumnFour = "Get Transcript";
        public string ColumnFive { get; set; }
        public string Created { get; set; }

        public SynoFileSummary(FtpListItem item)
        {
            ColumnOne = item.Name;
            Double size = item.Size;
            size = size / 1000;
            ColumnThree = $"{string.Format("{0:0.00}", size)}kbs";
            ColumnFive = GetType(item.Name);
            Created = item.Created.ToString("dddd, dd MMMM yyyy");
            ColumnTwo = item.Modified.ToString("dddd, dd MMMM yyyy");
        }

        private string GetType(string name)
        {
            string[] fileParts = name.Split('/');
            string fileName = fileParts[fileParts.Length - 1];
            string[] extentions = fileName.Split('.');
            string extension = extentions[extentions.Length - 1];
            return GetTypeFromExtension(extension);
        }

        private string GetTypeFromExtension(string extension)
        {
            switch (extension)
            {
                case "mp4":
                case "mpg":
                    return "Video";
                default: return "Unknown";
            }
        }
    }
}
