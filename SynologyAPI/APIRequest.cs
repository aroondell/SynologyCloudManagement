using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynologyAPI
{
    public class APIRequest
    {
        private Query Connection;
        private FileStationAPI FileStationRequest;

        public APIRequest()
        {
            Connection = new Query();
        }

        public void GetGeneralInformation()
        {
            Connection.CreateGeneralInfoQuery();
        }
    }
}
