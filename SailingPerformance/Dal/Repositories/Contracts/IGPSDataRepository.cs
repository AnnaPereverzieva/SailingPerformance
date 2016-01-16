using System;
using System.Collections.Generic;

namespace Dal.Repositories.Contracts
{
    public interface IGpsDataRepository:IRepository<GPSData>
    {
        List<GPSData> GetGpsData(Guid idSession);
        Dictionary<float, float> GetWindSpeedMinMax(Guid idSession);
    }
}
