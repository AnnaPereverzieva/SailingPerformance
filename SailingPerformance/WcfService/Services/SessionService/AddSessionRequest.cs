using System;
using System.Runtime.Serialization;

namespace WcfService.Services.SessionService
{
    [DataContract]
    public class AddSessionRequest
    {
        [DataMember]
        public Guid IdBoat { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime StopDate { get; set; }
    }
}