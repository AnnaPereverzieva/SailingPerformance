using System;
using System.Runtime.Serialization;

namespace WcfService.Services.GpsService.Requests
{
    [DataContract]
    public class GetGpsByDateRequest
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime DateFrom { get; set; }
        [DataMember]
        public DateTime DateTo { get; set; }
    }
}