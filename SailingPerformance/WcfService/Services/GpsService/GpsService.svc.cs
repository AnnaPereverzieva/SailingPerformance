using System;
using System.Collections.Generic;
using WcfService.Services.GpsService.Requests;
using WcfService.Services.GpsService.Responses;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.UI.WebControls;

namespace WcfService.Services.GpsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GpsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GpsService.svc or GpsService.svc.cs at the Solution Explorer and start debugging.
    public class GpsService : IGpsService
    {
        public GetGpsByDateResponse GetGpsDataByDate(GetGpsByDateRequest request) // pobieranie gps data przykladowe, zeby sprawdzic czy dziala service
        {
            string[]  dataGridPagerStyle = File.ReadAllLines(@"C:\Users\hpereverzieva\Desktop\11.txt");
            GetGpsByDateResponse response = new GetGpsByDateResponse();
            response.Time = new List<DateTime>();
            response.Latitude = new List<string>();
            response.Longitude = new List<string>();
            try
            {
                for (int i = 0; i < dataGridPagerStyle.Count(); i++)
                {
                    string entry = dataGridPagerStyle[i];
                    string item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
                    response.Date = Convert.ToDateTime(item);
                    response.Time.Add(Convert.ToDateTime(item));

                    entry = entry.Remove(0, entry.IndexOf(";", StringComparison.Ordinal) + 1);
                    item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
                    response.Latitude.Add(item);

                    entry = entry.Remove(0, entry.IndexOf(";", StringComparison.Ordinal) + 1);
                    item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
                    response.Longitude.Add(item);
                }
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
