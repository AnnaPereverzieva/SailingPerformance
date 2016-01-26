using System;
using System.Runtime.Serialization;

namespace WcfService.Services.BoatService
{
    [DataContract]
    public class DeleteBoatRequest
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}