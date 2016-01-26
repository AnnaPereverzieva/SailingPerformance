using System.Data.Entity;
using Dal.Repositories.Contracts;

namespace Dal.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly SailingDbContext _sailingDbContext;
        private DbContextTransaction _transaktion;
        public UnitOfWork(SailingDbContext sailingDbContext)
        {
            _sailingDbContext = sailingDbContext;
        }
        /// <summary>
        /// początek transakcji
        /// </summary>
        public void BeginTransaction()
        {
            if (_transaktion == null)
            {
                if (_sailingDbContext != null)
                {
                    _transaktion = _sailingDbContext.Database.BeginTransaction();
                }
            }
        }
        /// <summary>
        /// koniec transakcji 
        /// </summary>
        public void Commit()
        {
            if (_transaktion != null)
            {
                _sailingDbContext.SaveChanges();
                _transaktion.Commit();
                _transaktion = null;
            }
        }
    }
}
