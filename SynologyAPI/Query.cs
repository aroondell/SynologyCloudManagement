using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            BuildParameters("SYNO.API.Info", "1", "query", "all");
            ConstructUriWithQuery();
        }

        private void BuildParameters(string api, string version, string method, string query)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["api"] = api;
            parameters["version"] = version;
            parameters["method"] = method;
            parameters["query"] = query;
            Parameters = parameters.ToString();
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
