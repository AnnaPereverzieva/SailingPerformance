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

        //public Guid GetGuidLastSession()
        //{
        //    if(_sailingDbContext.Sessions.ToArray().Any())
        //    return _sailingDbContext.Sessions.ToArray().LastOrDefault().IdSession;
        //    return Guid.Empty;
        //}
    }
}
