using AutoMapper;
using Dal;
using WcfService.Services.BoatService;

namespace WcfService.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configuration()
        {
            Mapper.CreateMap<BoatRequest, Boat>();            
        }
    }
}