using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfService.Services.GpsService.Responses
{
    [DataContract]
    public class GetAllGpsByDateResponse:BaseResponse
    {
        [DataMember]
        public string Id { get; set; }
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