using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalStorageManager
{
    public class USBStorage
    {
        private string DrivePath;
        private Dictionary<DateTime, List<FileInfo>> SavedRecordings;
        public USBStorage()
        {
            string usbName = ConfigurationManager.AppSettings["UsbName"].ToString();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            DrivePath = allDrives.Where(a => a.DriveType == DriveType.Removable && a.VolumeLabel == usbName)
                                .Select(d => d.Name)
                                .FirstOrDefault();
        }

        public FileInfo[] GetAllFilesInRecordFolder()
        {
            string RecordPath = DrivePath + @"\RECORD";
            DirectoryInfo directory = new DirectoryInfo(RecordPath);
            FileInfo[] files = directory.GetFiles("*.WAV");
            return files;
        }

        public string MergeVoiceRecordingsAndReturnNewFilePath(DateTime date)
        {
            AudioManipulator manipulator = new AudioManipulator();
            string fileName = manipulator.MergeVoiceRecordingsAndReturnNewFilePath(date);
            return fileName;
        }

        public List<UsbFileSummary> GetVoiceRecordsFromStorage()
        {
            FileInfo[] recordFiles = GetAllFilesInRecordFolder();
            recordFiles = recordFiles.OrderBy(a => a.CreationTime).ToArray();
            SavedRecordings = new Dictionary<DateTime, List<FileInfo>>();
            foreach(FileInfo recordFile in recordFiles)
            {
                DateTime fileCreationDate = recordFile.CreationTime.Date;
                if (SavedRecordings.ContainsKey(fileCreationDate))
                {
                    SavedRecordings[fileCreationDate].Add(recordFile);
                }
                else
                {
                    List<FileInfo> infoList = new List<FileInfo> { recordFile };
                    SavedRecordings[fileCreationDate] = infoList;
                }
            }
            UsbFileSummary summary = new UsbFileSummary();
            List<UsbFileSummary> fileSummaries = SavedRecordings.Select(a => summary.CreateSummaryFromFileList(a.Key, a.Value)).ToList();
            return fileSummaries;
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
