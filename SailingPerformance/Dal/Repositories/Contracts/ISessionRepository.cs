using System;
using System.Collections.Generic;

namespace Dal.Repositories.Contracts
{
    public interface ISessionRepository: IRepository<Session>
    {
        List<Session> GetSessions(DateTime start, DateTime stop);
    }
}
