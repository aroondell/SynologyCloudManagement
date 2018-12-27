using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;

namespace SynologyAPI
{
    public class Query
    {
        private string HostUrl;
        private int HostPort;
        private string Path;
        private string Parameters;
        private Uri Uri;
        
        public Query()
        {
            HostUrl = ConfigurationManager.AppSettings["hostURL"].ToString();
            HostPort = Int32.Parse(ConfigurationManager.AppSettings["hostPort"]);
        }

        public void CreateStandardConnectivityTest()
        {
            Path = "webapi/query.cgi";
            ConstructUriWithPathOnly();
        }

        public void ConstructUriWithPathOnly()
        {
            UriBuilder uriBuilder = InitiateBuilder();
            Uri = uriBuilder.Uri;
        }

        public void SetPath(string path)
        {
            Path = path;
        }

        public void CreateGeneralInfoQuery()
        {
            Path = ConfigurationManager.AppSettings["generalInfo"].ToString();
            NameValueCollection collection = new NameValueCollection();
            collection["query"] = "all";
            BuildQueryWithParameters("SYNO.API.Info", "1", "query", collection);        
        }

        public void BuildQueryWithParameters(string api, string version, string method, NameValueCollection nameValues)
        {
            var collection = HttpUtility.ParseQueryString(String.Empty);
            collection["api"] = api;
            collection["version"] = version;
            collection["method"] = method;
            foreach(string key in nameValues)
            {
                collection[key] = nameValues[key];
            }
            Parameters = collection.ToString();
            ConstructUriWithQuery();
        }

        public Uri GetUri()
        {
            return Uri;
        }

        private void ConstructUriWithQuery()
        {
            UriBuilder uriBuilder = InitiateBuilder();
            uriBuilder.Query = Parameters;
            Uri = uriBuilder.Uri;
        }

        private UriBuilder InitiateBuilder()
        {
            UriBuilder uriBuilder = new UriBuilder(HostUrl);
            uriBuilder.Port = HostPort;
            uriBuilder.Path = Path;
            return uriBuilder;
        }
    }
}
