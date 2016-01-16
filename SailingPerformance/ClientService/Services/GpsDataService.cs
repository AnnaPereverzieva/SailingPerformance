using System;
using System.Collections.Generic;
using AutoMapper;
using ClientService.AutoMapper;
using ClientService.Model;
using Dal;
using Dal.Repositories;

namespace ClientService.Services
{
    public class GpsDataService
    {
        public GpsDataService()
        {
            AutoMapperConfiguration.ConfigureBoatMapping();
        }
        public List<DataGps> GetSessions( Guid idSession)
        {
            var repository = new GpsDataRepository();
            List<GPSData> list = repository.GetGpsData(idSession);
            return Mapper.Map<List<DataGps>>(list);
        }
        public Dictionary<float, float> GetWindSpeedMinMax(Guid idSession)
        {
            var repository = new GpsDataRepository();
            Dictionary<float, float> windSpeedMinMax = new Dictionary<float, float>(repository.GetWindSpeedMinMax(idSession));
            return windSpeedMinMax;
            
        }
        public Dictionary<float, float> GetWindDirectionMinMax(Guid idSession)
        {
            var repository = new GpsDataRepository();
            Dictionary<float, float> windDirectionMinMax = new Dictionary<float, float>(repository.GetWindDirectionMinMax(idSession));
            return windDirectionMinMax;

        }
    }
}
