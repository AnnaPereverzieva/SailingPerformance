using System;
using AutoMapper;
using Dal;
using Dal.Repositories;
using Dal.Repositories.Contracts;
using WcfService.AutoMapper;
using WcfService.Common;

namespace WcfService.Services.BoatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BoatService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BoatService.svc or BoatService.svc.cs at the Solution Explorer and start debugging.
    public class BoatService : IBoatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BoatRepository _repositoryBoat;
        public BoatService()
        {
            SailingDbContext context = new SailingDbContext();
            _repositoryBoat = new BoatRepository(context);
            _unitOfWork = new UnitOfWork(context);
            AutoMapperConfiguration.Configuration();
        }
        public BaseResponse AddBoat(BoatRequest boatRequest)
        {
            var response = new BaseResponse();
            try
            {
                _unitOfWork.BeginTransaction();
                var check = _repositoryBoat.GetGuidBoat(boatRequest.Model, boatRequest.Name);
                if (check == Guid.Empty)
                {
                    Boat boat = Mapper.Map<Boat>(boatRequest);
                 //   GuidExtensions guid = new GuidExtensions();
                 //   boat.IdBoat = guid.Increment(_repositoryBoat.GetGuidLastBoat());
                    _repositoryBoat.Add(boat);
                    _unitOfWork.Commit();
                    response.IsSuccess = true;
                }
                else
                {
                    _unitOfWork.Commit();
                    response.IsSuccess = false;
                    response.ErrorMessage = "Łódka o takiej nazwie i modelu juz istnieje";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.ToString();
            }
            return response;
        }

        public BaseResponse UpdateBoat(BoatRequest boatRequest)
        {
            var response = new BaseResponse();
            try
            {
                _unitOfWork.BeginTransaction();
                var check = _repositoryBoat.GetGuidBoat(boatRequest.Model, boatRequest.Name);
                if (check != Guid.Empty)
                {
                    Boat boat = Mapper.Map<Boat>(boatRequest);
                    _repositoryBoat.Update(boat);
                    _unitOfWork.Commit();
                    response.IsSuccess = true;
                }
                else
                {
                    _unitOfWork.Commit();
                    response.IsSuccess = false;
                    response.ErrorMessage = "Łódka o takiej nazwie i modelu nie istnieje";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.ToString();
            }
            return response;
        }

        public GetBoatResponse GetBoatId(BoatRequest boatRequest)
        {
            var response = new GetBoatResponse();
            try
            {
                _unitOfWork.BeginTransaction();
                var check = _repositoryBoat.GetGuidBoat(boatRequest.Model, boatRequest.Name);
                if (check != Guid.Empty)
                {
                    response.Id = check;
                    _unitOfWork.Commit();
                    response.IsSuccess = true;
                }
                else
                {
                    _unitOfWork.Commit();
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nie istnieje łódki o takim id";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.ToString();
            }
            return response;
        }

        public BaseResponse DeleteBoat(DeleteBoatRequest request)
        {
            var response = new BaseResponse();
            try
            {
                _unitOfWork.BeginTransaction();
                _repositoryBoat.Delete(new Boat { IdBoat = request.Id, Name = "Frosia", Model = "hgj"});
                _unitOfWork.Commit();
                response.IsSuccess = true;
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
