using System;
using AutoMapper;
using Dal;
using Dal.Repositories;
using Dal.Repositories.Contracts;

namespace WcfService.Services.BoatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BoatService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BoatService.svc or BoatService.svc.cs at the Solution Explorer and start debugging.
    public class BoatService : IBoatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBoatRepository _repositoryBoat;
        public BoatService()
        {
            SailingDbContext context = new SailingDbContext();
           // context.Database.Connection= "data source=H-PEREVERZIEVA2;initial catalog=SailingManagerDB;Integrated Security=True;Connect Timeout=35;Encrypt=False;TrustServerCertificate=False";
            _repositoryBoat = new BoatRepsitory(context);
            _unitOfWork = new UnitOfWork(context);
        }
        public BaseResponse AddBoatResponse(BoatRequest boatRequest)
        {
            var response = new BaseResponse();
            try
            {
                _unitOfWork.BeginTransaction();
                bool check = _repositoryBoat.CheckBoat(boatRequest.Model, boatRequest.Name);

                if (check)
                {
                    var boat = Mapper.Map<Boat>(boatRequest);
                    _repositoryBoat.Add(boat);
                    _unitOfWork.Commit();
                    response.IsSuccess = true;
                }
                else
                {
                    _unitOfWork.Commit();
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.ToString();
            }
            return response;
        }
    }
}
