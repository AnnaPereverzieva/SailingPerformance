using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfService.Services.GpsService.Requests
{
    [DataContract]
    public class PutDataRequest
    {
        [DataMember]
        public string IdBoat { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public List<string> Latitude { get; set; }
        [DataMember]
        public List<string> Longitude { get; set; }
        [DataMember]
        public List<DateTime> Time { get; set; }
        [DataMember]
        public List<string> StrengthWind { get; set; }
        [DataMember]
        public List<string> DirectionWind { get; set; }
    }
}