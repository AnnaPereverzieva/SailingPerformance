using System;
using System.Runtime.Serialization;

namespace WcfService.Services.BoatService
{
    [DataContract]
    public class GetBoatResponse:BaseResponse
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}