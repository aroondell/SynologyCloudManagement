using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalStorageManager
{
    public class AudioManipulator
    {
        USBStorage Storage;
        FileInfo[] AllRecordingFiles;

        public string MergeVoiceRecordingsAndReturnNewFilePath(DateTime date)
        {
            LoadFilesFromRecordingStorage();
            VoiceFile voiceFile = new VoiceFile();
            string newFilePath = voiceFile.MergeWavFiles(date, AllRecordingFiles);
            return newFilePath; 
        }

        private void LoadFilesFromRecordingStorage()
        {
            Storage = new USBStorage();
            AllRecordingFiles = Storage.GetAllFilesInRecordFolder();
        }
    }
}
