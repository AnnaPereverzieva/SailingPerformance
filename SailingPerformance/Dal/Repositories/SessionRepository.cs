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
        public void Add(Session entity)
        {
            _sailingDbContext.Sessions.Add(entity);
        }

        public void Update(Session entity)
        {
            _sailingDbContext.Sessions.AddOrUpdate(entity);
        }

        public void Delete(Session entity)
        {
            _sailingDbContext.Entry(entity).State = EntityState.Deleted;
        }

        public List<Session> GetSessions(DateTime start, DateTime stop, Guid idBoat)
        {
            List<Session> listSessions;
            using (_sailingDbContext = new SailingDbContext())
            {
                listSessions = _sailingDbContext.Sessions.Where(x => x.StartDate >= start && x.StopDate <= stop && x.IdBoat==idBoat).ToList();
            }
            return listSessions;
        }

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
