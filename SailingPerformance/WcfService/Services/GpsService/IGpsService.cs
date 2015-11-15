using System.ServiceModel;

namespace WcfService.Services.GpsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGpsService" in both code and config file together.
    [ServiceContract]
    public interface IGpsService
    {
        [OperationContract]
        string GetGpsDataByDate();
    }
}
