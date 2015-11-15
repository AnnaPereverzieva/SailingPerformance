using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfService.Services.GpsService.Responses
{
    [DataContract]
    public class GetGpsByDateResponse : BaseResponse
    {
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