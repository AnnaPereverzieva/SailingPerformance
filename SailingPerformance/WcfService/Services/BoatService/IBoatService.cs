using System.ServiceModel;

namespace WcfService.Services.BoatService
{
    /// <summary>
    ///  definicja metod udostępnianych przez WCF
    /// </summary>
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
