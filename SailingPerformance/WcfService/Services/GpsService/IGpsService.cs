using System.ServiceModel;
using WcfService.Services.GpsService.Requests;

namespace WcfService.Services.GpsService
{
    /// <summary>
    /// definicja metod udostępnianych przez WCF
    /// </summary>
    [ServiceContract]
    public interface IGpsService
    {
        [OperationContract]
        BaseResponse AddData(AddDataRequest addDataRequest);
    }
}
