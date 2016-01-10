using System;
using System.Runtime.Serialization;

namespace WcfService.Services.SessionService
{
    [DataContract]
    public class AddSessionResponse:BaseResponse
    {
        [DataMember]
        public Guid IdSession { get; set; }
    }
}