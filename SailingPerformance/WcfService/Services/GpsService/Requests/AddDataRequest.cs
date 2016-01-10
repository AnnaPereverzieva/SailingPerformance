using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfService.Services.GpsService.Requests
{
    [DataContract]
    public class AddDataRequest
    {
        [DataMember]
        public Guid IdBoat { get; set; }
        [DataMember]
        public List<GpsData> GpsDataList { get; set; }
    }
}