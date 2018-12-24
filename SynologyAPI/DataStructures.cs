using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public struct APINetworkResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, APIType> Data { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }

    public struct APIType
    {
        [JsonProperty(PropertyName = "maxVersion")]
        public string MaxVersion { get; set; }

        [JsonProperty(PropertyName = "mminVersion")]
        public string MinVersion { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }
}
