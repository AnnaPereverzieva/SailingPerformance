using System;
using System.Runtime.Serialization;

namespace WcfService.Services.GpsService.Responses
{
    [DataContract]
    public class GetGpsByDateRequest
    {
        [DataMember]
        public Guid IdSession { get; set; }
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
        public double GeoHeight { get; set; }
        [DataMember]
        public double GeoWidth { get; set; }
    }
}