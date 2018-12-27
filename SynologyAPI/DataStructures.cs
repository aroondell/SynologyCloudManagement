using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public struct APIError
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public Dictionary<string, string> Errors { get; set; }
    }

    #region GeneralInfo
    public struct APIGeneralInfoNetworkResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, APIType> Data { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public APIError Error { get; set; }
    }

    public struct APIType
    {
        [JsonProperty(PropertyName = "maxVersion")]
        public string MaxVersion { get; set; }

        [JsonProperty(PropertyName = "minVersion")]
        public string MinVersion { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }

    #endregion

    #region SYNO.API.Auth
    public struct AuthenticationAPINetworkResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, string> Data { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public APIError Error { get; set; }
    }

    #endregion

    #region SYNO.FileStation.List Top Level

    public struct APITopLevelSharedFoldersNetworkResponse
    {
        [JsonProperty(PropertyName = "data")]
        public TopLevelSharedFolderListInfo Data { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public APIError Error { get; set; }
    }

    public struct SharedFileOrFolderInfo
    {
        [JsonProperty(PropertyName = "isdir")]
        public bool IsDirectory { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }

    public struct TopLevelSharedFolderListInfo
    {
        [JsonProperty(PropertyName = "shares")]
        public List<SharedFileOrFolderInfo> Shares { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }

    #endregion

    #region SYNO.FileStation.List Files Level

    public struct APIFileLevelSharedFoldersNetworkResponse
    {
        [JsonProperty(PropertyName = "data")]
        public FileLevelSharedFolderListInfo Data { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public APIError Error { get; set; }
    }

    public struct FileLevelSharedFolderListInfo
    {
        [JsonProperty(PropertyName = "files")]
        public List<SharedFileOrFolderInfo> Files { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }

    #endregion

    #region SYNO.FileStation.Search

    public class StartFolderSearchResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, string> Data { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        public string GetTaskID()
        {
            return "{" + Data["taskid"] + "}";
        }
    }

    #endregion
}
