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
        
        /// <summary>
        /// dodanie łódki do bazy
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Boat entity)
        {
            _sailingDbContext.Boats.Add(entity);
        }
        /// <summary>
        /// aktualizacja danych o łódce
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Boat entity)
        {
            _sailingDbContext.Boats.AddOrUpdate(entity);
        }
        /// <summary>
        /// usunięcie łódki z bazy
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Boat entity)
        {
            _sailingDbContext.Entry(entity).State=EntityState.Deleted;
        }
        /// <summary>
        /// pobierania listy łódek z bazy
        /// </summary>
        /// <returns></returns>
        public List<Boat> GetBoats()
        {
            List<Boat> listBoats;
            using (_sailingDbContext = new SailingDbContext())
            {
                listBoats=_sailingDbContext.Boats.ToList();
            }
            return listBoats;
        }
        /// <summary>
        /// sprawdzenie czy istnieje łódka w bazie
        /// </summary>
        /// <param name="idBoat"></param>
        /// <returns></returns>
        public bool IsExistBoat(Guid idBoat)
        {
            var boat =_sailingDbContext.Boats.FirstOrDefault(x => x.IdBoat == idBoat);
            if (boat != null) return true;
            return false;
        }
        /// <summary>
        /// pobieranie Id łódki
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Guid GetGuidBoat(string model, string name)
        {
            var obj = _sailingDbContext.Boats.FirstOrDefault(n => n.Model == model || n.Name == name);
            if (obj != null) return obj.IdBoat;
            return Guid.Empty;
        }
    }
}
