using System;
using System.Collections.Generic;

namespace Dal.Repositories.Contracts
{
    public interface IGpsDataRepository:IRepository<GPSData>
    {
        List<GPSData> GetGpsData(List<Guid> idSessions );
    }
}
