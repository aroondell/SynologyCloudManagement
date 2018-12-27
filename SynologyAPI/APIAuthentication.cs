using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class APIAuthentication
    {
        private NetworkGetRequest NetworkRequest;
        private Query Query;
        private string Session;
        private static APIAuthentication Instance;

        private APIAuthentication(string session)
        {
            Session = session;
        }

        public static APIAuthentication CreateAPIAuthentication(string session)
        {
            if (Instance == null)
            {
                Instance = new APIAuthentication(session);
            }
            return Instance;
        }

        public string Login()
        {
            string username = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            NameValueCollection collection = new NameValueCollection();
            collection["account"] = username;
            collection["passwd"] = password;
            collection["session"] = Session;
            collection["format"] = "sid";
            Query = new Query();
            Query.SetPath("webapi/auth.cgi");
            Query.BuildQueryWithParameters("SYNO.API.Auth", "4", "login", collection);
            NetworkRequest = new NetworkGetRequest(Query.GetUri());
            string jsonResponse = NetworkRequest.SendGetRequest();
            if (NetworkRequest.GetResponseStatusCode())
            {              
                AuthenticationAPINetworkResponse networkResponse = APIResult.RetrieveResultsOfBasicAPINetworkResponse(jsonResponse);
                var entry = networkResponse.Data.Where(a => a.Key == "sid").ToList();
                if (entry.Count > 0)
                {
                    return entry.First().Value;
                }
            }
            return String.Empty;
        }

        public bool Logout()
        {
            NameValueCollection collection = new NameValueCollection();
            collection["session"] = Session;
            Query.SetPath("webapi/auth.cgi");
            Query.BuildQueryWithParameters("SYNO.API.Auth", "4", "logout", collection);
            NetworkRequest = new NetworkGetRequest(Query.GetUri());
            NetworkRequest.SendGetRequest();
            return NetworkRequest.GetResponseStatusCode();
        }

    }
}
