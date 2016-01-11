using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfService.Services.GpsService.Requests
{
    [DataContract]
    public class GpsData
    {
        [DataMember]
        public DateTime SecondsFromStart { get; set; }
        [DataMember]
        public double BoatSpeed { get; set; }
        [DataMember]
        public double BoatDirection { get; set; }
        [DataMember]
        public double WindSpeed { get; set; }
        [DataMember]
        public double WindDirection { get; set; }
        [DataMember]
        public string GeoHeight { get; set; }
        [DataMember]
        public string GeoWidth { get; set; }
    }
}