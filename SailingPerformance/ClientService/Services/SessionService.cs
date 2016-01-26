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

        /// <summary>
        /// pobieranie listy sesji z bazy
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="idBoat"></param>
        /// <returns></returns>
        public List<SessionDto> GetSessions(DateTime start, DateTime stop, Guid idBoat)
        {
            var repository = new SessionRepository();
            List<Session> list = repository.GetSessions( start, stop, idBoat);
            return Mapper.Map<List<SessionDto>>(list);
        }
		
        /// <summary>
        /// pobieranie zakresu dat pojedyńczej łódki
        /// </summary>
        /// <param name="idBoat"></param>
        /// <returns></returns>
        public Dictionary<DateTime, DateTime> GetStartEndDates(Guid idBoat)
        {
            var repository = new SessionRepository();
            Dictionary<DateTime, DateTime> startEndDates = new Dictionary<DateTime, DateTime>(repository.GetStartEndDate(idBoat));
            return startEndDates;
        }

    }
}
