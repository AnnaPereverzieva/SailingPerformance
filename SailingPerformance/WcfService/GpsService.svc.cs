using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GpsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GpsService.svc or GpsService.svc.cs at the Solution Explorer and start debugging.
    public class GpsService : IGpsService
    {
        public string GetGpsDataByDate()
        {
            return "hello";
        }
    }
}
