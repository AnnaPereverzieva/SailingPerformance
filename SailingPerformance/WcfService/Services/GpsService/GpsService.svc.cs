using System;
using WcfService.Services.GpsService.Requests;
using WcfService.Services.GpsService.Responses;

namespace WcfService.Services.GpsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GpsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GpsService.svc or GpsService.svc.cs at the Solution Explorer and start debugging.
    public class GpsService : IGpsService
    {
        public GetGpsByDateResponse GetGpsDataByDate(GetGpsByDateRequest request)
        {
            GetGpsByDateResponse response = new GetGpsByDateResponse();
            try
            {
                response.ErrorMessage = "ok,  id="+request.Id;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.ToString();
            }
            return response;
        }

        public GetAllGpsByDateResponse GetAllGpsDataByDate(GetAllGpsByDateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
