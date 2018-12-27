using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class FileStationSharedFolder
    {
        public SharedFileOrFolderInfo Info;
        private readonly string FileStationSearch = "SYNO.FileStation.Search";
        private string TaskID;
        Query query;
        NetworkGetRequest request;

        public FileStationSharedFolder(SharedFileOrFolderInfo info)
        {
            Info = info;
        }

        public List<SharedFileOrFolderInfo> GetAllFieldsFromFolder(string SID)
        {
            query = new Query();
            query.SetPath("webapi/entry.cgi");
            NameValueCollection collection = new NameValueCollection();
            collection["folder_path"] = Info.Path;
            collection["_sid"] = SID;
            query.BuildQueryWithParameters("SYNO.FileStation.List", FileStationAPI.APIVersions["SYNO.FileStation.List"], "list", collection);
            request = new NetworkGetRequest(query.GetUri());
            string jsonResponse = request.SendGetRequest();
            List<SharedFileOrFolderInfo> FilesAndFolders = APIResult.RetrieveResultsOfFileLevelListNetworkResponse(jsonResponse).Data.Files;
            return FilesAndFolders.Where(a => !a.IsDirectory).ToList();
        }

        public string GetAllFilesFromSearch(string SID) 
        {
            if (String.IsNullOrEmpty(TaskID))
            {
                StartFolderSearch(SID);
                return "Commencing Search";
            }
            query = new Query();
            query.SetPath("webapi/entry.cgi");
            NameValueCollection collection = new NameValueCollection();
            collection["taskid"] = TaskID;
            collection["_sid"] = SID;
            query.BuildQueryWithParameters(FileStationSearch, FileStationAPI.APIVersions[FileStationSearch], "list", collection);
            request = new NetworkGetRequest(query.GetUri());
            string jsonResponse = request.SendGetRequest();
            return jsonResponse;
        }

        public StartFolderSearchResponse StartFolderSearch(string SID)
        {
            query = new Query();
            query.SetPath("webapi/entry.cgi");
            NameValueCollection collection = new NameValueCollection();
            collection["folder_path"] = Info.Path;
            collection["_sid"] = SID;
            query.BuildQueryWithParameters(FileStationSearch, FileStationAPI.APIVersions[FileStationSearch], "start", collection);
            request = new NetworkGetRequest(query.GetUri());
            string jsonResponse = request.SendGetRequest();
            StartFolderSearchResponse startResponse = APIResult.RetrieveResultsOfStartFolderNetworkResponse(jsonResponse);
            TaskID = startResponse.GetTaskID();
            return APIResult.RetrieveResultsOfStartFolderNetworkResponse(jsonResponse);
        }

        public string UploadFile(string SID, string FilePath)
        {
            NetworkUpload networkUpload = new NetworkUpload();
            query = new Query();
            query.SetPath("webapi/entry.cgi");
            query.ConstructUriWithPathOnly();
            string result = networkUpload.PostFileAsync(query.GetUri(), FilePath, SID, Info.Path).Result;
            return result;
        }
    }
}
