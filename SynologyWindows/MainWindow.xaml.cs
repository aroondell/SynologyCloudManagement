using LocalStorageManager;
using SynologyFTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynologyWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ListBox mainList;
        ListBox mainListTitle;
        TextBlock saveDirectory;
        StorageConfiguration Configuration;
        public MainWindow()
        {
            InitializeComponent();
            mainList = (ListBox)this.FindName("MainList");
            mainListTitle = (ListBox)this.FindName("MainListTitle");
            saveDirectory = (TextBlock)this.FindName("SaveDirectory");
            Configuration = new StorageConfiguration();
            string currentSave = StorageConfiguration.GetSaveDirectory();
            saveDirectory.Text = $"Save Folder: {currentSave}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var col = Grid.GetRow((Button)sender);
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            MainListTitleStruct title = new MainListTitleStruct
            {
                Title1 = "Date",
                Title2 = "Recordings",
                Title3 = "Size",
                Title4 = ""
            };
            List<MainListTitleStruct> titles = new List<MainListTitleStruct>
            {
                title
            };
            mainListTitle.ItemsSource = titles;
            USBStorage storage = new USBStorage();
            List<UsbFileSummary> fileSummaries = storage.GetVoiceRecordsFromStorage();
            mainList.ItemsSource = fileSummaries;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            MainListTitleStruct title = new MainListTitleStruct
            {
                Title1 = "Name",
                Title2 = "Modified",
                Title3 = "Size",
                Title4 = ""
            };
            List<MainListTitleStruct> titles = new List<MainListTitleStruct>
            {
                title
            };
            mainListTitle.ItemsSource = titles;
            List<SynoFileSummary> fileSummaries = SynoMainList.GetFileSummariesFromMainFolder();
            mainList.ItemsSource = fileSummaries;
        }

        private void DynamicButtonClick(object sender, RoutedEventArgs e)
        {
            object TagValue = ((Button)sender).Tag;
            DynamicButton button = new DynamicButton((string)TagValue);
        }

        private void OpenSaveFolderClick(object sender, RoutedEventArgs e)
        {
            string currentSave = StorageConfiguration.GetSaveDirectory();
            Process.Start(currentSave);
        }

        private void ChangeSaveFolderClick(object sender, RoutedEventArgs e)
        {
            string newFolder = Configuration.SelectDirectory();
            saveDirectory.Text = $"Save Folder: {newFolder}";
        }
    }

    struct MainListTitleStruct
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string Title4 { get; set; }
    }
}
