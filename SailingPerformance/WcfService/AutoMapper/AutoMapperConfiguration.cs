using AutoMapper;
using Dal;
using WcfService.Services.BoatService;
using WcfService.Services.SessionService;

namespace WcfService.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configuration()
        {
            Mapper.CreateMap<BoatRequest, Boat>();
            Mapper.CreateMap<AddSessionRequest, Session>();
        }
    }
}