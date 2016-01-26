using AutoMapper;
using Dal;
using WcfService.Services.BoatService;
using WcfService.Services.GpsService.Requests;
using WcfService.Services.SessionService;

namespace WcfService.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// służy do mapowania obiektów, mapuje objekty requestu na obiekty bazy
        /// </summary>
        public static void Configuration()
        {
            Mapper.CreateMap<BoatRequest, Boat>();
            Mapper.CreateMap<AddSessionRequest, Session>();
            Mapper.CreateMap<GpsData, GPSData>();
        }
    }
}