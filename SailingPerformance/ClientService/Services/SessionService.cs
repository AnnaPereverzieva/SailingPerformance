using System;
using System.Collections.Generic;
using AutoMapper;
using ClientService.AutoMapper;
using ClientService.Model;
using Dal;
using Dal.Repositories;

namespace ClientService.Services
{
    public class SessionService
    {
        public SessionService()
        {
            AutoMapperConfiguration.ConfigureBoatMapping();
        }
        public List<SessionDto> GetSessions(DateTime start, DateTime stop, Guid idBoat)
        {
            var repository = new SessionRepository();
            List<Session> list = repository.GetSessions( start, stop, idBoat);
            return Mapper.Map<List<SessionDto>>(list);
        }

        public Dictionary<DateTime, DateTime> GetStartEndDates(Guid idBoat)
        {
            var repository = new SessionRepository();
            Dictionary<DateTime, DateTime> startEndDates = new Dictionary<DateTime, DateTime>(repository.GetStartEndDate(idBoat));
            return startEndDates;
        }

    }
}
