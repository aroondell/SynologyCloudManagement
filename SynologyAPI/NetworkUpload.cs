using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class NetworkUpload
    {
        string Boundary;

        public NetworkUpload()
        {
            Boundary = "-----------" + DateTime.Now.Ticks.ToString("x");
        }

        public async Task<string> PostFileAsync(Uri postUrl, string file, string SID, string folderPath)
        {          
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            byte[] FileBytes = File.ReadAllBytes(file);
            
            request.Method = "POST";
            request.ContentType = $"multipart/form-data; boundary={Boundary}";
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;

            byte[] buffer = new byte[4096];
            int count = 0;
            int length = 0;
            Stream requestStream = request.GetRequestStream();
            requestStream = CreateFormData(requestStream, SID, folderPath);
            FileStream inputStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            while ((count = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                await requestStream.WriteAsync(buffer, 0, count);
                length += count;
                Debug.WriteLine(length + " / " + FileBytes.Length);
            }
            inputStream.Close();

            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + Boundary + "--\r\n");
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

          
            WebResponse resp = await request.GetResponseAsync();

            using (Stream stream = resp.GetResponseStream())
            {
                StreamReader respReader = new StreamReader(stream);
                return respReader.ReadToEnd();
            }
        }

        private Stream CreateFormData(Stream rs, string SID, string folderPath)
        {
            NameValueCollection nvc = new NameValueCollection
            {
                {"api", "SYNO.FileStation.Upload" },
                {"version", FileStationAPI.APIVersions["SYNO.FileStation.Upload"]},
                {"method", "upload"},
                {"_sid", SID},
                {"path", folderPath },
                {"create_parents", "false"},
                {"overwrite", "true" }
            };

            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + Boundary + "\r\n");
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            foreach (string key in nvc.Keys)
            {
                rs.Write(boundaryBytes, 0, boundaryBytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }

            rs.Write(boundaryBytes, 0, boundaryBytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "file", "TimeToSayGoodbye-InstumentalVersion.mp3", "application/octet-stream");
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            return rs;
        }
    }
}
