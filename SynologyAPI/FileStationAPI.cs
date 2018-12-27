using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class FileStationAPI
    {
        private bool LoggedIn;
        private string SID;
        private NetworkGetRequest NetworkRequest;
        private Query Query;
        private readonly string Session = "FileStation";
        private readonly string FileStationList = "SYNO.FileStation.List";
        private readonly string FileStationSearch = "SYNO.FileStation.Search";
        private readonly string FileStationUpload = "SYNO.FileStation.Upload";
        public static Dictionary<string, string> APIVersions;
        private TopLevelSharedFolderListInfo SharedFoldersTopLevelData;
        private List<FileStationSharedFolder> SharedFolders;

        APIAuthentication Authentication;

        public FileStationAPI()
        {
            Authentication = APIAuthentication.CreateAPIAuthentication(Session);
            List<string> FileStationAPIs = new List<string>
            {
                FileStationList,
                FileStationSearch,
                FileStationUpload
            };
            ApiCollection apiCollection = ApiCollection.GetApiCollection();
            APIVersions = apiCollection.RetrieveLatestVersions(FileStationAPIs);
            SharedFoldersTopLevelData = new TopLevelSharedFolderListInfo();
            SharedFolders = new List<FileStationSharedFolder>();
        }

        public bool Login()
        {
            SID = Authentication.Login();
            LoggedIn = SID != String.Empty;
            return LoggedIn;
        }

        public bool Logout()
        {
            LoggedIn = Authentication.Logout();
            return LoggedIn;
        }

        public bool GetLoginStatus()
        {
            return LoggedIn;
        }

        public TopLevelSharedFolderListInfo RetrieveListOfSharedFolders()
        {
            Query = new Query();
            Query.SetPath("webapi/entry.cgi");
            NameValueCollection collection = new NameValueCollection();
            collection["_sid"] = SID;
            Query.BuildQueryWithParameters(FileStationList, APIVersions[FileStationList], "list_share", collection);
            NetworkRequest = new NetworkGetRequest(Query.GetUri());
            string jsonResponse = NetworkRequest.SendGetRequest();
            if (NetworkRequest.GetResponseStatusCode())
            {
                APITopLevelSharedFoldersNetworkResponse networkResponse = APIResult.RetrieveResultsOfSharedFoldersInfoNetworkResponse(jsonResponse);
                SharedFoldersTopLevelData = networkResponse.Data;
                PopulateSharedFolderCollection(SharedFoldersTopLevelData);
            }
            return SharedFoldersTopLevelData;
        }

        public List<SharedFileOrFolderInfo> ListAllFilesFromFolder(string TargetFolder)
        {
            if (SharedFolders.Count() < 1) RetrieveListOfSharedFolders();
            FileStationSharedFolder SharedFolder = SharedFolders.Where(a => a.Info.Name == TargetFolder).FirstOrDefault();
            return SharedFolder.GetAllFieldsFromFolder(SID);
        }

        public string SearchFilesFromTargetFolder(string TargetFolder)
        {
            if (SharedFolders.Count() < 1) RetrieveListOfSharedFolders();
            FileStationSharedFolder SharedFolder = SharedFolders.Where(a => a.Info.Name == TargetFolder).FirstOrDefault();
            if (String.IsNullOrEmpty(SharedFolder.Info.Name)) return $"Folder with name {TargetFolder} not found.";
            string AllFiles = SharedFolder.GetAllFilesFromSearch(SID);
            return AllFiles;       
        }

        public string UploadFileToChosenFolder(string TargetFolder, string FilePath)
        {
            if (SharedFolders.Count() < 1) RetrieveListOfSharedFolders();
            FileStationSharedFolder SharedFolder = SharedFolders.Where(a => a.Info.Name == TargetFolder).FirstOrDefault();
            if (!String.IsNullOrEmpty(SharedFolder.Info.Name))
            {
                var response = SharedFolder.UploadFile(SID, FilePath);
                return response;
            }
            return String.Empty;
        }

        public Dictionary<string, string> GetLatestVersionsList()
        {
            return APIVersions;
        }

        public string GetSID()
        {
            return SID;
        }

        private void PopulateSharedFolderCollection(TopLevelSharedFolderListInfo data)
        {
            SharedFolders = data.Shares.Select(a => new FileStationSharedFolder(a)).ToList();
        }
    }
}
