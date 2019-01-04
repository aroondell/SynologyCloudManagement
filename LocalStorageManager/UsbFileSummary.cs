using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalStorageManager
{
    public class UsbFileSummary
    {
        public string ColumnOne { get; set; }
        public string ColumnTwo { get; set; }
        public string ColumnThree { get; set; }
        public string ColumnFour { get; set; }
        public string ColumnFive { get; set; }

        public UsbFileSummary CreateSummaryFromFileList(DateTime date, List<FileInfo> fileInfos)
        {
            UsbFileSummary summary = new UsbFileSummary();
            string dateToString = date.ToString(@"dd-MMM-yyyy");
            summary.ColumnOne = "RecordingsDated-" + dateToString;
            summary.ColumnTwo = fileInfos.Count.ToString();
            long totalSize = fileInfos.Select(a => a.Length).Sum();
            totalSize = totalSize / 1000;
            summary.ColumnThree = $"{string.Format("{0:0.00}", totalSize)}kbs";
            summary.ColumnFour = "Upload";
            summary.ColumnFive = "UploadRecording--" + date.ToString();
            return summary;
        }
    }
}