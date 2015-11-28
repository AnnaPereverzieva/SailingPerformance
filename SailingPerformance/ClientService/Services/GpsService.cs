using System;
using System.Threading.Tasks;
using AutoMapper;
using ClientService.Responses;

namespace ClientService.Services
{
    public class GpsService
    {
    //    public async Task<GpsSingleByDateResponse> GetGpsDataByDate(DateTime from, DateTime to, int id)
    //    {
    //        var result = await Task.Run(() =>
    //        {
    //            GpsServiceClient client = new GpsServiceClient();
    //            GetGpsByDateResponse response = client.GetGpsDataByDate(new GetGpsByDateRequest { DateFrom = from, DateTo = to, Id =id });
    //            return Mapper.Map<GpsSingleByDateResponse>(response);
    //        });
    //        return result;
    //    }
    }
}
