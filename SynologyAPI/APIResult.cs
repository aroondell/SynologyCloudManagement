using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class APIResult
    {
        public static APIGeneralInfoNetworkResponse RetrieveResultsOfGeneralInfoNetworkResponse(string response)
        {
            APIGeneralInfoNetworkResponse NetworkResponse = JsonConvert.DeserializeObject<APIGeneralInfoNetworkResponse>(response);
            return NetworkResponse;
        }

        public static AuthenticationAPINetworkResponse RetrieveResultsOfBasicAPINetworkResponse(string response)
        {
            AuthenticationAPINetworkResponse NetworkResponse = JsonConvert.DeserializeObject<AuthenticationAPINetworkResponse>(response);
            return NetworkResponse;
        }

        public static APITopLevelSharedFoldersNetworkResponse RetrieveResultsOfSharedFoldersInfoNetworkResponse(string response)
        {
            APITopLevelSharedFoldersNetworkResponse NetworkResponse = JsonConvert.DeserializeObject<APITopLevelSharedFoldersNetworkResponse>(response);
            return NetworkResponse;
        }

        public static StartFolderSearchResponse RetrieveResultsOfStartFolderNetworkResponse(string response)
        {
            StartFolderSearchResponse NetworkResponse = JsonConvert.DeserializeObject<StartFolderSearchResponse>(response);
            return NetworkResponse;
        }

        public static APIFileLevelSharedFoldersNetworkResponse RetrieveResultsOfFileLevelListNetworkResponse(string response)
        {
            APIFileLevelSharedFoldersNetworkResponse NetworkResponse = JsonConvert.DeserializeObject<APIFileLevelSharedFoldersNetworkResponse>(response);
            return NetworkResponse;
        }
    }
}
