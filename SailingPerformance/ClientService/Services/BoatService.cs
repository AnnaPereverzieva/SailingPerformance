﻿using System.Collections.Generic;
using AutoMapper;
using ClientService.AutoMapper;
using ClientService.Model;
using Dal;
using Dal.Repositories;

namespace ClientService.Services
{
    public class BoatService
    {
        public BoatService()
        {
            AutoMapperConfiguration.ConfigureBoatMapping();
        }
        /// <summary>
        /// pobieranie listy łódek z bazy
        /// </summary>
        /// <returns></returns>
        public List<BoatDto> GetBoats()
        {
            var repository=new BoatRepository();
            List<Boat> list = repository.GetBoats();
            return Mapper.Map<List<BoatDto>>(list);          
        }
    }
}
