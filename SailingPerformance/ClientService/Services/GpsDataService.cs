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
        /// <summary>
        /// pobieranie listy sesje z bazy
        /// </summary>
        /// <param name="idSession"></param>
        /// <returns></returns>
        public List<DataGps> GetSessions( Guid idSession)
        {
            var repository = new GpsDataRepository();
            List<GPSData> list = repository.GetGpsData(idSession);
            return Mapper.Map<List<DataGps>>(list);
        }
    }
}
