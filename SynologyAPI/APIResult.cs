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
        APINetworkResponse NetworkResponse;
        public APINetworkResponse RetrieveResultsOfNetworkResponse(string response)
        {
            NetworkResponse = JsonConvert.DeserializeObject<APINetworkResponse>(response);
            return NetworkResponse;
        }
    }
}
