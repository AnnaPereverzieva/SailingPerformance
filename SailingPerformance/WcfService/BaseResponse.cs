using System.Runtime.Serialization;

namespace WcfService
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}