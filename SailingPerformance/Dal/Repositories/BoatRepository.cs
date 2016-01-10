using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Dal.Repositories.Contracts;

namespace Dal.Repositories
{
    public class BoatRepository : IBoatRepository
    {
        private SailingDbContext _sailingDbContext;

        public BoatRepository(SailingDbContext sailingDbContext=null)
        {
            _sailingDbContext = sailingDbContext;
        }
        

        public void Add(Boat entity)
        {
            _sailingDbContext.Boats.Add(entity);
        }

        public void Update(Boat entity)
        {
            _sailingDbContext.Boats.AddOrUpdate(entity);
        }

        public void Delete(Boat entity)
        {
            _sailingDbContext.Entry(entity).State=EntityState.Deleted;
        }

        public List<Boat> GetBoats()
        {
            List<Boat> listBoats;
            using (_sailingDbContext = new SailingDbContext())
            {
                listBoats=_sailingDbContext.Boats.ToList();
            }
            return listBoats;
        }

        public Guid GetGuidBoat(string model, string name)
        {
            var obj = _sailingDbContext.Boats.FirstOrDefault(n => n.Model == model || n.Name == name);
            if (obj != null) return obj.IdBoat;
            return Guid.Empty;
        }
    }
}
