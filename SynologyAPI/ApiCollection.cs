using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class ApiCollection
    {
        private Query query;
        private Dictionary<string, APIType> Collection;
        private static ApiCollection Instance;

        public static ApiCollection GetApiCollection()
        {
            if (Instance == null)
            {
                Instance = new ApiCollection();
            }
            return Instance;
        }

        private ApiCollection()
        {
            query = new Query();
            InitiateCollection();
        }

        private void InitiateCollection()
        {
            query.CreateGeneralInfoQuery();
            NetworkGetRequest networkRequest = new NetworkGetRequest(query.GetUri());
            string response = networkRequest.SendGetRequest();
            APIGeneralInfoNetworkResponse apiResponse = APIResult.RetrieveResultsOfGeneralInfoNetworkResponse(response);
            Collection = apiResponse.Data;
        }

        public Dictionary<string, string> RetrieveLatestVersions(List<string> APIs)
        {
            Dictionary<string, string> versions = new Dictionary<string, string>();
            foreach(string apiName in APIs)
            {
                versions.Add(apiName, Collection.Where(a => a.Key == apiName).Select((a) => a.Value.MaxVersion).FirstOrDefault());
            }
            return versions;
        }
    }
}
