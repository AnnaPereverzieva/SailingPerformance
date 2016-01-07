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
        public DateTime SecondsFromStart { get; set; }
        [DataMember]
        public double BoatSpeed { get; set; }
        [DataMember]
        public double BoatDirection{ get; set; }
        [DataMember]
        public double WindSpeed { get; set; }
        [DataMember]
        public double WindDirection { get; set; }
        [DataMember]
        public double GeoHeight { get; set; }
        [DataMember]
        public double GeoWidth { get; set; }
    }
}