using System;
using System.Data.Entity.Validation;
using WcfService.Services.GpsService.Requests;
using System.Linq;
using AutoMapper;
using Dal;
using Dal.Repositories;
using Dal.Repositories.Contracts;
using WcfService.AutoMapper;
using WcfService.Services.SessionService;

namespace WcfService.Services.GpsService
{
    /// <summary>
    /// implementacja interfejsu
    /// </summary>
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
        /// <summary>
        ///  udostępniona metoda dodania danych gps do bazy
        /// </summary>
        /// <param name="addDataRequest"></param>
        /// <returns>zwracana odpowiedź czy udało się dodać dane do bazy</returns>
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
    }
}
