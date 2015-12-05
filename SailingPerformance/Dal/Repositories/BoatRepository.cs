using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Dal.Repositories.Contracts;

namespace Dal.Repositories
{
    public class BoatRepository : IBoatRepository
    {
        private readonly SailingDbContext _sailingDbContext;

        public BoatRepository(SailingDbContext sailingDbContext)
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
            _sailingDbContext.Boats.Remove(entity);
        }

        public Guid GetGuidBoat(string model, string name)
        {
            var obj = _sailingDbContext.Boats.FirstOrDefault(n => n.Model == model || n.Name == name);
            if (obj != null) return obj.IdBoat;
            return Guid.Empty;
        }

        public Guid GetGuidLastBoat()
        {
            var obj = _sailingDbContext.Boats.ToArray().LastOrDefault();
            if (obj != null)
                return obj.IdBoat;
            return Guid.Empty;
        }
    }
}
