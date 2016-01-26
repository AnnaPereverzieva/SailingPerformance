using System;
using AutoMapper;
using Dal;
using Dal.Repositories;
using Dal.Repositories.Contracts;
using WcfService.AutoMapper;

namespace WcfService.Services.SessionService
{
    /// <summary>
    /// implementacja interfejsu
    /// </summary>
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SessionRepository _repositorySession;
        private readonly BoatRepository _repositoryBoat;
        public SessionService()
        {
            SailingDbContext context = new SailingDbContext();
            _repositorySession = new SessionRepository(context);
            _repositoryBoat = new BoatRepository(context);
            _unitOfWork = new UnitOfWork(context);
            AutoMapperConfiguration.Configuration();
        }
        /// <summary>
        ///  metoda dodania sesji do bazy
        /// </summary>
        /// <param name="addDataRequest"></param>
        /// <returns>zwracana odpowiedź czy udało się dodać sesję do bazy</returns>
        public AddSessionResponse AddSession(AddSessionRequest addDataRequest)
        {
            var response = new AddSessionResponse();
            try
            {
                _unitOfWork.BeginTransaction();
                var check = _repositoryBoat.IsExistBoat(addDataRequest.IdBoat);
                if (check)
                {
                    Session session = Mapper.Map<Session>(addDataRequest);
                    session.IdSession = Guid.NewGuid();
                    _repositorySession.Add(session);
                    response.IdSession = session.IdSession;
                    _unitOfWork.Commit();
                    response.IsSuccess = true;
                }
                else
                {
                    _unitOfWork.Commit();
                    response.IsSuccess = false;
                    response.ErrorMessage = "Łódki o takim id nie istnieje";
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
