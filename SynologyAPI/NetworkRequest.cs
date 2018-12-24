using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class NetworkRequest
    {
        Uri Uri;
        bool ResponseStatus;

        public NetworkRequest(Uri uri)
        {
            Uri = uri;
        }

        public string SendGetRequest()
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpResponseMessage response = client.GetAsync(Uri).Result;
                ResponseStatus = response.IsSuccessStatusCode;
                var responseInBytes = response.Content.ReadAsByteArrayAsync().Result;
                return Encoding.UTF8.GetString(responseInBytes);
            }
        }

        public bool GetResponseStatusCode()
        {
            return ResponseStatus;
        }
    }
}
