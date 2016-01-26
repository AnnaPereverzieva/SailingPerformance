using System.ServiceModel;

namespace WcfService.Services.SessionService
{
    /// <summary>
    /// definicja metod udostępnianych przez WCF
    /// </summary>
    [ServiceContract]
    interface ISessionService
    {
        [OperationContract]
        AddSessionResponse AddSession(AddSessionRequest addDataRequest);
    }
}
