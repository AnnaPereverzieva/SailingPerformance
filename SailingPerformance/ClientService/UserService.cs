using System;
using System.Threading.Tasks;
using ClientService.GpsServiceReference;
using ClientService.UserServiceReference;

namespace ClientService
{
    public class UserService
    {
        public async Task<bool> CheckUser(string login, string password)
        {
            var result = await Task.Run(() =>
            {
                UserServiceClient client = new UserServiceClient();
                return client.CheckUser(login, password);
            });
            return result;
        }

        public async Task<string> GetGpsDataByDate(DateTime from, DateTime to)
        {
            var result = await Task.Run(() =>
            {
                GpsServiceClient client = new GpsServiceClient();
                var response = client.GetGpsDataByDateAsync(new GetGpsByDateRequest { DateFrom = from, DateTo = to });
                return response.Result.ErrorMessage;
            });
            return result;
        }
    }
}
