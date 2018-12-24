using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynologyAPI;

namespace TestLibrary
{
    [TestClass]
    public class APIResultTest
    {
        Query query;
        NetworkRequest networkRequest;
        APIResult result;

        [TestInitialize]
        public void TestInitialize()
        {
            query = new Query();
            result = new APIResult();
        }

        [TestMethod]
        public void CanDeserialiseGeneralInfoResponse()
        {
            query.CreateGeneralInfoQuery();
            networkRequest = new NetworkRequest(query.GetUri());
            string response = networkRequest.SendGetRequest();
            APINetworkResponse apiResponse = result.RetrieveResultsOfNetworkResponse(response);
            bool legitResponse = apiResponse.Success == true && apiResponse.Data.Any();
            Assert.AreEqual(legitResponse, true);
        }
    }
}
