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
        readonly string fileName = "Configuration.json";
        string path;

        public StorageConfiguration()
        {
            path = Path.Combine(Environment.CurrentDirectory, fileName);
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
                StorageJSON jsonConfig = GetStorageJSON();
                jsonConfig.SaveFolder = folder;
                SaveNewConfig(jsonConfig);
                return folder;
            }
            return string.Empty;
        }

        private StorageJSON GetStorageJSON()
        {
            string jsonText = File.ReadAllText(path);
            StorageJSON json = JsonConvert.DeserializeObject<StorageJSON>(jsonText);
            return json;
        }

        private void SaveNewConfig(StorageJSON json)
        {
            string jsonText = JsonConvert.SerializeObject(json);
            File.WriteAllText(path, jsonText);
        }
    }

    public class StorageJSON
    {
        public string SaveFolder { get; set; }
    }
}
