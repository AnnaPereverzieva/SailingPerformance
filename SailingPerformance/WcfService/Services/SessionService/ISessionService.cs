using System.ServiceModel;

namespace WcfService.Services.SessionService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISessionService" in both code and config file together.
    [ServiceContract]
    interface ISessionService
    {
        [OperationContract]
        AddSessionResponse AddSession(AddSessionRequest addDataRequest);
    }
}
