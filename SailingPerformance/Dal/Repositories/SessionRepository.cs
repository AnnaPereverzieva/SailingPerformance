using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Dal.Repositories.Contracts;

namespace Dal.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private SailingDbContext _sailingDbContext;

        public SessionRepository(SailingDbContext sailingDbContext = null)
        {
            _sailingDbContext = sailingDbContext;
        }
        /// <summary>
        /// dodanie sesji do bazy
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Session entity)
        {
            _sailingDbContext.Sessions.Add(entity);
        }
        /// <summary>
        /// aktualizacja sesji
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Session entity)
        {
            _sailingDbContext.Sessions.AddOrUpdate(entity);
        }
        /// <summary>
        /// usuięcie sesji
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Session entity)
        {
            _sailingDbContext.Entry(entity).State = EntityState.Deleted;
        }
        /// <summary>
        /// pobierania listy sesje wg daty i Id sesji
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="idBoat"></param>
        /// <returns></returns>
        public List<Session> GetSessions(DateTime start, DateTime stop, Guid idBoat)
        {
            List<Session> listSessions;
            using (_sailingDbContext = new SailingDbContext())
            {
                listSessions = _sailingDbContext.Sessions.Where(x => x.StartDate >= start && x.StopDate <= stop && x.IdBoat==idBoat).ToList();
            }
            return listSessions;
        }
        /// <summary>
        /// pobieranie zakresów dat wg Id łódki
        /// </summary>
        /// <param name="idBoat"></param>
        /// <returns></returns>
        public Dictionary<DateTime, DateTime> GetStartEndDate(Guid idBoat)
        {
            DateTime startDate;
            DateTime endDate;
            Dictionary<DateTime, DateTime> startEndDates = new Dictionary<DateTime, DateTime>();
            using (_sailingDbContext = new SailingDbContext())
            {
                startDate = (DateTime)_sailingDbContext.Sessions.Where(x => x.IdBoat == idBoat).Select(x => x.StartDate).Min();
                endDate = (DateTime)_sailingDbContext.Sessions.Where(x => x.IdBoat == idBoat).Select(x => x.StopDate).Max();
                startEndDates.Add(startDate, endDate);                
            }
            return startEndDates;
        }
    }
}
