using System;
using System.ServiceModel;

namespace WcfService.Services.BoatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBoatService" in both code and config file together.
    [ServiceContract]
    public interface IBoatService
    {
        [OperationContract]
        BaseResponse AddBoat(BoatRequest boatRequest);
        [OperationContract]
        BaseResponse UpdateBoat(BoatRequest boatRequest);
        [OperationContract]
        BaseResponse DeleteBoat(DeleteBoatRequest id);
        [OperationContract]
        GetBoatResponse GetBoatId(BoatRequest boatRequest);
    }
}
