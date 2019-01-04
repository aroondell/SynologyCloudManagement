using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalStorageManager
{
    public class AudioManipulator
    {
        USBStorage Storage;
        FileInfo[] AllViableRecordingFiles;

        public ExpandoObject MergeVoiceRecordingsAndReturnNewFilePath(DateTime date)
        {
            LoadFilesFromRecordingStorage();
            VoiceFile voiceFile = new VoiceFile();
            FileInfo[] chosenFiles = AllViableRecordingFiles.Where(a => a.CreationTime.Date == date.Date).ToArray();
            string newFilePath = voiceFile.MergeWavFiles(date, chosenFiles);
            if (!String.IsNullOrEmpty(newFilePath))
            {
                dynamic expeando = new ExpandoObject();
                List<RecordPart> recordParts = StoreRecordParts(chosenFiles);
                expeando.FilePath = newFilePath;
                expeando.RecordParts = recordParts;
                return expeando;
            }
            return null;
        }

        private void LoadFilesFromRecordingStorage()
        {
            Storage = new USBStorage();
            AllViableRecordingFiles = Storage.GetAllViableFilesInRecordFolder();
        }

        private List<RecordPart> StoreRecordParts(FileInfo[] files)
        {
            List<RecordPart> recordParts = files.Select(a => new RecordPart
            {
                CreationTime = a.CreationTime,
                Size = a.Length
            }).ToList();
            return recordParts;                                   
        }
    }
}
