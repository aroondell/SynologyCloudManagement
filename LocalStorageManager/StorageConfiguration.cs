using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

namespace LocalStorageManager
{
    public class StorageConfiguration
    {
        readonly static string fileName = "Configuration.json";
        string path;
        ConfigurationJSON config;

        public StorageConfiguration()
        {
            path = Path.Combine(Environment.CurrentDirectory, fileName);
            config = GetStorageJSON();
        }

        public string SelectDirectory()
        {
            string currentUserDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose Save Folder";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = currentUserDirectory;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = currentUserDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folder = dlg.FileName;
                config.SaveFolder = folder;
                SaveNewConfig();
                return folder;
            }
            return string.Empty;
        }

        public bool HasRecordNotBeenProcessed(FileInfo fileInfo)
        {
            RecordPart record = new RecordPart
            {
                CreationTime = fileInfo.CreationTime,
                Size = fileInfo.Length
            };
            List<RecordPart> records = config.RecordsProcessed;
            if (records != null)
            {
                RecordPart recordCheck = config.RecordsProcessed
                            .Where(a => a.CreationTime == record.CreationTime && a.Size == record.Size)
                            .FirstOrDefault();
                return recordCheck == null;
            }
            return true;
        }

        public static string GetSaveDirectory()
        {
            string Path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
            string jsonText = File.ReadAllText(Path);
            ConfigurationJSON config= JsonConvert.DeserializeObject<ConfigurationJSON>(jsonText);
            return config.SaveFolder;
        }

        public bool StorePartsAsProcessed(List<RecordPart> recordParts)
        {
            if (config.RecordsProcessed == null)
            {
                config.RecordsProcessed = recordParts;
            }
            else
            {
                config.RecordsProcessed.AddRange(recordParts);
            }
            SaveNewConfig();
            return true;
        }

        private ConfigurationJSON GetStorageJSON()
        {
            string jsonText = File.ReadAllText(path);
            ConfigurationJSON json = JsonConvert.DeserializeObject<ConfigurationJSON>(jsonText);
            return json;
        }

        private void SaveNewConfig()
        {
            string jsonText = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, jsonText);
        }
    }

    public class ConfigurationJSON
    {
        public string SaveFolder { get; set; }
        public List<RecordPart> RecordsProcessed { get; set; }
    }

    public class RecordPart
    {
        public DateTime CreationTime { get; set; }
        public long Size { get; set; }
    }
}
