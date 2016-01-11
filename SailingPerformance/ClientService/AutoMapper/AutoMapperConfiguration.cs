using AutoMapper;
using ClientService.Model;
using Dal;

namespace ClientService.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            ConfigureBoatMapping();
        }

        public static void ConfigureBoatMapping()
        {
            Mapper.CreateMap<Boat, BoatDto>();
            Mapper.CreateMap<Session, SessionDto>();
            Mapper.CreateMap<GPSData, DataGps>();
        }
    }
}
