using System;
using System.Runtime.Serialization;

namespace WcfService.Services.BoatService
{
    [DataContract]
    public class BoatRequest
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public Guid IdBoat { get; set; }
    }
}