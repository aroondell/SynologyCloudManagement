using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynologyAPI;

namespace TestLibrary
{
    [TestClass]
    public class NetworkRequestTest
    {
        Query query;
        NetworkRequest networkRequest;

        [TestInitialize]
        public void TestInitialize()
        {
            query = new Query();
        }

        [TestMethod]
        public void CanTestNetworkConnectivity()
        {
            bool ShouldFail = ConstructRequestWithPathAndReturnStatus("webapu");
            bool ShouldPass = ConstructRequestWithPathAndReturnStatus("webapi/query.cgi");
            Assert.AreEqual(ShouldFail, false);
            Assert.AreEqual(ShouldPass, true);
        }

        [TestMethod]
        public void CanSendGetRequest()
        {
            query.CreateGeneralInfoQuery();
            networkRequest = new NetworkRequest(query.GetUri());
            string response = networkRequest.SendGetRequest();
            Assert.AreEqual(response.Length > 0, true);
        }

        private bool ConstructRequestWithPathAndReturnStatus(string path)
        {
            query.SetPath(path);
            query.ConstructUriWithPathOnly();
            Uri generatedUri = query.GetUri();
            networkRequest = new NetworkRequest(query.GetUri());
            networkRequest.SendGetRequest();
            return networkRequest.GetResponseStatusCode();
        }
    }
}
