using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynologyAPI;

namespace TestLibrary
{
    [TestClass]
    public class QueryTest
    {
        Query query;

        [TestInitialize]
        public void TestInitialize()
        {
            query = new Query();
        }

        [TestMethod]
        public void CanBuildPathOnlyQuery()
        {
            string correctResult = "https://carthaven.com:7766/webapi/query.cgi";
            query.SetPath("webapi/query.cgi");
            query.ConstructUriWithPathOnly();
            Uri generatedUri = query.GetUri();
            Assert.AreEqual(correctResult, generatedUri.ToString());
        }

        [TestMethod]
        public void CanBuildGeneralRequestQuery()
        {
            string correctResult = "https://carthaven.com:7766/webapi/query.cgi?api=SYNO.API.Info&version=1&method=query&query=all";
            query.CreateGeneralInfoQuery();
            Uri generatedUri = query.GetUri();
            Assert.AreEqual(correctResult, generatedUri.ToString());
        }
    }
}
