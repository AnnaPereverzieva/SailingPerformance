using AutoMapper;
using ClientService.GpsServiceReference;
using ClientService.Model;
using ClientService.Responses;

namespace ClientService.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            ConfigureUserMapping();
        }

        private static void ConfigureUserMapping()
        {
            Mapper.CreateMap<GetGpsByDateResponse,GpsSingleByDateResponse>();           
        }
    }
}
