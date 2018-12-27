using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class APIRequest
    {
        private Query Query;
        private NetworkGetRequest NetworkRequest;
        private FileStationAPI FileStationRequest;

        public APIRequest()
        {
            Query = new Query();
        }

        public void GetGeneralInformation()
        {
            Query.CreateGeneralInfoQuery();
        }

        public bool ConfirmConnectivity()
        {
            Query.CreateStandardConnectivityTest();
            NetworkRequest = new NetworkGetRequest(Query.GetUri());
            NetworkRequest.SendGetRequest();
            return NetworkRequest.GetResponseStatusCode();
        }
    }
}
