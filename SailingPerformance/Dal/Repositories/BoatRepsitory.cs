using System.Data.Entity;
using System.Linq;
using Dal.Repositories.Contracts;

namespace Dal.Repositories
{
    public class BoatRepsitory : IBoatRepository
    {
        private readonly SailingDbContext _sailingDbContext;
        public BoatRepsitory(SailingDbContext sailingDbContext)
        {
            _sailingDbContext = sailingDbContext;
        }
        public void Add(Boat entity)
        {
            _sailingDbContext.Boats.Add(entity);
        }

        public void Update(Boat entity)
        {
            _sailingDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Boat entity)
        {
            _sailingDbContext.Boats.Remove(entity);
        }

        public bool CheckBoat(string model, string name)
        {
            var obj = _sailingDbContext.Boats.First(n => n.Model == model || n.Name == name);
            if (obj != null) return false;
            return true;
        }
    }
}
