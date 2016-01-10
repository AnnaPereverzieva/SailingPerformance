using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using WcfService.Services.GpsService.Requests;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.UI.WebControls;
using AutoMapper;
using Dal;
using Dal.Repositories;
using Dal.Repositories.Contracts;
using WcfService.AutoMapper;
using WcfService.Services.SessionService;

namespace WcfService.Services.GpsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GpsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GpsService.svc or GpsService.svc.cs at the Solution Explorer and start debugging.
    public class GpsService:IGpsService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GpsDataRepository _repositoryGpsData;

        public GpsService()
        {
            SailingDbContext context = new SailingDbContext();
            _repositoryGpsData = new GpsDataRepository(context); 
            _unitOfWork = new UnitOfWork(context);
            AutoMapperConfiguration.Configuration();
        }
        public BaseResponse AddData(AddDataRequest addDataRequest)
        {
            var response = new BaseResponse();
            try
            {
                var addSession = new SessionService.SessionService();
                AddSessionResponse res = addSession.AddSession(new AddSessionRequest
                {
                    IdBoat = addDataRequest.IdBoat,
                    StartDate = addDataRequest.GpsDataList.Max(x => x.SecondsFromStart),
                    StopDate = addDataRequest.GpsDataList.Min(x => x.SecondsFromStart)
                });

                _unitOfWork.BeginTransaction();
                if (res.IsSuccess)
                {
                    foreach (var item in addDataRequest.GpsDataList)
                    {
                        GPSData data= Mapper.Map<GPSData>(item);
                        data.BoatDirection = 4;
                        data.BoatSpeed = 6;
                        data.WindDirection = 2;
                        data.WindSpeed = 9;
                        data.IdSession =new Guid("4ADAEDD9-DAB7-E511-82AF-ACB57D99B460");
                        data.IdGPSData = Guid.NewGuid();
                        _repositoryGpsData.Add(data);
                    }
           
                    _unitOfWork.Commit();
                    response.IsSuccess = true;
                }
                else
                {
                    _unitOfWork.Commit();
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nie udalo sie utworzyc sesji.";
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        response.ErrorMessage += validationError.PropertyName + "\n";
                        response.ErrorMessage += validationError.ErrorMessage + "\n";
                    }
                }
                response.IsSuccess = false;
            }
          
            return response;
        }



        //public GetGpsByDateResponse GetGpsDataByDate(GetGpsByDateRequest request) // pobieranie gps data przykladowe, zeby sprawdzic czy dziala service
        //{
        //    string[]  dataGridPagerStyle = File.ReadAllLines(@"C:\Users\hpereverzieva\Desktop\11.txt");
        //    GetGpsByDateResponse response = new GetGpsByDateResponse();
        //    response.Time = new List<DateTime>();
        //    response.Latitude = new List<string>();
        //    response.Longitude = new List<string>();
        //    try
        //    {
        //        for (int i = 0; i < dataGridPagerStyle.Count(); i++)
        //        {
        //            string entry = dataGridPagerStyle[i];
        //            string item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
        //            response.Date = Convert.ToDateTime(item);
        //            response.Time.Add(Convert.ToDateTime(item));

        //            entry = entry.Remove(0, entry.IndexOf(";", StringComparison.Ordinal) + 1);
        //            item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
        //            response.Latitude.Add(item);

        //            entry = entry.Remove(0, entry.IndexOf(";", StringComparison.Ordinal) + 1);
        //            item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
        //            response.Longitude.Add(item);
        //        }
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.ErrorMessage = ex.ToString();
        //    }
        //    return response;



    }
}
